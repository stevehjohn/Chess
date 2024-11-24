namespace Engine.LiChessClient;
using static Engine.LiChessClient.Infrastructure.Console;

public static class EntryPoint
{
    // Useful bots: KateFish, WorstFish, maia1
    private const string OpponentBot = "KateFish";
    
    private const int Games = 5;
    
    public static async Task Main()
    {
        var colour = ForegroundColor;
        
        var results = new List<int>();
        
        Clear();

        for (var i = 0; i < Games; i++)
        {
            if (results.Count > 0)
            {
                OutputLine("&NL;  &Cyan;Results so far:&White;");
                
                OutputLine();
                
                for (var game = 0; game < results.Count; game++)
                {
                    var result = results[game];
                    
                    Output($"    &Cyan;Game {game + 1}&White;: ");

                    switch (result)
                    {
                        case > 0:
                            Output("&Green;");
                            break;
                        case < 0:
                            Output("&Magenta;");
                            break;
                        default:
                            Output("&Gray;");
                            break;
                    }

                    OutputLine($"{results[game]}");
                }
            }

            var client = new Client.LiChessClient();

            try
            {
                var result = await client.ChallengeLiChess(OpponentBot);
                
                results.Add(result);

                OutputLine(result == 1 
                    ? "&NL;  &Green;OcpCore Engine&White; Wins!" 
                    : $"&NL;  &Magenta;{OpponentBot}&White; Wins.");
            }
            catch (Exception exception)
            {
                OutputLine($"    &Magenta;Error playing game &White;{i + 1}&Magenta; against&White; {OpponentBot}");
                
                OutputLine($"&NL;    &Gray;{exception.Message}");
            }
        }

        ForegroundColor = colour;
        
        OutputLine();
    }
}