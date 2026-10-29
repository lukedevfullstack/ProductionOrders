using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProductionOrdersSequor.Desktop.Services
{
    public class FlexibleIntConverter : JsonConverter<int?>
    {
        public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetInt32(out int intVal))
                {
                    return intVal;
                }
                if (reader.TryGetDecimal(out decimal decVal))
                {
                    return (int)decVal;
                }
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                string? stringValue = reader.GetString();
                if (int.TryParse(stringValue, out int result))
                {
                    return result;
                }
                if (decimal.TryParse(stringValue, out decimal decResult))
                {
                    return (int)decResult;
                }
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteNumberValue(value.Value);
            else
                writer.WriteNullValue();
        }
    }
}