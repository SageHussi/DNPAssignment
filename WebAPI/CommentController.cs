using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepo;

    public CommentsController(ICommentRepository commentRepo)
    {
        this.commentRepo = commentRepo;
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> AddComment([FromBody] CreateCommentDto request)
    {
        Comment comment = new Comment { Body = request.Body, UserId = request.UserId, PostId = request.PostId };
        Comment created = await commentRepo.AddAsync(comment);
        return Created($"/comments/{created.CommentId}", ToDto(created));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommentDto>> GetSingleComment(int id)
    {
        Comment comment = await commentRepo.GetSingleAsync(id);
        return Ok(ToDto(comment));
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommentDto>> GetComments([FromQuery] int? userId, [FromQuery] int? postId)
    {
        IQueryable<Comment> comments = commentRepo.GetManyAsync();

        if (userId.HasValue)
        {
            comments = comments.Where(c => c.UserId == userId.Value);
        }

        if (postId.HasValue)
        {
            comments = comments.Where(c => c.PostId == postId.Value);
        }

        List<CommentDto> dtos = comments.ToList().Select(ToDto).ToList();
        return Ok(dtos);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateComment(int id, [FromBody] UpdateCommentDto request)
    {
        Comment existing = await commentRepo.GetSingleAsync(id);
        Comment comment = new Comment { CommentId = id, Body = request.Body, UserId = existing.UserId, PostId = existing.PostId };
        await commentRepo.UpdateAsync(comment);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteComment(int id)
    {
        await commentRepo.DeleteAsync(id);
        return NoContent();
    }

    private static CommentDto ToDto(Comment comment)
    {
        return new CommentDto { Id = comment.CommentId, Body = comment.Body, UserId = comment.UserId, PostId = comment.PostId };
    }
}