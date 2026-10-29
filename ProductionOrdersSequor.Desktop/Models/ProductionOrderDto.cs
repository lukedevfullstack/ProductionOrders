using ProductionOrdersSequor.Desktop.Services;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class ProductionOrderDto
{
    [JsonPropertyName("order")]
    public string Order { get; set; } = string.Empty;

    [JsonPropertyName("quantity")]
    public decimal Quantity { get; set; }

    [JsonPropertyName("productCode")]
    public string ProductCode { get; set; } = string.Empty;

    [JsonPropertyName("productDescription")]
    public string ProductDescription { get; set; } = string.Empty;

    [JsonPropertyName("image")]
    public string? Image { get; set; } = string.Empty;

    [JsonConverter(typeof(FlexibleIntConverter))]
    [JsonPropertyName("cycleTime")]
    public int? CycleTime { get; set; }

    public List<MaterialDto> Materials { get; set; } = new();

    public override string ToString() => Order;
}

public class MaterialDto
{
    public string MaterialCode { get; set; } = string.Empty;
    public string MaterialDescription { get; set; } = string.Empty;
}

public class ProductionPayloadDto
{
    public string Email { get; set; } = string.Empty;
    public string Order { get; set; } = string.Empty;
    public string ProductionDate { get; set; } = string.Empty;
    public string ProductionTime { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string MaterialCode { get; set; } = string.Empty;
    public int CycleTime { get; set; }
}