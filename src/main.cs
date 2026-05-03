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
            if (input == "exit 0") break;

            var parts = input.Split(' ', 2);
            var cmd = parts[0];
            var argument = parts.Length > 1 ? parts[1] : "";

            if (cmd == "echo")
            {
                Console.WriteLine(argument);
            }
            else if (cmd == "type")
            {
                if (builtinCommands.Contains(argument))
                {
                    Console.WriteLine($"{argument} is a shell builtin");
                }
                else
                {
                    string? pathEnv = Environment.GetEnvironmentVariable("PATH");
                    string[] directories = pathEnv?.Split(Path.PathSeparator) ?? [];
                    
                    bool found = false;
                    foreach (var dir in directories) 
                    {
                        var filePath = Path.Combine(dir, argument);

                        if (File.Exists(filePath))
                        {
                            Console.WriteLine($"{argument} is {filePath}");
                            found = true;
                            break;
                        }   
                    }

                    if (!found)
                    {
                        Console.WriteLine($"{argument}: not found");
                    }
                }
            }
            else 
            {
                Console.WriteLine($"{input}: command not found");
            }
        }
    }
}