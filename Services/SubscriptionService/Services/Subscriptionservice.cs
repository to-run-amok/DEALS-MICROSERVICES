using Microsoft.VisualBasic;

public class Subscriptionservice : ISubscriptionService
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IUserApiClient _userApiClient;
    private readonly IEmailService _emailService;

    public Subscriptionservice(ISubscriptionRepository subscriptionRepository,
                                IUserApiClient userApiClient,
                                IEmailService emailService)
    {
        _subscriptionRepository = subscriptionRepository;
        _userApiClient = userApiClient;
        _emailService = emailService;   
    }
    public async Task<SubscriptionResponseDto> SubscribeAsync(SubcriptionCreateDto dto,int subscriberId)
    {
        bool exists = await _subscriptionRepository.ExistsAsync(subscriberId,dto.CropType);

        
        if(exists)
        {
            throw new InvalidOperationException("Already subscribed to this crop.");
        }
        

        if(string.IsNullOrWhiteSpace(dto.CropType))
        {
            throw new InvalidOperationException("Crop type is required.");
        }

        var newSubs = new Subscription
        {
           CropType = dto.CropType.Trim().ToLowerInvariant(),
           SubscribedAt = DateTime.UtcNow,
           SubscriberId = subscriberId

        };

        await _subscriptionRepository.AddAsync(newSubs);
        await _subscriptionRepository.SaveChangesAsync();

        return MapToResponseDto(newSubs);
    }

    public async Task<IEnumerable<SubscriptionResponseDto>>GetMySubscriptionsAsync(int subscriberId)
    {
        var subs = await _subscriptionRepository.GetBySubscriberIdAsync(subscriberId);

        return subs.Select(MapToResponseDto);
    }
    public async Task UnsubscribeAsync(int subscriptionId,int subscriberId)
    {
        var subs = await _subscriptionRepository.GetByIdAsync(subscriptionId);

        if(subs==null)
        {
            throw new KeyNotFoundException("Subscription listing does not exist.");
        }

        if(subs.SubscriberId != subscriberId)
        {
            throw new UnauthorizedAccessException("You are not authorized to Unsuscribe.");
        }

        _subscriptionRepository.Delete(subs);
        await _subscriptionRepository.SaveChangesAsync();
    }   

    public async Task NotifySubscribersAsync(CropCreatedEvent message)
    {
        var subscriptions = await _subscriptionRepository.GetByCropTypeAsync(message.CropType);

        if(!subscriptions.Any())
        {
            Console.WriteLine($"No subscribers found for {message.CropType}");
            return;
        }

        
        
        foreach(var subscription in subscriptions)
        {
            try
            {
                var user =
                    await _userApiClient.GetUserAsync(
                        subscription.SubscriberId);

                

                if(user == null ||
                string.IsNullOrWhiteSpace(user.Email))
                {
                    continue;
                }

                Console.WriteLine($"Email: {user.Email}");

                await _emailService.SendAsync(
                    user.Email,
                    "New Crop Available",
                    $"A new crop has been listed.\n\n" +
                    $"Crop Type: {message.CropType}\n" +
                    $"Crop Name: {message.CropName}\n" +
                    $"Crop Id: {message.CropId}");

                Console.WriteLine(
                    $"Email sent to {user.Email}");
            }
            catch(Exception ex)
            {
                Console.WriteLine(
                    $"Failed to send email: {ex.Message}");
            }
        }
    }

    private static SubscriptionResponseDto MapToResponseDto(Subscription subscription)
    {
        return new SubscriptionResponseDto
        {
            Id = subscription.Id,
            CropType = subscription.CropType,
            SubscribedAt = subscription.SubscribedAt,
        };
    }
}