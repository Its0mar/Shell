using System.Diagnostics;

namespace Shell.Commands;

public class ExternalCommandRunner
{
    /// <summary>
    /// Runs an external executable, using exec to preserve the command name.
    /// </summary>
    public static void Run(string commandName, string filePath, string[] arguments)
    {
        var escapedArgs = string.Join(' ', arguments.Select(arg => $"\"{arg}\""));

        var startInfo = new ProcessStartInfo
        {
            FileName = "/bin/sh",
            UseShellExecute = false,
        };

        // Build the 'exec' command
        // exec -a [Name the program sees] [Actual Path] [Arguments]
        startInfo.ArgumentList.Add("-c");
        startInfo.ArgumentList.Add($"exec -a \"{commandName}\" \"{filePath}\" {escapedArgs}");

        try
        {
            using Process? process = Process.Start(startInfo);
            process?.WaitForExit();
        }
        catch (Exception)
        {
            Console.WriteLine($"{commandName}: command not found");
        }
    }
}
