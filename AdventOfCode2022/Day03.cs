using FluentAssertions;

namespace AdventOfCode2022;

public class Day03
{
    [Theory]
    [InlineData("Day03_Example.txt", 157)]
    [InlineData("Day03.txt", 7917)]
    public async Task Part1(string inputPath, int result)
    {
        var backpacks = await ReadInput(inputPath);
        var backpacksWithItemsInBothCompartments = backpacks.Select(tuple => tuple.Item1.Intersect(tuple.Item2));
        backpacksWithItemsInBothCompartments.Select(backpack => backpack.Sum(Priority)).Sum().Should().Be(result);
    }

    [Theory]
    [InlineData("Day03_Example.txt", 70)]
    [InlineData("Day03.txt", 2585)]
    public async Task Part2(string inputPath, int result)
    {
        var backpacks = await File.ReadAllLinesAsync(inputPath);
        var groups = Group(backpacks, 3);
        var badges = groups.Select(group => group[0].Intersect(group[1]).Intersect(group[2]).First());
        badges.Sum(Priority).Should().Be(result);
    }

    private static int Priority(char item)
    {
        if (item >= 'a' && item <= 'z')
            return item - 'a' + 1;

        return item - 'A' + 27;
    }

    private static IEnumerable<IList<string>> Group(IEnumerable<string> items, int itemsPerGroup)
    {
        var group = new List<string>();
        foreach (var item in items)
        {
            group.Add(item);
            if (group.Count != itemsPerGroup)
                continue;

            yield return group;
            group = new List<string>();
        }
    }

    private static async Task<IEnumerable<Tuple<string, string>>> ReadInput(string inputPath)
    {
        var lines = await File.ReadAllLinesAsync(inputPath);
        return lines.Select(s =>
        {
            var first = s[..(s.Length / 2)];
            var second = s[(s.Length / 2)..];
            return new Tuple<string, string>(first, second);
        });
    }
}