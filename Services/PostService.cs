using System;
using System.Text.Json;
using Backend.DTOs;

namespace Backend.Services;

public class PostService : IPostService
{
    private HttpClient _httpClient;

    public PostService(HttpClient HttpClient)
    {
        _httpClient = HttpClient;
    }

    public async Task<IEnumerable<PostDto>> Get()
    {
        var result = await _httpClient.GetAsync(_httpClient.BaseAddress);
        var body = await result.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var post = JsonSerializer.Deserialize<IEnumerable<PostDto>>(body, options);
        return post;
    }
}
