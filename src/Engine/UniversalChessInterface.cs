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
            
            case "position":
                Position(parts[1..]);
                
                break;
            
            case "go":
                Go();

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

    private void Position(string[] parts)
    {
        if (parts[0] == "startpos")
        {
            _core.Initialise();
        }
        else
        {
            throw new EngineException("Only 'startpos' position is currently supported.");
        }

        if (parts.Length == 1)
        {
            return;
        }

        if (parts[1] != "moves")
        {
            throw new EngineException("Expected 'moves' or no further instructions.");
        }

        for (var i = 2; i < parts.Length; i++)
        {
            try
            {
                _core.MakeMove(parts[i]);
            }
            catch (Exception exception)
            {
                throw new EngineException($"An error occurred trying to nake move {parts[i]}. {exception}");
            }
        }
    }

    private void Go()
    {
    }

    private void Reply(string response)
    {
        _response.Add(response);
    }
}