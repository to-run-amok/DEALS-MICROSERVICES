public interface ICropRepository
{
    Task<Crop?> GetByIdAsync(int id);

    Task<List<Crop>> GetAllAsync();

    Task<List<Crop>> GetByFarmerIdAsync(int farmerId);

    Task AddAsync(Crop crop);

    void Update(Crop crop);
    
    void Delete(Crop crop);

    Task SaveChangesAsync();
}