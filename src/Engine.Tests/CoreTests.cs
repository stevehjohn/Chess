using Engine.General;
using Xunit;
using Xunit.Abstractions;

namespace Engine.Tests;

public class CoreTests
{
    private static readonly List<long> ExpectedCombinations =
    [
        20,
        400,
        8_902,
        197_281,
        4_865_609,
        119_060_324,
        3_195_901_860,
        84_998_978_956,
        2_439_530_234_167,
        69_352_859_712_417
    ];

    private readonly ITestOutputHelper _outputHelper;
    
    private readonly Core _core = new();

    public CoreTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    [Theory]
    [InlineData(4)]
    public void MovesPerPly(int maxDepth)
    {
        for (var i = 1; i <= maxDepth; i++)
        {
            _core.Initialise();

            _core.GetMove(i);

            for (var j = 1; j <= i; j++)
            {                
                var count = _core.GetDepthCount(j);

                var expected = ExpectedCombinations[j - 1];
                
                var pass = count == expected;

                var output = $"  {(pass ? "PASS" : "FAIL")}  Depth: {j,2}  Combinations: {count,13:N0}  Expected: {expected,13:N0}";

                _outputHelper.WriteLine(output);

                // ReSharper disable once Xunit.XunitTestWithConsoleOutput
                Console.WriteLine(output);
            }
        }
    }

    [Theory]
    [InlineData("4BR2/3N4/2R2Q2/Q7/Q7/2K1k3/PP6/8 w", "a5e5|c6e6|f6e5|f6e6|f6e7|f6f2|f6f3")]
    [InlineData("rnbqkbnr/ppppppp1/8/8/1P6/2N5/P1PPPPpP/R1BQK2R b", "g2h1")]
    [InlineData("1rbqkbnQ/3pp3/2n2p2/ppp3pp/P3NP2/4P1PN/1PPPBK1P/R1B4R w", "e2h5|h8h5")]
    [InlineData("rnb1kbnr/pppp1ppp/4p3/PN4P1/1PP4P/B7/R2PP2q/3QKB1R b", "h2g3")]
    public void TakesCheckmateMoveIfAvailable(string fen, string expectedMoves)
    {
        _core.Initialise(fen);

        var move = _core.GetMove(1);

        var moves = expectedMoves.Split('|');
        
        Assert.Contains(move.Move, moves);
        
        Assert.Equal(MoveOutcome.OpponentInCheckmate, move.Outcome);
    }
    
    [Theory]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w", "g2g4")]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w", "e2e3|b7b5")]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w", "e2e4|b7b5|f1c4|f7f5")]
    public void DoesNotHallucinateCheckmate(string fen, string playedMoves)
    {
        _core.Initialise(fen);

        var moves = playedMoves.Split('|');

        foreach (var move in moves)
        {
            _core.MakeMove(move);
        }

        var nextMove = _core.GetMove(5);
        
        Assert.NotEqual(MoveOutcome.OpponentInCheckmate, nextMove.Outcome);
    }

    [Fact]
    public void PicksRandomMove()
    {
        var collisions = 0;
        
        for (var i = 0; i < 10; i++)
        {
            _core.Initialise();

            var firstMove = _core.GetMove(3);

            _core.Initialise();

            var secondMove = _core.GetMove(3);

            if (firstMove == secondMove)
            {
                collisions++;
            }
        }
        
        Assert.NotEqual(10, collisions);
    }
}