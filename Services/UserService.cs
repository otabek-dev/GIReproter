using HisoBOT.DB;
using HisoBOT.Models;

namespace HisoBOT.Services;

public class UserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public List<User> GetUsers()
    {

        var users = _context.Users.ToList();
        return users;
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

    public void SetTypeProject(long userId, bool isTypeProjectName)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);

        if (user is null)
            return;

        user.IsTypeProjectName = isTypeProjectName;
        _context.SaveChanges();
    }
}

