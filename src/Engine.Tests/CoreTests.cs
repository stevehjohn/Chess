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
    [InlineData("K7/8/8/8/8/8/5Q2/7k b")]
    [InlineData("k7/7R/8/7p/5p1P/b4N2/8/RQ5K b")]
    public void CoreDetectsStalemate(string fen)
    {
        _core.Initialise(fen);

        var move = _core.GetMove(1);
        
        Assert.Equal(MoveOutcome.Stalemate, move.Outcome);
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

    [Theory]
    [InlineData("rn1qkbnr/ppp2ppp/8/3pp3/8/7b/PPPPPPPP/RNBQKB1R w", "g2h3")]
    [InlineData("rn1qkbnr/ppp1pp1p/7p/3p4/8/3P3b/PPP1PPPP/RN1QKB1R w", "g2h3")]
    public void ShouldPickDecentMove(string fen, string expectedMove)
    {
        _core.Initialise(fen);

        var nextMove = _core.GetMove(5);
        
        Assert.Equal(expectedMove, nextMove.Move);
    }

    [Theory]
    [InlineData("rnb3nr/pp2kp1p/3bp1p1/1Bp5/5P1N/8/PPPP2PP/RNBQK2R w", "e1g1", "rnb   nr|pp  kp p|   bp p | Bp     |     P N|        |PPPP  PP|RNBQ RK ")]
    public void SpecialMovesAreRecognisedFromOpponent(string fen, string move, string expected)
    {
        _core.Initialise(fen);
        
        _core.MakeMove(move);
        
        Assert.Equal(expected, _core.ToString());
    }

    [Theory]
    [InlineData("5Q1k/r1p1Np1p/np6/p3N1p1/4P3/3P4/PPP2PPP/R4RK1 b")]
    public void RecognisesCheckmateFromOpponent(string fen)
    {
        _core.Initialise(fen);

        var move = _core.GetMove(2);
        
        Assert.Equal(MoveOutcome.EngineInCheckmate, move.Outcome);
    }

    [Theory]
    [InlineData("r1bqkbnr/p1pp1ppp/p3p3/8/3PP3/5N2/PPP2PPP/RNBQK2R b", "d8g5|d8h4")]
    public void ScoresAreEvaluatedToThePlayersBestInterests(string fen, string notExpected)
    {
        _core.Initialise(fen);

        var move = _core.GetMove(5);

        Assert.DoesNotContain(move.Move, notExpected.Split('|'));
    }

    [Theory]
    [InlineData("p7/1P6/8/8/8/8/8/8 b", 1, "10")]
    [InlineData("8/p7/1P6/8/8/8/8/8 w", 1, "10")]
    [InlineData("p7/1P6/8/8/8/8/8/8 w", 1, "90")]
    [InlineData("p7/8/1P6/8/8/8/8/8 w", 1, "0,10")]
    public void CalculatesExpectedPlyScores(string fen, int depth, string expectedScores)
    {
        _core.Initialise(fen);

        _core.GetMove(depth);

        var scores = expectedScores.Split(",").Select(int.Parse).ToList();

        for (var i = 0; i < depth; i++)
        {
            Assert.Equal(scores[i], _core.GetBestScore(i + 1));
        }        
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