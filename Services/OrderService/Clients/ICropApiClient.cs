public interface ICropApiClient
{
    Task<CropDto?> GetCropAsync(int cropId);

    Task MarkAsSoldAsync(int cropId);
}