using RepositoryContracts;
using Entities;

namespace InMemoryRepository;

public class CommentInMemoryRepository : ICommentRepository
{
    private List<Comment> comments = new();

    public CommentInMemoryRepository()
    {
        comments.Add(new Comment { CommentId = 1, Body = "First comment", PostId = 1, UserId = 1 });
        comments.Add(new Comment { CommentId = 2, Body = "Second comment", PostId = 2, UserId = 2 });
        comments.Add(new Comment { CommentId = 3, Body = "Third comment", PostId = 3, UserId = 3 });
        comments.Add(new Comment { CommentId = 4, Body = "Fourth comment", PostId = 4, UserId = 4 });
        comments.Add(new Comment { CommentId = 5, Body = "Fifth comment", PostId = 5, UserId = 5 });
    }

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