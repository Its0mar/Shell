namespace Shell.Core;

public class CommandParser
{
    public record ParsedCommand(string Name, string[] Arguments);

    public static ParsedCommand? Parse(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var name = parts[0];
        var arguments = parts[1..];

        return new ParsedCommand(name, arguments);
    }
}
