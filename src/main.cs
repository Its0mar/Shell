using System.Diagnostics;

class Program
{
    static void Main()
    {
        var builtinCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "echo", "exit", "type"
        };

        while (true)
        {
            Console.Write("$ ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input)) continue;
            if (input == "exit" || input == "exit 0") break;

            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var cmd = parts[0];
            var argument = parts[1..];

            if (cmd == "echo")
            {
                Console.WriteLine(string.Join(' ', argument));
            }
            else if (cmd == "type")
            {
                if (builtinCommands.Contains(argument[0]))
                {
                    Console.WriteLine($"{argument[0]} is a shell builtin");
                }
                else
                {
                    string? pathEnv = Environment.GetEnvironmentVariable("PATH");
                    string[] directories = pathEnv?.Split(Path.PathSeparator) ?? [];
                    
                    bool found = false;
                    foreach (var dir in directories) 
                    {
                        var filePath = Path.Combine(dir, argument[0]);

                        if (File.Exists(filePath) && File.GetUnixFileMode(filePath).HasFlag(UnixFileMode.UserExecute))
                        {
                            Console.WriteLine($"{argument[0]} is {filePath}");
                            found = true;
                            break;
                        }   
                    }

                    if (!found)
                    {
                        Console.WriteLine($"{argument[0]}: not found");
                    }
                }
            }
            else 
            {
                    string? pathEnv = Environment.GetEnvironmentVariable("PATH");
                    string[] directories = pathEnv?.Split(Path.PathSeparator) ?? [];
                    
                    bool found = false;
                    string filePath = "";
                    foreach (var dir in directories) 
                    {
                        filePath = Path.Combine(dir, cmd);

                        if (File.Exists(filePath) && File.GetUnixFileMode(filePath).HasFlag(UnixFileMode.UserExecute))
                        {
                            found = true;
                            break;
                        }   
                    }

                    if (!found)
                    {
                        Console.WriteLine($"{cmd}: command not found");
                    }
                    else
                    {
                        var escapedArgs = string.Join(' ', argument.Select(arg => $"\"{arg}\""));

                        var startInfo = new ProcessStartInfo
                        {
                            FileName = "/bin/sh",
                            UseShellExecute = false,
                        };

                        // 2. Build the 'exec' command
                        // exec -a [Name the program sees] [Actual Path] [Arguments]
                        startInfo.ArgumentList.Add("-c");
                        startInfo.ArgumentList.Add($"exec -a \"{cmd}\" \"{filePath}\" {escapedArgs}");

                        try
                        {
                            using Process? process = Process.Start(startInfo);
                            process?.WaitForExit();
                        }
                        catch (Exception)
                        {
                            Console.WriteLine($"{cmd}: command not found");
                        }
                    }
            }
        }
    }
}