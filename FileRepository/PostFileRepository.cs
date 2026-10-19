namespace FileRepository;
using System.Text.Json;
using Entities;
using RepositoryContracts;

public class PostFileRepository : IPostRepository
{
    private readonly string filePath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<Post> AddAsync(Post post)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        int maxId = posts.Count > 0 ? posts.Max(p => p.PostId) : 0;
        post.PostId = maxId + 1;
        posts.Add(post);
        postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postsAsJson);
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson);
        Post? existingPost = posts.SingleOrDefault(p => p.PostId == post.PostId);
        if (existingPost is null)
        {
            throw new InvalidOperationException($"Post with ID '{post.PostId}' not found");
        }
        posts.Remove(existingPost);
        posts.Add(post);
        postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postsAsJson);
    }

    public async Task DeleteAsync(int id)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson);
        Post? existingPost = posts.SingleOrDefault(p => p.PostId == id);
        if (existingPost is null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found");
        }
        posts.Remove(existingPost);
        postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postsAsJson);
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson);
        Post? existingPost = posts.SingleOrDefault(p => p.PostId == id);
        if (existingPost is null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found");
        }
        return existingPost;
    }

    public IQueryable<Post> GetManyAsync()
        {
            string postsAsJson = File.ReadAllText(filePath);
            List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
            return posts.AsQueryable();
        }
}