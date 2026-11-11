using ApiContracts;

namespace BlazorApp.Services;

public interface ICommentService
{
    public Task<CommentDto> AddCommentAsync(CreateCommentDto request);
    public Task UpdateCommentAsync(int id, UpdateCommentDto request);
    public Task DeleteCommentAsync(int id);
    public Task<CommentDto> GetCommentAsync(int id);
    public Task<ICollection<CommentDto>> GetCommentsAsync(int? postId, int? userId);
}