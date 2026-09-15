namespace CLI.UI.ManagePosts;
using Entities;
using RepositoryContracts;

public class CreateCommentView
{
    private readonly ICommentRepository commentRepository;

    public CreateCommentView(ICommentRepository commentRepository)
    {
        this.commentRepository = commentRepository;
    }

    public async Task CreateCommentAsync()
    {
        Console.WriteLine("Enter Body:");
        string Body = Console.ReadLine();
        
        Console.WriteLine("Enter UserId:");
        string userIdInput = Console.ReadLine();
        int userId = int.Parse(userIdInput);
        
        Console.WriteLine("Enter PostId:");
        string postIdInput = Console.ReadLine();
        int postId = int.Parse(postIdInput);
        
        Comment newComment = new Comment{Body = Body, UserId = userId, PostId = postId};
        Comment createdComment = await commentRepository.AddAsync(newComment);
        
        Console.WriteLine("Comment added successfully: " + createdComment.CommentId);
    }
}