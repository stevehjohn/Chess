namespace Engine.Extensions;

public static class CoreExtensions
{
    public static void OutputBoard(this Core core)
    {
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

            Console.Write(state[i]);
        }
    }
}