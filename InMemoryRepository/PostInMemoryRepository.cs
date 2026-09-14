namespace InMemoryRepository;
using Entities;
using RepositoryContracts;

public class PostInMemoryRepository : IPostRepository
{
    private List<Post> posts = new();

    public PostInMemoryRepository()
    {
        posts.Add(new Post { PostId = 1, Title = "My first post :)", Body = "This is the body of my first post", UserId = 1 });
        posts.Add(new Post { PostId = 2, Title = "My second post :)", Body = "This is the body of my second post", UserId = 2 });
        posts.Add(new Post { PostId = 3, Title = "My third post :)", Body = "This is the body of my third post", UserId = 3 });
        posts.Add(new Post { PostId = 4, Title = "My fourth post :)", Body = "This is the body of my fourth post", UserId = 4 });
        posts.Add(new Post { PostId = 5, Title = "My fifth post :)", Body = "This is the body of my fifth post", UserId = 5 });
    }

    public Task<Post> AddAsync(Post post)
    {
        post.PostId = posts.Any()
            ? posts.Max(p => p.PostId) + 1
            : 1;
        posts.Add(post);
        return Task.FromResult(post);
    }

    public Task UpdateAsync(Post post)
    {
        Post? existingPost = posts.SingleOrDefault(p => p.PostId == post.PostId);
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{post.PostId}' not found");
        }

        posts.Remove(existingPost);
        posts.Add(post);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Post? postToRemove = posts.SingleOrDefault(p => p.PostId == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        posts.Remove(postToRemove);
        return Task.CompletedTask;
    }

    public Task<Post> GetSingleAsync(int id)
    {
        Post? post = posts.SingleOrDefault(p => p.PostId == id);
        if (post is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }
        return Task.FromResult(post);
    }

    public IQueryable<Post> GetManyAsync()
    {
        return posts.AsQueryable();
    }
}