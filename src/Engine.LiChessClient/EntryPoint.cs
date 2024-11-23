namespace Engine.LiChessClient;
using static Engine.LiChessClient.Infrastructure.Console;

public static class EntryPoint
{
    private const string OpponentBot = "maia1";
    
    private const int Games = 1;
    
    public static async Task Main()
    {
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
                results.Add(await client.ChallengeLiChess(OpponentBot));
            }
            catch
            {
                    
                Output($"    &Magenta;Error playing game &White;{i + 1}&Magenta; against&White; {OpponentBot}");
            }
        }
    }
}