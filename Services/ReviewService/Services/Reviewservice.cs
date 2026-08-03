public class Reviewservice : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IOrderApiClient _orderApiClient;


    public Reviewservice(IReviewRepository ReviewRepository, IOrderApiClient orderApiClient)
    {
        _reviewRepository = ReviewRepository;
        _orderApiClient = orderApiClient;
    }

    public async Task<ReviewResponseDto?> GetByIdAsync(int id)
    {
        var review = await _reviewRepository.GetByIdAsync(id);

        if(review==null)
        {
            throw new KeyNotFoundException("The review listing does not exist.");
        }

        return MatToResponseDto(review);
    }

    public async Task<IEnumerable<ReviewResponseDto>> GetReviewByFarmerIdAync(int farmerId)
    {
        var reviews = await _reviewRepository.GetByFarmerIdAsync(farmerId);

        return reviews.Select(MatToResponseDto);
    }
    public async Task<IEnumerable<ReviewResponseDto>> GetReviewByCropIdAync(int cropId)
    {
        var reviews = await _reviewRepository.GetByCropIdAsync(cropId);

        return reviews.Select(MatToResponseDto);
    }
    

    public async Task<IEnumerable<ReviewResponseDto>> GetReviewByBuyerIdAync(int buyerId)
    {
        var reviews = await _reviewRepository.GetByBuyerIdAsync(buyerId);

        return reviews.Select(MatToResponseDto);
    }   

    public async Task<ReviewResponseDto> CreateReviewAsync(ReviewCreateDto dto,int buyerId)
    {
        var order = await _orderApiClient.GetOrderAsync(dto.OrderId);

        if (order == null)
        {
            throw new KeyNotFoundException("Order not found.");
        }

        if (order.BuyerId != buyerId)
        {
            throw new UnauthorizedAccessException("You can only review your own orders.");
        }

        if (dto.Rating < 1 || dto.Rating > 5)
        {
            throw new InvalidOperationException("Rating must be between 1 and 5.");
        }

        var existingReview =await _reviewRepository.GetByOrderIdAsync(dto.OrderId);

        if (existingReview != null)
        {
            throw new InvalidOperationException("Review already exists for this order.");
        }

        var review = new Review
        {
            CropId = order.CropId,
            FarmerId = order.FarmerId,
            BuyerId = order.BuyerId,
            Rating = dto.Rating,
            Comment = dto.Comment.Trim(),
            CreatedAt = DateTime.UtcNow,
            OrderId = dto.OrderId
        };

        await _reviewRepository.AddAsync(review);

        await _reviewRepository.SaveChangesAsync();

        return MatToResponseDto(review);
    }

    private static ReviewResponseDto MatToResponseDto(Review review)
    {
        return new ReviewResponseDto
        {
            Id = review.Id,
            Rating = review.Rating,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt,
            BuyerId = review.BuyerId,
            CropId = review.CropId,
            FarmerId = review.FarmerId,
            OrderId = review.OrderId
        };
    }
}