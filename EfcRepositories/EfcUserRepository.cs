using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcUserRepository : IUserRepository
{
    private readonly AppDbContext ctx;

    public EfcUserRepository(AppDbContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<User> AddAsync(User user)
    {
        await ctx.Users.AddAsync(user);
        await ctx.SaveChangesAsync();
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        if (!await ctx.Users.AnyAsync(u => u.UserId == user.UserId))
        {
            throw new InvalidOperationException($"User with id {user.UserId} not found");
        }
        ctx.Users.Update(user);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        User? existing = await ctx.Users.SingleOrDefaultAsync(u => u.UserId == id);
        if (existing == null)
        {
            throw new InvalidOperationException($"User with id {id} not found");
        }
        ctx.Users.Remove(existing);
        await ctx.SaveChangesAsync();
    }

    public async Task<User> GetSingleAsync(int id)
    {
        User? user = await ctx.Users.SingleOrDefaultAsync(u => u.UserId == id);
        if (user == null)
        {
            throw new InvalidOperationException($"User with id {id} not found");
        }
        return user;
    }

    public IQueryable<User> GetManyAsync()
    {
        return ctx.Users.AsQueryable();
    }
}