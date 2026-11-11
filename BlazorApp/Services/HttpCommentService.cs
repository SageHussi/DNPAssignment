using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpCommentService : ICommentService
{
    private static readonly JsonSerializerOptions jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient client;

    public HttpCommentService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<CommentDto> AddCommentAsync(CreateCommentDto request)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("comments", request);
        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}:{responseContent}");
        }

        CommentDto returned = JsonSerializer.Deserialize<CommentDto>(responseContent, jsonOptions) ?? throw new JsonException();
        return returned;
    }

    public async Task<CommentDto> GetCommentAsync(int id)
    {
        HttpResponseMessage response = await client.GetAsync($"comments/{id}");
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}:{content}");
        }

        var comment = JsonSerializer.Deserialize<CommentDto>(content, jsonOptions) ?? throw new JsonException();
        return comment;
    }

    public async Task<ICollection<CommentDto>> GetCommentsAsync(int? postId, int? userId)
    {
        List<string> queryParts = new();

        if (postId.HasValue)
        {
            queryParts.Add("postId=" + postId.Value);
        }

        if (userId.HasValue)
        {
            queryParts.Add("userId=" + userId.Value);
        }

        string url = "comments";
        if (queryParts.Count > 0)
        {
            url += "?" + string.Join("&", queryParts);
        }

        HttpResponseMessage response = await client.GetAsync(url);
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}:{content}");
        }

        ICollection<CommentDto> comments = JsonSerializer.Deserialize<ICollection<CommentDto>>(content, jsonOptions) ?? [];
        return comments;
    }

    public async Task UpdateCommentAsync(int id, UpdateCommentDto request)
    {
        HttpResponseMessage response = await client.PutAsJsonAsync($"comments/{id}", request);
        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}:{responseContent}");
        }
    }

    public async Task DeleteCommentAsync(int id)
    {
        HttpResponseMessage response = await client.DeleteAsync($"comments/{id}");
        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}:{responseContent}");
        }
    }
}