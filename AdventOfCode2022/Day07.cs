using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode2022.Day07;

public partial class Solution
{
    [Theory]
    [InlineData("Day07_Example.txt", 95437)]
    [InlineData("Day07.txt", 1077191)]
    public async Task Part1(string inputPath, int result)
    {
        var root = await ReadInput(inputPath);
        root.Directories().Where(node => node.Size() <= 100000).Sum(node => node.Size()).Should().Be(result);
    }

    [Theory]
    [InlineData("Day07_Example.txt", 24933642)]
    [InlineData("Day07.txt", 5649896)]
    public async Task Part2(string inputPath, int result)
    {
        var root = await ReadInput(inputPath);
        var used = root.Size();
        var toFind = 30000000 - (70000000 - used);
        var dirs = root.Directories().Where(node => node.Size() >= toFind).ToList();
        dirs.Sort((a, b) => a.Size().CompareTo(b.Size()));
        dirs.First().Size().Should().Be(result);
    }

    private static async Task<Node> ReadInput(string inputPath)
    {
        var lines = await File.ReadAllLinesAsync(inputPath);

        var root = new Node("/", null);
        var currentNode = root;

        foreach (var line in lines)
        {
            var match = CommandRegex().Match(line);
            if (match.Success)
            {
                currentNode = ProcessCommand(match, root, currentNode);
                continue;
            }

            match = FileRegex().Match(line);
            if (match.Success)
            {
                ProcessFile(match, currentNode);
            }
        }

        return root;
    }

    private static Node ProcessCommand(Match match, Node root, Node current)
    {
        switch (match.Groups[1].Value)
        {
            case "cd":
            {
                var target = match.Groups[2].Value;
                switch (target)
                {
                    case "/":
                        return root;

                    case "..":
                        return current.Parent;

                    default:
                    {
                        var node = current.Children.Find(n => n.Name == target);
                        if (node != null)
                            return node;

                        node = new Node(target, current);
                        current.Children.Add(node);
                        return node;
                    }
                }
            }

            default:
                return current;
        }
    }

    private static void ProcessFile(Match match, Node currentNode) =>
        currentNode.Children.Add(new Node(match.Groups[2].Value, currentNode, int.Parse(match.Groups[1].Value)));

    [GeneratedRegex(@"^\$ (cd) ?(.+)?$")]
    private static partial Regex CommandRegex();

    [GeneratedRegex(@"^(\d+) (.+)$")]
    private static partial Regex FileRegex();
}

public class Node
{
    public Node(string name, Node? parent)
    {
        Name = name;
        Parent = parent;
    }

    public Node(string name, Node parent, int size) : this(name, parent)
    {
        _size = size;
    }

    public bool IsDir => Children.Count > 0;

    public string Name { get; }

    public IEnumerable<Node> Directories()
    {
        var nodes = new List<Node>();
        if (IsDir && Name != "/")
            nodes.Add(this);

        foreach (var child in Children)
            nodes.AddRange(child.Directories());

        return nodes;
    }

    public int Size() => Children.Count == 0 ? _size : Children.Sum(node => node.Size());

    private int _size;

    public List<Node> Children { get; } = new();

    public Node? Parent { get; }

    public override string ToString() => Children.Count > 0 ? $"DIR: {Name} (Children: {Children.Count}, Size: {Size()})" : $"FILE: {Name} ({_size})";
}