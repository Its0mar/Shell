class Program
{
    static void Main()
    {
        while (true)
        {
            Console.Write("$ ");
            var command = Console.ReadLine();

            if (command == "exit") break;

            else if (command.StartsWith("echo "))
            {
                Console.WriteLine(command[5..]);
            }

            else if (command.StartsWith("type "))
            {
                var commandName = command[5..];
                var builtinCommands = new[] { "echo", "exit", "type" };
                if (builtinCommands.Contains(commandName))
                {
                    Console.WriteLine($"{commandName} is a shell builtin");
                }
                else
                {
                    Console.WriteLine($"{commandName}: not found");
                }
            }

            else Console.WriteLine($"{command}: command not found");
        }
    }
}
