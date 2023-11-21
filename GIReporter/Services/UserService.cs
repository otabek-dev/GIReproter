using GIReporter.Commands.Interfaces;
using GIReporter.DB;
using GIReporter.Models;
using Microsoft.EntityFrameworkCore;

namespace GIReporter.Services;

public class UserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsAdminAsync(long userId)
    {
        var isAdmin = await _context.Users.AnyAsync(u =>  u.Id == userId);
        return isAdmin;
    }

    public async Task<State> GetUserStateAsync(long userId)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);
       
        if (user is null)
            return State.Any;

        return user.UserState;
    }

    public async Task SetUserStateAsync(long userId, State userState)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
            return;

        user.UserState = userState;
        await _context.SaveChangesAsync();
    }

    public async Task<string?> GetInProcessCommand(long userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
            return null;

        return user.InProcessCommandName ?? null;
    }

    public async Task SetInProcessCommand(long userId, string? command)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
            return;

        user.InProcessCommandName = command;
        await _context.SaveChangesAsync();
    }
}

