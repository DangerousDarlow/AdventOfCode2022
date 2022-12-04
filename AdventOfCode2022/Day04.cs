using FluentAssertions;

namespace AdventOfCode2022.Day04;

public class Solution
{
    [Theory]
    [InlineData("Day04_Example.txt", 2)]
    [InlineData("Day04.txt", 534)]
    public async Task Part1(string inputPath, int result)
    {
        var pairRanges = await ReadInput(inputPath);
        pairRanges.Count(tuple => tuple.Item1.Contains(tuple.Item2) || tuple.Item2.Contains(tuple.Item1)).Should().Be(result);
    }

    [Theory]
    [InlineData("Day04_Example.txt", 4)]
    [InlineData("Day04.txt", 841)]
    public async Task Part2(string inputPath, int result)
    {
        var pairRanges = await ReadInput(inputPath);
        pairRanges.Count(tuple => tuple.Item1.Overlaps(tuple.Item2)).Should().Be(result);
    }

    private static async Task<IEnumerable<Tuple<IntegerRange, IntegerRange>>> ReadInput(string inputPath)
    {
        var lines = await File.ReadAllLinesAsync(inputPath);
        return lines.Select(s =>
        {
            var pair = s.Split(',');
            var ranges = pair.Select(elf =>
            {
                var bounds = elf.Split('-');
                return new IntegerRange(int.Parse(bounds[0]), int.Parse(bounds[1]));
            }).ToList();

            return new Tuple<IntegerRange, IntegerRange>(ranges[0], ranges[1]);
        });
    }
}

public class IntegerRange
{
    public IntegerRange(int start, int end)
    {
        Start = start;
        End = end;
    }

    public int Start { get; }
    public int End { get; }

    public bool Contains(IntegerRange other) => Start <= other.Start && End >= other.End;

    public bool Overlaps(IntegerRange other) => End >= other.Start && Start <= other.End;

    public override string ToString() => $"{Start}-{End}";
}