namespace Shell.Core;

public class PathResolver
{
    private readonly string[] _directories;

    public PathResolver()
    {
        string? pathEnv = Environment.GetEnvironmentVariable("PATH");
        _directories = pathEnv?.Split(Path.PathSeparator) ?? [];
    }

    /// <summary>
    /// Searches the PATH directories for an executable with the given name.
    /// Returns the full path if found, or null otherwise.
    /// </summary>
    public string? FindExecutable(string name)
    {
        foreach (var dir in _directories)
        {
            var filePath = Path.Combine(dir, name);

            if (File.Exists(filePath) && File.GetUnixFileMode(filePath).HasFlag(UnixFileMode.UserExecute))
            {
                return filePath;
            }
        }

        return null;
    }
}
