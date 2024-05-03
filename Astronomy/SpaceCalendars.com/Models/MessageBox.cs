using System.Text.Json;

namespace Galaxon.Astronomy.SpaceCalendars.com.Models;

public class MessageBox
{
    public Dictionary<string, List<string>> Messages { get; set; } = new();

    /// <summary>
    /// These classes correspond to bootstrap alerts.
    /// <see href="https://getbootstrap.com/docs/5.2/components/alerts/"/>
    /// </summary>
    public static readonly string[] ValidLevels = { "danger", "warning", "success", "info" };

    /// <summary>
    /// Constructor.
    /// </summary>
    public MessageBox()
    {
    }

    public void Add(string level, string message, bool repeatsOk = false)
    {
        // Guards.
        if (!ValidLevels.Contains(level))
        {
            throw new ArgumentException(
                "Invalid level. Valid levels are \"danger\", \"warning\", \"success\", and \"info\".",
                nameof(level)
            );
        }
        if (string.IsNullOrEmpty(message))
        {
            throw new ArgumentException("Empty message.", nameof(message));
        }

        // Check if a list of messages for this level already exists.
        bool containsKey = Messages.ContainsKey(level);

        // Check for a repeat.
        if (containsKey && !repeatsOk && Messages[level].Contains(message))
        {
            return;
        }

        // Check the collection exists for the specified level.
        if (!containsKey)
        {
            Messages[level] = new List<string>();
        }

        // Add the message to the collection.
        Messages[level].Add(message);
    }

    /// <summary>
    /// Serialize as JSON.
    /// </summary>
    public string Serialize()
    {
        return JsonSerializer.Serialize(Messages);
    }

    /// <summary>
    /// Deserialize from JSON.
    /// </summary>
    public static MessageBox Deserialize(string json)
    {
        Dictionary<string, List<string>>? messages =
            JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);
        if (messages == null)
        {
            throw new ArgumentException("Invalid JSON. Could not be deserialized to MessageBox.",
                nameof(json));
        }
        return new MessageBox { Messages = messages };
    }
}
