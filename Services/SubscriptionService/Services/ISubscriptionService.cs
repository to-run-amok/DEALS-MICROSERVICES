public interface ISubscriptionService
{
    Task<SubscriptionResponseDto> SubscribeAsync(SubcriptionCreateDto dto,int subscriberId);

    Task<IEnumerable<SubscriptionResponseDto>>GetMySubscriptionsAsync(int subscriberId);

    Task UnsubscribeAsync(int subscriptionId,int subscriberId);
    Task NotifySubscribersAsync(CropCreatedEvent message);

}