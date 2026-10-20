using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepo;

    public PostsController(IPostRepository postRepo)
    {
        this.postRepo = postRepo;
    }

    [HttpPost]
    public async Task<ActionResult<PostDto>> AddPost([FromBody] CreatePostDto request)
    {
        Post post = new Post { Title = request.Title, Body = request.Body, UserId = request.UserId };
        Post created = await postRepo.AddAsync(post);
        return Created($"/posts/{created.PostId}", ToDto(created));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDto>> GetSinglePost(int id)
    {
        Post post = await postRepo.GetSingleAsync(id);
        return Ok(ToDto(post));
    }

    [HttpGet]
    public ActionResult<IEnumerable<PostDto>> GetPosts([FromQuery] string? titleContains, [FromQuery] int? userId)
    {
        IQueryable<Post> posts = postRepo.GetManyAsync();

        if (!string.IsNullOrWhiteSpace(titleContains))
        {
            posts = posts.Where(p => p.Title.Contains(titleContains));
        }

        if (userId.HasValue)
        {
            posts = posts.Where(p => p.UserId == userId.Value);
        }

        List<PostDto> dtos = posts.ToList().Select(ToDto).ToList();
        return Ok(dtos);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdatePost(int id, [FromBody] UpdatePostDto request)
    {
        Post existing = await postRepo.GetSingleAsync(id);
        Post post = new Post { PostId = id, Title = request.Title, Body = request.Body, UserId = existing.UserId };
        await postRepo.UpdateAsync(post);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeletePost(int id)
    {
        await postRepo.DeleteAsync(id);
        return NoContent();
    }

    private static PostDto ToDto(Post post)
    {
        return new PostDto { Id = post.PostId, Title = post.Title, Body = post.Body, UserId = post.UserId };
    }
}