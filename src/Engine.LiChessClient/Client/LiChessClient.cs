using System.Net.Http.Headers;
using System.Text.Json;
using Engine.LiChessClient.Client.Models;
using static Engine.LiChessClient.Infrastructure.Console;

namespace Engine.LiChessClient.Client;

public class LiChessClient
{
    private readonly HttpClient _client;

    private readonly JsonSerializerOptions _serializerOptions;
    
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

        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };
    }

    public async Task ChallengeLiChess(string username)
    {
        var colour = ForegroundColor;
        
        Clear();
        
        OutputLine($"\n  &Cyan;Challenging &White;{username}");

        var response = await Post<ChallengeResponse>($"challenge/{username}");

        if (response.Status == "created")
        {
            OutputLine("\n  &Cyan; Challenge &Green;ACCEPTED&White;.");
        }
        else
        {
            OutputLine("\n  &Cyan; Challenge &Magenta;DECLINED&White;.");
        
            OutputLine();

            ForegroundColor = colour;
            
            return;
        }

        await PlayGame(response.Id);
        
        OutputLine();

        ForegroundColor = colour;
    }

    private async Task PlayGame(string id)
    {
        OutputLine($"  &Cyan;Game ID: &white{id}");

        var response = await Get<ChallengeResponse>($"board/game/{id}");
    }

    private async Task<TResponse> Post<TResponse>(string path, string content = null)
    {
        if (_logCommunications)
        {
            OutputLine($"&Gray;POST: {path}");
        }

        var response = await _client.PostAsync($"api/{path}", new StringContent(content ?? string.Empty));

        if (_logCommunications)
        {
            OutputLine($"{response.StatusCode}");
        }

        var responseString = await response.Content.ReadAsStringAsync();

        var responseObject = JsonSerializer.Deserialize<TResponse>(responseString);
        
        if (_logCommunications)
        {
            OutputLine($"&Gray;{JsonSerializer.Serialize(JsonSerializer.Deserialize<JsonElement>(responseString), _serializerOptions)}");
        }

        return responseObject;
    }

    private async Task<TResponse> Get<TResponse>(string path)
    {
        if (_logCommunications)
        {
            OutputLine($"&Gray;POST: {path}");
        }

        var response = await _client.GetAsync($"api/{path}");

        if (_logCommunications)
        {
            OutputLine($"{response.StatusCode}");
        }

        var responseString = await response.Content.ReadAsStringAsync();

        var responseObject = JsonSerializer.Deserialize<TResponse>(responseString);
        
        if (_logCommunications)
        {
            OutputLine($"&Gray;{JsonSerializer.Serialize(JsonSerializer.Deserialize<JsonElement>(responseString), _serializerOptions)}");
        }

        return responseObject;
    }
}