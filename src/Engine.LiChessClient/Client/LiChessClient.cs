using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Engine.LiChessClient.Client.Models;
using Engine.Pieces;
using static Engine.LiChessClient.Infrastructure.Console;

namespace Engine.LiChessClient.Client;

public class LiChessClient : IDisposable
{
    private const int WaitAttempts = 6;
    
    private readonly HttpClient _client;

    private readonly JsonSerializerOptions _serializerOptions;
    
    private readonly Core _core = new();

    private readonly bool _logCommunications;
    
    public LiChessClient(bool logCommunications = false)
    {
        _client = new HttpClient();

        var apiKey = "lip_F0EevLtSkhLimJbHnUlk"; //File.ReadAllLines("LiChess.key")[0];

        _client = new HttpClient
        {
            BaseAddress = new Uri("https://lichess.org")
        };

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

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

            var challengeState = await AwaitAcceptance(response.Id);

            if (challengeState.Accepted)
            {
                OutputLine("&NL;  &Cyan;Challenge &Green;ACCEPTED&White;.");

                await PlayGame(response.Id);
            }
            else
            {
                OutputLine($"&NL;  &Cyan;Challenge &Magenta;{challengeState.Reason}&White;.");
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

    private async Task<(bool Accepted, string Reason)> AwaitAcceptance(string id)
    {
        OutputLine($"&NL;  &Cyan;Game ID: &White;{id}");
        
        Thread.Sleep(1000);

        for (var attempt = 1; attempt <= WaitAttempts; attempt++)
        {
            var response = await Get<ChallengeResponse>($"challenge/{id}/show");

            if (response.Status == "accepted")
            {
                return (true, null);
            }

            if (response.Status is "declined" or "offline")
            {
                return (false, response.Status.ToUpperInvariant());
            }

            if (response.Status == "created")
            {
                Output($"  &Cyan;Attempt &White;{attempt}&Cyan; of &White;{WaitAttempts}&Cyan; Waiting ");

                var y = CursorLeft;
                
                for (var i = 10; i >= 0; i--)
                {
                    CursorLeft = y;
                    
                    switch (i)
                    {
                        case > 5:
                            Output("&Magenta;");
                            break;
                        case > 2:
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

        return (false, "TIMEOUT");
    }

    private async Task PlayGame(string id)
    {
        using var response = await _client.GetAsync($"api/bot/game/stream/{id}", HttpCompletionOption.ResponseHeadersRead);

        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync();

        using var reader = new StreamReader(stream);

        var first = true;

        var engineIsWhite = true;

        _core.Initialise();
                
        // Process.Start(new ProcessStartInfo
        // {
        //     FileName = $"httts://lichess.org/{id}",
        //     UseShellExecute = true
        // });

        var opponentName = string.Empty;

        GameState state = null;
        
        while (! reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();

            if (string.IsNullOrWhiteSpace(line))
            {
                OutputLine("  &White;...");

                if (state != null)
                {
                    await PlayMove(id, state, engineIsWhite);
                }

                continue;
            }

            OutputLine(line);
            
            if (first)
            {
                first = false;

                var game = JsonSerializer.Deserialize<StreamResponse>(line);

                OutputLine($"&NL;  &Cyan;White&White;: &Green;{game.White.Name}    &Cyan;Black&White;: {game.Black.Name}");
                
                engineIsWhite = game.White.Name == "StevoJ";

                opponentName = engineIsWhite ? game.Black.Name : game.White.Name;

                state = game.State;
            }
            else
            {
                //try
                {
                    state = JsonSerializer.Deserialize<GameState>(line);
                }
                // catch
                // {
                //     var chat = JsonSerializer.Deserialize<ChatLine>(line);
                //
                //     OutputLine($"&NL;  &Magenta;{opponentName}&White;: {chat.Text}");
                // }
            }

            if (state == null)
            {
                continue;
            }

            await PlayMove(id, state, engineIsWhite);
        }
    }

    private async Task PlayMove(string id, GameState state, bool engineIsWhite)
    {
        var moves = (state.Moves ?? string.Empty).Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var lastMove = string.Empty;

        if (moves.Length > 0)
        {
            lastMove = moves[^1];
        }

        if (_core.Player == Colour.White && engineIsWhite)
        {
            OutputLine("&NL;  &Cyan;Thinking&White;...");
            
            var engineMove = _core.GetMove(5);

            OutputLine($"&NL;  &Green;Engine&White;: {engineMove}");
            
            await Post<NullRequest, BasicResponse>($"bot/game/{id}/move/{engineMove}", null);
            
            _core.MakeMove(engineMove);
        }
        else
        {
            if (moves.Length <= _core.MoveCount)
            {
                return;
            }

            _core.MakeMove(lastMove);
            
            OutputLine($"&NL;  &Green;Opponent&White;: {lastMove}");
        }
    }

    private async Task<TResponse> Post<TRequest, TResponse>(string path, TRequest content) where TRequest : class
    {
        if (_logCommunications)
        {
            OutputLine($"&NL;&Gray;POST: api/{path}");
        }
        
        using var response = await _client.PostAsync($"api/{path}", content is NullRequest ? new StringContent(string.Empty) : JsonContent.Create(content));

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

        using var response = await _client.GetAsync($"api/{path}");

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

    public void Dispose()
    {
        _client?.Dispose();
    }
}