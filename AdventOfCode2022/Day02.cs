using FluentAssertions;

namespace AdventOfCode2022;

public class Day02
{
    [Theory]
    [InlineData("Day02_Example.txt", 15)]
    [InlineData("Day02.txt", 14375)]
    public async Task Part1(string inputPath, int result)
    {
        var input = await ReadInput(inputPath, ToRoundPart1);
        input.Select(round => round.Score).Sum().Should().Be(result);
    }

    [Theory]
    [InlineData("Day02_Example.txt", 12)]
    [InlineData("Day02.txt", 10274)]
    public async Task Part2(string inputPath, int result)
    {
        var input = await ReadInput(inputPath, ToRoundPart2);
        input.Select(round => round.Score).Sum().Should().Be(result);
    }

    private static async Task<IEnumerable<Round>> ReadInput(string inputPath, Func<string, Round> stringToRound)
    {
        var lines = await File.ReadAllLinesAsync(inputPath);
        return lines.Select(stringToRound);
    }

    private static Round ToRoundPart1(string input)
    {
        var plays = input.Split(' ');
        if (plays.Length != 2)
            throw new Exception($"Failed to split input '{input}'");

        return new Round(ToOpponentPlay(plays[0]), plays[1] switch
        {
            "X" => Play.Rock,
            "Y" => Play.Paper,
            "Z" => Play.Scissors,
            _ => throw new ArgumentOutOfRangeException($"Invalid play '{plays[1]}'")
        });
    }

    private static Round ToRoundPart2(string input)
    {
        var plays = input.Split(' ');
        if (plays.Length != 2)
            throw new Exception($"Failed to split input '{input}'");

        var opponent = ToOpponentPlay(plays[0]);

        return new Round(opponent, plays[1] switch
        {
            "X" => LoseTo(opponent),
            "Y" => DrawWith(opponent),
            "Z" => WinAgainst(opponent),
            _ => throw new ArgumentOutOfRangeException($"Invalid play '{plays[1]}'")
        });
    }

    private static Play LoseTo(Play opponent) =>
        opponent switch
        {
            Play.Rock => Play.Scissors,
            Play.Paper => Play.Rock,
            Play.Scissors => Play.Paper,
            _ => throw new ArgumentOutOfRangeException($"Invalid play '{opponent}'")
        };

    private static Play DrawWith(Play opponent) => opponent;

    private static Play WinAgainst(Play opponent) =>
        opponent switch
        {
            Play.Rock => Play.Paper,
            Play.Paper => Play.Scissors,
            Play.Scissors => Play.Rock,
            _ => throw new ArgumentOutOfRangeException($"Invalid play '{opponent}'")
        };

    private static Play ToOpponentPlay(string play) =>
        play switch
        {
            "A" => Play.Rock,
            "B" => Play.Paper,
            "C" => Play.Scissors,
            _ => throw new ArgumentOutOfRangeException($"Invalid play '{play}'")
        };
}

public enum Play
{
    Rock,
    Paper,
    Scissors
}

public class Round
{
    public Round(Play opponent, Play you)
    {
        Opponent = opponent;
        You = you;
    }

    private Play Opponent { get; }
    private Play You { get; }

    public int Score
    {
        get
        {
            var score = You switch
            {
                Play.Rock => 1,
                Play.Paper => 2,
                Play.Scissors => 3,
                _ => throw new ArgumentOutOfRangeException()
            };

            if (You == Opponent)
                return score + 3;

            if ((You == Play.Rock && Opponent == Play.Scissors) ||
                (You == Play.Paper && Opponent == Play.Rock) ||
                (You == Play.Scissors && Opponent == Play.Paper))
                return score + 6;

            return score;
        }
    }
}