namespace Engine.LiChessClient;
using static Engine.LiChessClient.Infrastructure.Console;

public static class EntryPoint
{
    public static async Task Main()
    {
        var results = new List<int>();
        
        for (var i = 0; i < 10; i++)
        {
            Clear();

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
            
            results.Add(await client.ChallengeLiChess("maia1"));
        }
    }
}