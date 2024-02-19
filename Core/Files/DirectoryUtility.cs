using System.Reflection;

namespace Galaxon.Core.Files;

/// <summary>
/// Extension methods for the Directory class, and other useful methods for working with
/// directories.
/// </summary>
public static class DirectoryUtility
{
    /// <summary>
    /// Gets the path to the closest ancestor directory to the current (assembly) directory
    /// (which could be itself) containing a file with the specified extension.
    /// </summary>
    /// <returns>
    /// The path to the directory if found, or <c>null</c>.
    /// </returns>
    public static string ? GetParentDirectoryByFileType(string extension)
    {
        // Get the directory where the currently executing assembly (the application) is located.
        string assemblyLocation = Assembly.GetExecutingAssembly().Location;

        // Find the directory containing the desired file by traversing up the directory tree.
        string? currentDirectory = Path.GetDirectoryName(assemblyLocation);
        while (currentDirectory != null)
        {
            string[] solutionFiles = Directory.GetFiles(currentDirectory, $"*.{extension}");
            if (solutionFiles.Length > 0)
            {
                // Return the directory.
                return currentDirectory;
            }

            // Move up one level in the directory hierarchy.
            currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
        }

        // If no solution file is found, return null.
        return null;
    }

    /// <summary>
    /// Gets the path to the directory containing the solution file (.sln).
    /// </summary>
    /// <returns>
    /// The path to the solution directory if found, or <c>null</c> if no solution file was found.
    /// </returns>
    public static string? GetSolution()
    {
        return GetParentDirectoryByFileType("sln");
    }

    /// <summary>
    /// Gets the path to the directory containing the project file (.csproj).
    /// </summary>
    /// <returns>
    /// The path to the project directory if found, or <c>null</c> if no project file was found.
    /// </returns>
    public static string? GetProject()
    {
        return GetParentDirectoryByFileType("csproj");
    }
}
