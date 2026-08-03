using Microsoft.EntityFrameworkCore;
public static class SeedData
{
    public static async Task SeedAdminAsync(IdentityDbContext context)
    {
        if (await context.DealsUsers.AnyAsync(x => x.Role == UserRole.Admin))
        {
            return;
        }

        var admin = new User
        {
            Name = "System Admin",
            Email = "admin@deals.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role = UserRole.Admin,
            IsActive = true
        };

        context.DealsUsers.Add(admin);

        await context.SaveChangesAsync();
    }
}