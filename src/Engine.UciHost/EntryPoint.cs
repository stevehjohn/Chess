namespace Engine.UciHost;

public static class EntryPoint
{
    public static void Main()
    {
        var uci = new UniversalChessInterface(ResponseCallback);

        var command = string.Empty;

        while (! command.Equals("quit", StringComparison.InvariantCultureIgnoreCase))
        {
            command = Console.ReadLine();

            if (command == null)
            {
                command = string.Empty;
                
                continue;
            }

            uci.IssueCommand(command);
        }
    }

    private static void ResponseCallback(string response)
    {
        Console.WriteLine(response);
    }
}