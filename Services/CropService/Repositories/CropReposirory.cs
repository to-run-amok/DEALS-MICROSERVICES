using Microsoft.EntityFrameworkCore;

public class CropRepository : ICropRepository
{
    private readonly CropDbContext _context;

    public CropRepository(CropDbContext context)
    {
        _context = context;
    }

    public async Task<Crop?> GetByIdAsync(int id)
    {
        return await _context.DealsCrops.FindAsync(id);

    }
    public async Task<List<Crop>> GetAllAsync()
    {
        return await _context.DealsCrops.ToListAsync();
    }
    public async Task<List<Crop>> GetByFarmerIdAsync(int farmerId)
    {
        return await _context.DealsCrops
                .Where(c=>c.FarmerId ==farmerId)
                .ToListAsync();
    }
    public async Task AddAsync(Crop crop)
    {
        await _context.DealsCrops.AddAsync(crop);
    }
    public void Update(Crop crop)
    {
        _context.DealsCrops.Update(crop);
    }
    public void Delete(Crop crop)
    {
        _context.DealsCrops.Remove(crop);
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}