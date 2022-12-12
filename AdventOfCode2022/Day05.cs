using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode2022.Day05;

public partial class Solution
{
    [Theory]
    [InlineData("Day05_Example.txt", "CMZ")]
    [InlineData("Day05.txt", "RTGWZTHLD")]
    public async Task Part1(string inputPath, string result)
    {
        var (stacks, moves) = await ReadInput(inputPath);
        
        foreach (var move in moves)
            stacks.Move(move);

        stacks.TopCrates().Should().Be(result);
    }

    [Theory]
    [InlineData("Day05_Example.txt", "MCD")]
    [InlineData("Day05.txt", "STHGRZZFR")]
    public async Task Part2(string inputPath, string result)
    {
        var (stacks, moves) = await ReadInput(inputPath);
        
        foreach (var move in moves)
            stacks.MoveBatch(move);

        stacks.TopCrates().Should().Be(result);
    }

    private static async Task<Tuple<Stacks, IEnumerable<Move>>> ReadInput(string inputPath)
    {
        var lines = await File.ReadAllLinesAsync(inputPath);
        var (drawing, procedure) = lines.SplitAtEmptyLine();
        var stacks = CreateStacksFromDrawing(drawing);
        var moves = CreateMovesFromProcedure(procedure);
        return new(stacks, moves);
    }

    private static IEnumerable<Move> CreateMovesFromProcedure(string[] procedure) => procedure.Select(s =>
    {
        var match = ProcedureRegex().Match(s);
        var (count, from, to) = (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value));
        return new Move(count, from, to);
    });

    private static Stacks CreateStacksFromDrawing(IReadOnlyList<string> drawing)
    {
        var stackIdLine = drawing[^1];

        var stackIdIndexes = stackIdLine
            .Select((c, i) => (c, i))
            .Where(x => char.IsDigit(x.c))
            .Select(x => x.i)
            .ToList();

        var stacks = new Stacks(stackIdIndexes.Count);

        for (var drawingIndex = drawing.Count - 2; drawingIndex >= 0; drawingIndex--)
        {
            foreach (var stackIdIndex in stackIdIndexes)
            {
                var drawingLine = drawing[drawingIndex];
                var crate = drawingLine[stackIdIndex];
                if (crate == ' ')
                    continue;

                var stackId = int.Parse(stackIdLine[stackIdIndex].ToString());
                stacks.AddToStack(stackId, crate);
            }
        }

        return stacks;
    }

    [GeneratedRegex("^move (\\d+) from (\\d+) to (\\d+)$")]
    private static partial Regex ProcedureRegex();
}

public class Stacks
{
    private readonly List<List<char>> _stacks;

    public Stacks(int count)
    {
        _stacks = new List<List<char>>();
        for (var i = 0; i < count; ++i) _stacks.Add(new List<char>());
    }

    public void AddToStack(int stackIndex, char value) => _stacks[stackIndex - 1].Add(value);

    public void Move(Move move)
    {
        var count = move.Count;
        while (count-- > 0) Move(move.From, move.To);
    }

    private void Move(int from, int to)
    {
        var fromStack = _stacks[from - 1];
        var toStack = _stacks[to - 1];

        var value = fromStack[^1];
        fromStack.RemoveAt(fromStack.Count - 1);
        toStack.Add(value);
    }
    
    public void MoveBatch(Move move)
    {
        var fromStack = _stacks[move.From - 1];
        var toStack = _stacks[move.To - 1];

        var values = fromStack.GetRange(fromStack.Count - move.Count, move.Count);
        fromStack.RemoveRange(fromStack.Count - move.Count, move.Count);
        toStack.AddRange(values);
    }

    public string TopCrates()
    {
        var builder = new StringBuilder();
        _stacks.ForEach(list => builder.Append(list.Last()));
        return builder.ToString();
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        for (var i = 0; i < _stacks.Count; ++i)
        {
            builder.Append($"{i + 1}: ");
            foreach (var value in _stacks[i]) builder.Append(value);
            builder.Append("  ");
        }

        return builder.ToString();
    }
}

public record Move(int Count, int From, int To);

public static class Extensions
{
    public static (string[] before, string[] after) SplitAtEmptyLine(this string[] lines)
    {
        var index = Array.IndexOf(lines, string.Empty);
        return (lines[..index], lines[(index + 1)..]);
    }
}