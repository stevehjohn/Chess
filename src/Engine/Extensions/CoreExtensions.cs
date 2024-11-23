namespace Engine.Extensions;

public static class CoreExtensions
{
    public static void OutputBoard(this Core core)
    {
        var foreground = Console.ForegroundColor;

        var background = Console.BackgroundColor;
        
        var state = core.ToString();
        
        Console.Write("  ");

        for (var i = 0; i < state.Length; i++)
        {
            if (state[i] == '|')
            {
                Console.WriteLine();
                
                Console.Write("  ");

                i++;
            }

            if (char.IsUpper(state[i]))
            {
                Console.ForegroundColor = background;

                Console.BackgroundColor = foreground;
            }
            else
            {
                Console.ForegroundColor = foreground;

                Console.BackgroundColor = background;
            }

            Console.Write(state[i]);
        }

        Console.ForegroundColor = foreground;

        Console.BackgroundColor = background;
        
        Console.WriteLine();
    }
}