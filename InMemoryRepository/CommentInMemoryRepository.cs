using RepositoryContracts;
using Entities;

namespace InMemoryRepository;

public class commentInMemoryRepository : ICommentRepository
{
    private List<Comment> comments = new();

    public Task<Comment> AddAsync(Comment comment)
    {
        comment.CommentId = comments.Any()
            ? comments.Max(p => p.CommentId) + 1
            : 1;
        comments.Add(comment);
        return Task.FromResult(comment);
    }

    public Task UpdateAsync(Comment comment)
    {
        Comment? existingcomment = comments.SingleOrDefault(u =>u.CommentId  == comment.CommentId);
        if (existingcomment is null)
        {
            throw new InvalidOperationException(
                $"comment with ID '{comment.CommentId}' not found");
        }

        comments.Remove(existingcomment);
        comments.Add(comment);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Comment? commentToRemove = comments.SingleOrDefault(u =>u.CommentId  == id);
        if (commentToRemove is null)
        {
            throw new InvalidOperationException(
                $"comment with ID '{id}' not found");
        }

        comments.Remove(commentToRemove);
        return Task.CompletedTask;
    }

    public Task<Comment> GetSingleAsync(int id)
    {
        Comment? comment = comments.SingleOrDefault(u => u.CommentId == id);
        if (comment is null)
        {
            throw new InvalidOperationException(
                $"comment with ID '{id}' not found");
        }
        return Task.FromResult(comment);
    }

    public IQueryable<Comment> GetManyAsync()
    {
        return comments.AsQueryable();
    }
}