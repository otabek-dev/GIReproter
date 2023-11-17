﻿using GIReporter.DB;
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

    public bool IsAdmin(long userId)
    {
        var isAdmin = _context.Users.Any(u =>  u.Id == userId);
        return isAdmin;
    }

    public State GetUserState(long userId)
    {
        var user = _context.Users
            .FirstOrDefault(u => u.Id == userId);
       
        if (user is null)
            return State.All;

        return user.UserState;
    }

    public async Task SetUserState(long userId, State userState)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
            return;

        user.UserState = userState;
        await _context.SaveChangesAsync();
    }
}
