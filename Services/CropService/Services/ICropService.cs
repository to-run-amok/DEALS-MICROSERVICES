public interface ICropService
{
    Task<CropResponseDto> CreateAsync(CropCreateDto dto,int farmerId);

    Task<CropResponseDto> GetByIdAsync(int id);

    Task<IEnumerable<CropResponseDto>> GetAllAsync();

    Task<IEnumerable<CropResponseDto>> GetMyCropsAsync(int farmerId);

    Task MarkAsSoldAsync(int cropId);

    Task<CropResponseDto> UpdateAsync(int cropId,int farmerId,CropUpdateDto dto);

    Task DeleteAsync(int cropId,int farmerId);
}