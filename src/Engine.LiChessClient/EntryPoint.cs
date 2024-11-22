namespace Engine.LiChessClient;

public static class EntryPoint
{
    public static async Task Main()
    {
        var client = new Client.LiChessClient();
        
        await client.ChallengeLiChess("maia1");
    }
}