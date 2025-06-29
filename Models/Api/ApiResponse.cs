using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;
using System.Text.RegularExpressions;

namespace crickinfo_mvc_ef_core.Models.Api
{
    public class ApiResponse
    {
        public List<PointTableEntry> Points_Table { get; set; }
        public List<ScheduleEntry> Schedule { get; set; }
    }

    public class PointTableEntry
    {
        public int Id { get; set; }

        [JsonPropertyName("Team")]
        public string TeamName { get; set; }

        [JsonConverter(typeof(StringToIntConverter))]
        public int Matches { get; set; }

        [JsonConverter(typeof(StringToIntConverter))]
        public int Won { get; set; }

        [JsonConverter(typeof(StringToIntConverter))]
        public int Lost { get; set; }

        [JsonConverter(typeof(StringToIntConverter))]
        public int Tied { get; set; }

        [JsonConverter(typeof(StringToIntConverter))]
        public int NR { get; set; }

        [JsonConverter(typeof(StringToIntConverter))]
        public int Points { get; set; }

        public string NRR { get; set; }
    }

    public class ScheduleEntry
    {
        [JsonPropertyName("Match No")]
        [JsonConverter(typeof(MatchNumberConverter))]
        public int MatchNo { get; set; }

        [JsonPropertyName("Team 1")]
        public string Team1 { get; set; }

        [JsonPropertyName("Team 2")]
        public string Team2 { get; set; }

        [JsonConverter(typeof(DateStringConverter))]
        public DateTime Date { get; set; }

        public string Result { get; set; }
    }

    // Custom converter for string to int conversion
    public class StringToIntConverter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();
                if (int.TryParse(stringValue, out int result))
                    return result;
                return 0; // Default value if parsing fails
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt32();
            }
            return 0;
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }

    // Custom converter for match number (extracts number from "1st Match", "2nd Match", etc.)
    public class MatchNumberConverter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();

                // Extract number from strings like "1st Match", "2nd Match", "Final", etc.
                var match = Regex.Match(stringValue, @"(\d+)");
                if (match.Success && int.TryParse(match.Groups[1].Value, out int result))
                {
                    return result;
                }

                // Handle special cases
                if (stringValue.ToLower().Contains("final"))
                    return 999; // Assign a high number for finals
                if (stringValue.ToLower().Contains("qualifier"))
                    return 900; // Assign number for qualifiers

                return 0; // Default value
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt32();
            }
            return 0;
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }

    // Custom converter for date strings like "22 Mar" to DateTime (default year 2025)
    public class DateStringConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();

                // Try to parse formats like "22 Mar"
                if (DateTime.TryParseExact($"{stringValue} 2025", "d MMM yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }

                // Try alternative format "22 March"
                if (DateTime.TryParseExact($"{stringValue} 2025", "d MMMM yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result2))
                {
                    return result2;
                }

                // Try to parse other common date formats
                if (DateTime.TryParse(stringValue, out DateTime result3))
                {
                    // If no year specified, set to 2025
                    if (result3.Year == DateTime.Now.Year && stringValue.Length <= 6)
                    {
                        return new DateTime(2025, result3.Month, result3.Day);
                    }
                    return result3;
                }

                return new DateTime(2025, 1, 1); // Default date if parsing fails
            }
            return DateTime.MinValue;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
        }
    }
}