using System.Reflection;
using Galaxon.Core.Files;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Galaxon.Development.Application;

public class SetupTools
{
    /// <summary>
    /// Cached value of path to working directory.
    /// </summary>
    private static string? _workingDirectory;

    /// <summary>
    /// Get the application environment (Development, Production, Testing, Staging, etc.)
    /// </summary>
    /// <returns>The environment as a string.</returns>
    public static string GetEnvironment()
    {
        return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            ?? Environment.GetEnvironmentVariable("CONSOLEAPP_ENVIRONMENT")
            ?? "Production";
    }

    /// <summary>
    /// Get the directory where to find appsettings files and where to put the Logs directory.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static string GetWorkingDirectory()
    {
        if (_workingDirectory != null)
        {
            return _workingDirectory;
        }

        string env = GetEnvironment();
        string dir;

        if (env == "Development")
        {
            string? projDir = DirectoryUtility.GetProjectDirectory();
            if (string.IsNullOrEmpty(projDir))
            {
                throw new InvalidOperationException("Could not find project directory.");
            }
            dir = projDir;
        }
        else
        {
            dir = Directory.GetCurrentDirectory();
        }

        _workingDirectory = dir;

        return dir;
    }

    /// <summary>
    /// Load configuration from the appsettings.json files.
    /// </summary>
    public static IConfiguration LoadConfiguration()
    {
        string env = GetEnvironment();
        string dir = GetWorkingDirectory();

        // Load the config.
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(dir)
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{env}.json", true, true)
            .Build();

        return configuration;
    }

    /// <summary>
    /// Configures the Serilog logging service.
    /// </summary>
    public static void SetupLogging(string workingDirectory, IConfiguration configuration)
    {
        // Get the path to the Logs directory and ensure it exists.
        string logsDir = Path.Combine(workingDirectory, "Logs");
        Directory.CreateDirectory(logsDir);

        // Get the full path to the log file.
        string assemblyName = Assembly.GetExecutingAssembly().GetName().Name ?? "Unknown";
        string logFilePath = Path.Combine(logsDir, $"{assemblyName}.log");

        // Initialize Serilog using the configuration, but override the file path.
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }

    public static IConfiguration Initialize()
    {
        string dir = GetWorkingDirectory();
        IConfiguration configuration = LoadConfiguration();
        SetupLogging(dir, configuration);
        return configuration;
    }
}
