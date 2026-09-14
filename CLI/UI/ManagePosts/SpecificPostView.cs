using RepositoryContracts;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class SpecificPostView
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;

    public SpecificPostView(IPostRepository postRepository, ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
    }

    public async Task ViewSpecificPosts()
    {
        Console.WriteLine("Enter a post ID:");
        string postIdInput = Console.ReadLine();
        int postId = int.Parse(postIdInput);
        
        Post specificPost = await postRepository.GetSingleAsync(postId);
        Console.WriteLine("Specific post: " + specificPost.Title + ", " + specificPost.Body);

        IQueryable<Comment> comments = commentRepository.GetManyAsync();

        foreach (Comment comment in comments)
        {
            if (comment.PostId == specificPost.PostId)
            {
                Console.WriteLine("Comment: " + comment.Body);
            }
        }
    }
}