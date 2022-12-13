using FluentAssertions;

namespace AdventOfCode2022.Day06;

public class Solution
{
    [Theory]
    [InlineData("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 7)]
    [InlineData("bvwbjplbgvbhsrlpgdmjqwftvncz", 5)]
    [InlineData("nppdvjthqldpwncqszvftbrmjlhg", 6)]
    [InlineData("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 10)]
    [InlineData("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 11)]
    public async Task Part1Examples(string input, int result)
    {
        FindStartOfMarker(input, 4).Should().Be(result);
    }

    [Theory]
    [InlineData("Day06_Example.txt", 7)]
    [InlineData("Day06.txt", 1140)]
    public async Task Part1(string inputPath, int result)
    {
        FindStartOfMarker(await ReadInput(inputPath), 4).Should().Be(result);
    }

    [Theory]
    [InlineData("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 19)]
    [InlineData("bvwbjplbgvbhsrlpgdmjqwftvncz", 23)]
    [InlineData("nppdvjthqldpwncqszvftbrmjlhg", 23)]
    [InlineData("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 29)]
    [InlineData("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 26)]
    public async Task Part2Examples(string input, int result)
    {
        FindStartOfMarker(input, 14).Should().Be(result);
    }

    [Theory]
    [InlineData("Day06_Example.txt", 19)]
    [InlineData("Day06.txt", 3495)]
    public async Task Part2(string inputPath, int result)
    {
        FindStartOfMarker(await ReadInput(inputPath), 14).Should().Be(result);
    }

    private static int FindStartOfMarker(string input, int length)
    {
        for (var start = 0; start < input.Length - length; start++)
        {
            if (input.Substring(start, length).Distinct().Count() == length)
                return start + length;
        }

        throw new Exception("Marker not found");
    }

    private static async Task<string> ReadInput(string inputPath)
    {
        return (await File.ReadAllLinesAsync(inputPath)).First();
    }
}