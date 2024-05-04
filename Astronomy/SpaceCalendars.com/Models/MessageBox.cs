using System.Text.Json;

namespace Galaxon.Astronomy.SpaceCalendars.com.Models;

public class MessageBox
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public MessageBox() { }

    public Dictionary<EMessageSeverity, List<string>> Messages { get; set; } = new ();

    public void Add(EMessageSeverity severity, string message, bool repeatsOk = false)
    {
        // Check there's a message.
        if (string.IsNullOrEmpty(message))
        {
            throw new ArgumentException("Empty message.", nameof(message));
        }

        // Check if a list of messages for this level already exists.
        bool containsKey = Messages.ContainsKey(severity);

        // Check for a repeat.
        if (containsKey && !repeatsOk && Messages[severity].Contains(message))
        {
            return;
        }

        // Check the collection exists for the specified level.
        if (!containsKey)
        {
            Messages[severity] = [];
        }

        // Add the message to the collection.
        Messages[severity].Add(message);
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
        Dictionary<EMessageSeverity, List<string>>? messages = JsonSerializer.Deserialize<Dictionary<EMessageSeverity, List<string>>>(json);
        if (messages == null)
        {
            throw new ArgumentException("Invalid JSON. Could not be deserialized to MessageBox.",
                nameof(json));
        }
        return new MessageBox { Messages = messages };
    }
}
