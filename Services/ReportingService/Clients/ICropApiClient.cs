public interface ICropApiClient
{
    Task<IEnumerable<CropDto>> GetAllCropsAsync();

}