using FluentAssertions;

namespace AdventOfCode2022.Day01;

public class Solution
{
    [Theory]
    [InlineData("Day01_Example.txt", 24000)]
    [InlineData("Day01.txt", 74198)]
    public async Task Part1(string inputPath, int result)
    {
        var input = await ReadInput(inputPath);
        input
            .CaloriesPerElf()
            .Max()
            .Should().Be(result);
    }

    [Theory]
    [InlineData("Day01_Example.txt", 45000)]
    [InlineData("Day01.txt", 209914)]
    public async Task Part2(string inputPath, int result)
    {
        var input = await ReadInput(inputPath);
        input
            .CaloriesPerElf()
            .OrderDescending()
            .Take(3)
            .Aggregate((first, second) => first + second)
            .Should().Be(result);
    }

    private static async Task<IEnumerable<int?>> ReadInput(string inputPath)
    {
        var lines = await File.ReadAllLinesAsync(inputPath);
        return lines.Select(s => int.TryParse(s, out var result) ? (int?) result : null);
    }
}

public static class Extensions
{
    public static IEnumerable<int> CaloriesPerElf(this IEnumerable<int?> values)
    {
        var sumForElf = 0;
        foreach (var value in values)
        {
            if (value.HasValue)
                sumForElf += value.Value;
            else
            {
                yield return sumForElf;
                sumForElf = 0;
            }
        }

        yield return sumForElf;
    }
}