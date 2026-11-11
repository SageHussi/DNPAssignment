using System.Runtime.CompilerServices;
using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpPostService : IPostService
{
    private static readonly JsonSerializerOptions jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
    private readonly HttpClient client;
    
    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }
    
    
    public async Task<PostDto> AddPostAsync(CreatePostDto request)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("posts", request);
        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}:{responseContent}");
        }

        PostDto returned = JsonSerializer.Deserialize<PostDto>(responseContent, jsonOptions) ?? throw new JsonException();
        return returned;
    }

    public async Task<PostDto> GetPostAsync(int id)
    {
        HttpResponseMessage response = await client.GetAsync($"posts/{id}");
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}:{content}");
        }

        var post = JsonSerializer.Deserialize<PostDto>(content, jsonOptions);
        return post;
    }

    public async Task<ICollection<PostDto>> GetPostsAsync(string? titleContains, int? userId)
    {
        List<string> queryParts = new();
        
        if (!string.IsNullOrWhiteSpace(titleContains))
        {
            queryParts.Add("?titleContains=" + Uri.EscapeDataString(titleContains));
        }
        
        if (userId.HasValue)
        {
            queryParts.Add("userId=" + userId.Value);
        }
        
        string url = "posts";
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

        ICollection<PostDto> posts = JsonSerializer.Deserialize<ICollection<PostDto>>(content, jsonOptions) ?? [];
        return posts;
    }

    public async Task UpdatePostAsync(int id, UpdatePostDto request)
    {
        HttpResponseMessage response = await client.PutAsJsonAsync($"posts/{id}", request);
        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}:{responseContent}");
        }
    }

    public async Task DeletePostAsync(int id)
    {
        HttpResponseMessage response = await client.DeleteAsync($"posts/{id}");
        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}:{responseContent}");
        }
    }
}
