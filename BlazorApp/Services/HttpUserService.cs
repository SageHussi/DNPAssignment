using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpUserService : IUserService
{
    private static readonly JsonSerializerOptions jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient client;

    public HttpUserService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<UserDto> AddUserAsync(CreateUserDto request)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("users", request);
        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}:{responseContent}");
        }

        UserDto returned = JsonSerializer.Deserialize<UserDto>(responseContent, jsonOptions) ?? throw new JsonException();
        return returned;
    }

    public async Task<UserDto> GetUserAsync(int id)
    {
        HttpResponseMessage response = await client.GetAsync($"users/{id}");
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}:{content}");
        }

        var user = JsonSerializer.Deserialize<UserDto>(content, jsonOptions);
        return user;
    }
    
    public async Task<ICollection<UserDto>> GetUsersAsync(string? usernameContains)
    {
        string url = "users";
        if (!string.IsNullOrWhiteSpace(usernameContains))
        {
            url += "?usernameContains=" + Uri.EscapeDataString(usernameContains);
        }

        HttpResponseMessage response = await client.GetAsync(url);
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}:{content}");
        }

        ICollection<UserDto> users = JsonSerializer.Deserialize<ICollection<UserDto>>(content, jsonOptions) ?? [];
        return users;
    }

    public async Task UpdateUserAsync(int id, UpdateUserDto request)
    {
        HttpResponseMessage response = await client.PutAsJsonAsync($"users/{id}", request);
        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}:{responseContent}");
        }
    }

    public async Task DeleteUserAsync(int id)
    {
        HttpResponseMessage response = await client.DeleteAsync($"users/{id}");
        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}:{responseContent}");
        }
    }
}