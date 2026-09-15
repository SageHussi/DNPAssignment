namespace CLI.UI.ManagePosts;
using Entities;
using RepositoryContracts;

public class CreatePostView
{
    private readonly IPostRepository postRepository;
    
    public CreatePostView(IPostRepository postRepository)
    {
      this.postRepository = postRepository;
    }

    public async Task CreatePostAsync()
    {
        Console.WriteLine("Enter Title:");
        string Title = Console.ReadLine();
        
        Console.WriteLine("Enter Body:");
        string Body = Console.ReadLine();
        
        Console.WriteLine("Enter UserId:");
        string userIdInput = Console.ReadLine();
        int userId = int.Parse(userIdInput);
        
        Post newPost = new Post{Title = Title, Body = Body, UserId = userId};
        Post createdPost = await postRepository.AddAsync(newPost);
        
        Console.WriteLine("Post added successfully: " + createdPost.PostId);
    }
}