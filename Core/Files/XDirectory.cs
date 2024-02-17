using System.Reflection;

namespace Galaxon.Core.Files;

/// <summary>
/// Extension methods for the Directory class, and other useful methods for working with
/// directories.
/// </summary>
public static class XDirectory
{
    /// <summary>
    /// Gets the path to the solution directory containing the solution file (.sln).
    /// </summary>
    /// <returns>
    /// The path to the solution directory if found, or <c>null</c> if no solution file was found.
    /// </returns>
    public static string? GetSolutionDirectory()
    {
        // Get the directory where the currently executing assembly (your application) is located.
        string assemblyLocation = Assembly.GetExecutingAssembly().Location;

        // Find the directory containing the solution file by traversing up the directory tree.
        string? currentDirectory = Path.GetDirectoryName(assemblyLocation);
        while (currentDirectory != null)
        {
            string[] solutionFiles = Directory.GetFiles(currentDirectory, "*.sln");
            if (solutionFiles.Length > 0)
            {
                // Solution file(s) found in this directory, so it's likely the solution root.
                return currentDirectory;
            }

            // Move up one level in the directory hierarchy.
            currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
        }

        // If no solution file is found, return null or an empty string to indicate failure.
        return null;
    }
}
