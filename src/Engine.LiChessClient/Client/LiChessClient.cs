using System.Net.Http.Headers;
using System.Text.Json;
using static Engine.LiChessClient.Infrastructure.Console;

namespace Engine.LiChessClient.Client;

public class LiChessClient
{
    private readonly HttpClient _client;

    private readonly UciInterface _interface;

    private readonly bool _logCommunications;
    
    public LiChessClient(bool logCommunications = false)
    {
        _client = new HttpClient();

        var apiKey = File.ReadAllLines("LiChess.key")[0];

        _client = new HttpClient
        {
            BaseAddress = new Uri("https://lichess.org")
        };

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        _interface = new UciInterface();

        _logCommunications = logCommunications;
    }

    public async Task ChallengeLiChess(string username)
    {
        Clear();
        
        OutputLine();
        
        OutputLine($"  &Cyan;Challenging &White;{username}");

        var response = await Post($"challenge/{username}");
    }

    private async Task<string> Post(string path, string content = null)
    {
        if (_logCommunications)
        {
            OutputLine();

            OutputLine($"&Gray;POST: {path}");
        }

        var response = await _client.PostAsync($"api/{path}", new StringContent(content ?? string.Empty));

        OutputLine();
        
        OutputLine($"{response.StatusCode}");
        
        var responseString = await response.Content.ReadAsStringAsync();

        if (_logCommunications)
        {
            OutputLine($"&Gray;{Prettify(responseString)}");

            OutputLine();
        }

        return responseString;
    }

    private static string Prettify(string json)
    {
        var jsonObject = JsonSerializer.Deserialize<JsonElement>(json);

        return JsonSerializer.Serialize(jsonObject, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }
}