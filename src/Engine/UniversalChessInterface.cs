using Engine.Infrastructure;

namespace Engine;

public class UniversalChessInterface
{
    private const int DefaultDepth = 6;
    
    private readonly Core _core = new();

    private readonly Action<string> _responseCallback;
    
    public UniversalChessInterface(Action<string> responseCallback)
    {
        _responseCallback = responseCallback;
    }
    
    public void IssueCommand(string command)
    {
        ParseCommand(command);
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
            
            case "stop":
                Stop();
                
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
        _core.GetMove(DefaultDepth, move => _responseCallback($"bestmove {move}"));
    }

    private void Stop()
    {
        Reply($"bestmove {_core.Interrupt()}");
    }

    private void Reply(string response)
    {
        _responseCallback(response);
    }
}