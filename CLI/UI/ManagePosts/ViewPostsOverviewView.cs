namespace CLI.UI.ManagePosts;
using Entities;
using RepositoryContracts;

public class ViewPostsOverviewView
{
    private readonly IPostRepository postRepository;

    public ViewPostsOverviewView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    public void ViewPostsOverview()
    {
        IQueryable<Post> posts = postRepository.GetManyAsync();

        foreach (Post post in posts)
        {
            Console.WriteLine("Post Title: "+ post.Title + ", " + " Post ID: " + post.PostId);
        }
    }
}