using Shell.Commands;

namespace Shell.Core;

public class ShellRunner
{
    private readonly PathResolver _pathResolver;

    public ShellRunner()
    {
        _pathResolver = new PathResolver();
    }

    public void Run()
    {
        while (true)
        {
            Console.Write("$ ");
            var input = Console.ReadLine();

            var parsed = CommandParser.Parse(input);
            if (parsed == null) continue;

            var (cmd, arguments) = parsed;

            if (cmd == "exit" && (arguments.Length == 0 || arguments[0] == "0"))
                break;

            if (cmd == "echo")
            {
                BuiltinCommands.HandleEcho(arguments);
            }
            else if (cmd == "type")
            {
                BuiltinCommands.HandleType(arguments, _pathResolver);
            }
            else if (cmd == "pwd")
            {
                BuiltinCommands.HandlePwd();
            }
            else if (cmd == "cd")
            {
                BuiltinCommands.HandleCd(arguments);
            }
            else
            {
                var executablePath = _pathResolver.FindExecutable(cmd);

                if (executablePath == null)
                {
                    Console.WriteLine($"{cmd}: command not found");
                }
                else
                {
                    ExternalCommandRunner.Run(cmd, executablePath, arguments);
                }
            }
        }
    }
}
