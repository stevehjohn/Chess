namespace Engine.LiChessClient;

public static class EntryPoint
{
    public static async Task Main()
    {
        var client = new Client.LiChessClient(true);
        
        await client.ChallengeLiChess("maia1");
    }
}