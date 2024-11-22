using System.Net.Http.Headers;
using static Engine.LiChessClient.Infrastructure.Console;

namespace Engine.LiChessClient.Client;

public class LiChessClient
{
    private HttpClient _client;

    private UciInterface _interface;
    
    public LiChessClient()
    {
        _client = new HttpClient();

        var apiKey = File.ReadAllLines("LiChess.key")[0];

        _client = new HttpClient
        {
            BaseAddress = new Uri("https://lichess.org")
        };

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        _interface = new UciInterface();
    }

    public async Task ChallengeLiChess(string username)
    {
        Clear();
        
        OutputLine();
        
        OutputLine($"  &Cyan;Challenging &White;{username}");

        await Post($"challenge/{username}");
    }

    private async Task<string> Post(string path, string content = null)
    {
        OutputLine();
        
        OutputLine($"  &Gray;POST: {path}");
        
        var response = await _client.PostAsync($"api/{path}", new StringContent(content ?? string.Empty));

        var responseString = await response.Content.ReadAsStringAsync();
        
        OutputLine($"  &Gray;{responseString}");
        
        OutputLine();

        return responseString;
    }
}