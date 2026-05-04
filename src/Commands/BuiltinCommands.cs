using System.Net.NetworkInformation;
using Shell.Core;

namespace Shell.Commands;

public class BuiltinCommands
{
    private static readonly HashSet<string> Names = new(StringComparer.OrdinalIgnoreCase)
    {
        "echo", "exit", "type", "pwd", "cd"
    };

    public static bool IsBuiltin(string command)
    {
        return Names.Contains(command);
    }

    public static void HandleEcho(string[] arguments)
    {
        Console.WriteLine(string.Join(' ', arguments));
    }

    public static void HandleType(string[] arguments, PathResolver pathResolver)
    {
        var target = arguments[0];

        if (IsBuiltin(target))
        {
            Console.WriteLine($"{target} is a shell builtin");
        }
        else
        {
            var executablePath = pathResolver.FindExecutable(target);

            if (executablePath != null)
            {
                Console.WriteLine($"{target} is {executablePath}");
            }
            else
            {
                Console.WriteLine($"{target}: not found");
            }
        }
    }

    public static void HandlePwd()
    {
        Console.WriteLine(Directory.GetCurrentDirectory());
    }

    public static void HandleCd(string[] arguments)
    {
        var target = arguments[0];
        if (target == "~") {
            HandleCdHome();
            return;
        }
        if (!Directory.Exists(target)) {
            Console.WriteLine($"{target}: No such file or directory");
            return;
        }

        Directory.SetCurrentDirectory(target);
    }

    private static void HandleCdHome() {
        Directory.SetCurrentDirectory(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
    }
}
