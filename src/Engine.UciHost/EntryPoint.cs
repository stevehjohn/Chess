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
            
            File.AppendAllLines("ocp-core-engine-log.txt", [ command ]);

            try
            {
                uci.IssueCommand(command);
            }
            catch (Exception exception)
            {
                File.AppendAllLines("ocp-core-engine-errors.txt", [ exception.Message ]);
            }
        }
    }

    private static void ResponseCallback(string response)
    {
        File.AppendAllLines("ocp-core-engine-log.txt", [ response ]);

        Console.WriteLine(response);
    }
}