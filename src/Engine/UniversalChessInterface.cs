using Engine.Infrastructure;

namespace Engine;

public class UniversalChessInterface
{
    private readonly Core _core = new();

    private readonly List<string> _response = [];
    
    public List<string> Respond(string command)
    {
        _response.Clear();

        ParseCommand(command);
        
        return _response;
    }

    private void ParseCommand(string command)
    {
        command = command.ToLower();
        
        var parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        switch (parts[0])
        {
            case "uci":
                Uci();
                
                break;
            
            case "ucinewgame":
                UciNewGame();

                break;
            
            case "isready":
                IsReady();

                break;
            
            default:
                throw new EngineException($"Unknown UCI command '{command}'.");
        }
    }

    private void Uci()
    {
        Reply($"id name {Core.EngineName}");
        
        Reply($"id author {Core.Author}");
        
        Reply("uciok");
    }

    private void UciNewGame()
    {
        _core.Initialise();
    }

    private void IsReady()
    {
        Reply("readyok");
    }

    private void Reply(string response)
    {
        _response.Add(response);
    }
}