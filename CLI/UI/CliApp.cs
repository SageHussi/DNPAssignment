using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;

namespace CLI.UI;

using InMemoryRepository;
using RepositoryContracts;

public class CliApp
{
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;

    public CliApp(IUserRepository userRepository, ICommentRepository commentRepository, IPostRepository postRepository)
    {
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
    }

    public async Task StartAsync()
    {
        Console.WriteLine("CLI started");

        CreateUserView createUserView = new CreateUserView(userRepository);
        await createUserView.createUserAsync();
        
        CreatePostView createPostView = new CreatePostView(postRepository);
        await createPostView.CreatePostAsync();
        
        CreateCommentView createCommentView = new CreateCommentView(commentRepository);
        await createCommentView.CreateCommentAsync();

        ViewPostsOverviewView postsOverviewView = new ViewPostsOverviewView(postRepository);
        postsOverviewView.ViewPostsOverview();
        
        SpecificPostView specificPostView = new SpecificPostView(postRepository, commentRepository);
        await specificPostView.ViewSpecificPosts();
    }
}