using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcPostRepository : IPostRepository
{
    private readonly AppDbContext ctx;

    public EfcPostRepository(AppDbContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<Post> AddAsync(Post Post)
    {
        await ctx.Posts.AddAsync(Post);
        await ctx.SaveChangesAsync();
        return Post;
    }

    public async Task UpdateAsync(Post Post)
    {
        if (!await ctx.Posts.AnyAsync(u => u.PostId == Post.PostId))
        {
            throw new InvalidOperationException($"Post with id {Post.PostId} not found");
        }
        ctx.Posts.Update(Post);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        Post? existing = await ctx.Posts.SingleOrDefaultAsync(u => u.PostId == id);
        if (existing == null)
        {
            throw new InvalidOperationException($"Post with id {id} not found");
        }
        ctx.Posts.Remove(existing);
        await ctx.SaveChangesAsync();
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        Post? Post = await ctx.Posts.SingleOrDefaultAsync(u => u.PostId == id);
        if (Post == null)
        {
            throw new InvalidOperationException($"Post with id {id} not found");
        }
        return Post;
    }

    public IQueryable<Post> GetManyAsync()
    {
        return ctx.Posts.AsQueryable();
    }
}