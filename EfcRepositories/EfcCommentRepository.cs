using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcCommentRepository : ICommentRepository
{
    private readonly AppDbContext ctx;

    public EfcCommentRepository(AppDbContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<Comment> AddAsync(Comment Comment)
    {
        await ctx.Comments.AddAsync(Comment);
        await ctx.SaveChangesAsync();
        return Comment;
    }

    public async Task UpdateAsync(Comment Comment)
    {
        if (!await ctx.Comments.AnyAsync(u => u.CommentId == Comment.CommentId))
        {
            throw new InvalidOperationException($"Comment with id {Comment.CommentId} not found");
        }
        ctx.Comments.Update(Comment);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        Comment? existing = await ctx.Comments.SingleOrDefaultAsync(u => u.CommentId == id);
        if (existing == null)
        {
            throw new InvalidOperationException($"Comment with id {id} not found");
        }
        ctx.Comments.Remove(existing);
        await ctx.SaveChangesAsync();
    }

    public async Task<Comment> GetSingleAsync(int id)
    {
        Comment? Comment = await ctx.Comments.SingleOrDefaultAsync(u => u.CommentId == id);
        if (Comment == null)
        {
            throw new InvalidOperationException($"Comment with id {id} not found");
        }
        return Comment;
    }

    public IQueryable<Comment> GetManyAsync()
    {
        return ctx.Comments.AsQueryable();
    }
}