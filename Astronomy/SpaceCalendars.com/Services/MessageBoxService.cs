using Galaxon.Astronomy.SpaceCalendars.com.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Galaxon.Astronomy.SpaceCalendars.com.Services;

public class MessageBoxService
{
    public static void AddMessage(ITempDataDictionary tempData, string level, string message,
        bool repeatsOk = false)
    {
        // Get the MessageBox from the TempDataDictionary, or create a new one if needed.
        MessageBox messageBox = tempData["MessageBox"] is string json
            ? MessageBox.Deserialize(json)
            : new MessageBox();

        // Add the message to the MessageBox.
        messageBox.Add(level, message, repeatsOk);

        // Store the updated MessageBox in the TempDataDictionary.
        tempData["MessageBox"] = messageBox.Serialize();
    }
}
