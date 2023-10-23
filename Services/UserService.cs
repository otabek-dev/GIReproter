using HisoBOT.DB;
using Microsoft.EntityFrameworkCore;

namespace HisoBOT.Services;

public class UserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public bool IsAdmin(long userId)
    {
        var isAdmin = _context.Users.Any(u =>  u.Id == userId);
        return isAdmin;
    }

    public bool IsTypeProjectName(long userId)
    {
        var isTypeProjectName = _context.Users
            .FirstOrDefault(u => u.Id == userId);
       
        if (isTypeProjectName is null)
            return false;

        return isTypeProjectName.IsTypeProjectName;
    }

    public async Task SetIsTypeProjectName(long userId, bool isTypeProjectName)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
            return;

        user.IsTypeProjectName = isTypeProjectName;
        await _context.SaveChangesAsync();
    }
}

