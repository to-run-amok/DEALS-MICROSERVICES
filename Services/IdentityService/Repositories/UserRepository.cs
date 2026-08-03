using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly IdentityDbContext _context;

    public UserRepository(IdentityDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.DealsUsers.FirstOrDefaultAsync(u => u.Email==email);

    }
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.DealsUsers.FindAsync(id);
    }
    public async Task AddAsync(User user)
    {
        await _context.DealsUsers.AddAsync(user);
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

}