using ProductionOrdersSequor.Desktop.Services;
using System.Text.Json.Serialization;

namespace ProductionOrdersSequor.Desktop
{
    public class ProductionHistoryDto
    {
        [JsonPropertyName("order")]
        public string Order { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("date")]
        public string DateRaw { get; set; } = string.Empty;

        public string ProductionDate => !string.IsNullOrEmpty(DateRaw) && DateRaw.Contains(' ') ? DateRaw.Split(' ')[0] : DateRaw;

        public string ProductionTime
        {
            get
            {
                if (string.IsNullOrEmpty(DateRaw)) return string.Empty;
                var parts = DateRaw.Split(' ');

                if (parts.Length >= 3)
                {
                    return $"{parts[1]} {parts[2]}";
                }
                return parts.Length > 1 ? parts[1] : string.Empty;
            }
        }

        [JsonPropertyName("quantity")]
        public decimal Quantity { get; set; }

        [JsonPropertyName("materialDescription")]
        public string MaterialDescription { get; set; } = string.Empty;

        [JsonConverter(typeof(FlexibleIntConverter))]
        [JsonPropertyName("cycleTime")]
        public int? CycleTime { get; set; }
    }
}