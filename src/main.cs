class Program
{
    static void Main()
    {
        while (true)
        {
            Console.Write("$ ");
            var command = Console.ReadLine();

            if (command == "exit") break;

            else Console.WriteLine($"{command}: command not found");
        }
    }
}
