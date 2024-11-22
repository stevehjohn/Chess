using Engine.Infrastructure;

namespace Engine;

public class UciInterface
{
    private Core _core;

    public string ReceiveCommand(string command)
    {
        switch (command.ToLowerInvariant())
        {
            case "uci":
                _core = new Core();
                
                return "ukiok";
            
            case "ucinewgame":
                break;
            
            case "isready":

                return "readyok";
        }

        if (command.StartsWith("position"))
        {
            SetEngineState(command);

            return null;
        }

        if (command.StartsWith("go"))
        {
            var move = _core.GetMove(6);

            return $"bestmove {move}";
        }

        throw new EngineException($"Unknown UCI command {command}.");
    }

    private void SetEngineState(string command)
    {
        var parts = command.Split(' ');

        var processingMoves = false;
        
        foreach (var part in parts)
        {
            if (processingMoves)
            {
                _core.MakeMove(part);
            }

            switch (part.ToLowerInvariant())
            {
                case "startpos":
                    _core.Initialise();
                    
                    continue;
                
                case "moves":
                    processingMoves = true;
                    
                    continue;
            }
        }
    }
}