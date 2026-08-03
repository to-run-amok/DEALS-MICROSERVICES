public interface IReviewService
{
    Task<ReviewResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<ReviewResponseDto>> GetReviewByFarmerIdAync(int farmerId);
    Task<IEnumerable<ReviewResponseDto>> GetReviewByCropIdAync(int cropId);
    Task<IEnumerable<ReviewResponseDto>> GetReviewByBuyerIdAync(int buyerId);
    Task<ReviewResponseDto> CreateReviewAsync(ReviewCreateDto dto, int buyerId);
}