using System.Text.Json;
using System.Text.Json.Serialization;

namespace Galaxon.Quantities;

public class QuantityJsonConverter : JsonConverter<Quantity>
{
    /// <inheritdoc/>
    public override Quantity? Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Expected start of array token.");
        }

        double amount = 0;
        string? units = "";

        // Read the first element (amount).
        if (!reader.Read() || reader.TokenType != JsonTokenType.Number)
        {
            throw new JsonException("Expected number token for amount.");
        }
        amount = reader.GetDouble();

        // Read the second element (units).
        if (!reader.Read() || reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Expected string token for units.");
        }
        units = reader.GetString();

        // Consume the end of array token
        if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("Expected end of array token.");
        }

        return new Quantity(amount, units);
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, Quantity qty, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        writer.WriteNumberValue(qty.Amount);
        writer.WriteStringValue(qty.Units.ToString());
        writer.WriteEndArray();
    }
}
