using System.Text;

namespace src.Core;

public class CommandParser
{
    public record ParsedCommand(string Name, string[] Arguments);

    public static ParsedCommand? Parse(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;

        List<string> args = new();
        StringBuilder currentArg = new();
        bool inSingleQuote = false;

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];

            if (c == '\'')
            {
                inSingleQuote = !inSingleQuote;
            }
            else if (c == ' ' && !inSingleQuote)
            {
                if (currentArg.Length > 0)
                {
                    args.Add(currentArg.ToString());
                    currentArg.Clear();
                }
            }
            else
            {
                currentArg.Append(c);
            }
        }

        if (currentArg.Length > 0)
            args.Add(currentArg.ToString());

        if (args.Count == 0) return null;

        return new ParsedCommand(args[0], [.. args.Skip(1)]);
    }

}