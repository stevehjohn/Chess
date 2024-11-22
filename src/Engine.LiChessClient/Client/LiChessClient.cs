using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Engine.LiChessClient.Client.Models;
using static Engine.LiChessClient.Infrastructure.Console;

namespace Engine.LiChessClient.Client;

public class LiChessClient
{
    private const int WaitAttempts = 20;
    
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
            PropertyNameCaseInsensitive = false,
            WriteIndented = true
        };
    }

    public async Task ChallengeLiChess(string username)
    {
        var colour = ForegroundColor;
        
        Clear();
        
        OutputLine($"&NL;  &Cyan;Challenging &White;{username}");

        var response = await Post<ChallengeRequest, ChallengeResponse>($"challenge/{username}", new ChallengeRequest
        {
            Clock = new Clock
            {
                Increment = 10,
                Linit = 900
            },
            KeepAliveStream = false,
            Variant = "standard"
        });

        if (response.Status == "created")
        {
            OutputLine("&NL;  &Cyan;Challenge &Yellow;CREATED&White;.");

            var accepted = await AwaitAcceptance(response.Id);

            if (accepted)
            {
                OutputLine("&NL;  &Cyan;Challenge &Green;ACCEPTED&White;.");

                await PlayGame(response.Id);
            }
            else
            {
                OutputLine("&NL;  &Cyan;Challenge &Magenta;DECLINED&White;.");
            }
        }
        else if (response.Status == "accepted")
        {
            OutputLine("&NL;  &Cyan;Challenge &Green;ACCEPTED&White;.");

            await PlayGame(response.Id);
        }
        else
        {
            OutputLine("&NL;  &Cyan;Challenge &Magenta;DECLINED&White;.");
        }
        
        OutputLine();

        ForegroundColor = colour;
    }

    private async Task<bool> AwaitAcceptance(string id)
    {
        OutputLine($"&NL;  &Cyan;Game ID: &White;{id}");

        for (var attempt = 1; attempt <= 20; attempt++)
        {
            var response = await Get<ChallengeResponse>($"challenge/{id}/show");

            if (response.Status == "accepted")
            {
                return true;
            }

            if (response.Status is "declined" or "offline")
            {
                return false;
            }

            if (response.Status == "created")
            {
                Output($"  &Cyan;Attempt &White;{attempt}&Cyan; of &White;{WaitAttempts}&Cyan; Waiting ");

                var y = CursorLeft;
                
                for (var i = 30; i >= 0; i--)
                {
                    CursorLeft = y;
                    
                    switch (i)
                    {
                        case > 20:
                            Output("&Magenta;");
                            break;
                        case > 10:
                            Output("&Yellow;");
                            break;
                        default:
                            Output("&Green;");
                            break;
                    }

                    Output($"{i}  ");

                    Thread.Sleep(1000);
                }
            }
        }

        return false;
    }

    private async Task PlayGame(string id)
    {
    }

    private async Task<TResponse> Post<TRequest, TResponse>(string path, TRequest content)
    {
        if (_logCommunications)
        {
            OutputLine($"&NL;&Gray;POST: api/{path}");
        }

        var response = await _client.PostAsync($"api/{path}", JsonContent.Create(content));

        if (_logCommunications)
        {
            OutputLine($"&NL;{response.StatusCode}");
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
            OutputLine();
            
            OutputLine($"&Gray;GET: api/{path}");
        }

        var response = await _client.GetAsync($"api/{path}");

        if (_logCommunications)
        {
            OutputLine($"&NL;{response.StatusCode}");
        }
        
        var responseString = await response.Content.ReadAsStringAsync();

        var responseObject = JsonSerializer.Deserialize<TResponse>(responseString);
        
        if (_logCommunications)
        {
            OutputLine($"&Gray;{JsonSerializer.Serialize(JsonSerializer.Deserialize<JsonElement>(responseString), _serializerOptions)}");
            
            OutputLine();
        }

        return responseObject;
    }
}