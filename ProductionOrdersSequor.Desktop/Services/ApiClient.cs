using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProductionOrdersSequor.Desktop.Services
{
    public static class ApiClient
    {
        private static readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7079/api/")
        };

        public static async Task<List<ProductionOrderDto>> GetOrdersAsync()
        {
            try
            {
                var response = await client.GetAsync("orders/GetOrders");
                if (!response.IsSuccessStatusCode) return new();

                var jsonString = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.AllowNamedFloatingPointLiterals
                };

                using var doc = JsonDocument.Parse(jsonString);
                var root = doc.RootElement;

                if (root.ValueKind == JsonValueKind.Array)
                {
                    return JsonSerializer.Deserialize<List<ProductionOrderDto>>(jsonString, options) ?? new();
                }
                else if (root.ValueKind == JsonValueKind.Object)
                {
                    foreach (var prop in root.EnumerateObject())
                    {
                        if (prop.Value.ValueKind == JsonValueKind.Array)
                        {
                            return JsonSerializer.Deserialize<List<ProductionOrderDto>>(prop.Value.GetRawText(), options) ?? new();
                        }
                    }
                }

                return new();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar ordens: {ex.Message}");
                return new();
            }
        }

        public static async Task<HttpResponseMessage> SendProductionAsync(ProductionPayloadDto payload)
        {
            return await client.PostAsJsonAsync("orders/SetProduction", payload);
        }

        public static async Task<List<ProductionHistoryDto>> GetHistoryAsync(string email = "")
        {
            try
            {
                string url = string.IsNullOrEmpty(email) ? "orders/GetProduction" : $"orders/GetProduction?email={email}";
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode) return new();

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                // 1. Tenta desserializar diretamente como uma lista (caso venha direto como [...])
                try
                {
                    var list = JsonSerializer.Deserialize<List<ProductionHistoryDto>>(jsonResponse, options);
                    if (list != null) return list;
                }
                catch { }

                // 2. Se falhar, tenta ler como um objeto genérico e procurar por propriedades comuns (ex: data, items, value)
                using var doc = JsonDocument.Parse(jsonResponse);
                var root = doc.RootElement;

                if (root.ValueKind == JsonValueKind.Object)
                {
                    foreach (var prop in root.EnumerateObject())
                    {
                        if (prop.Value.ValueKind == JsonValueKind.Array)
                        {
                            try
                            {
                                var nestedList = JsonSerializer.Deserialize<List<ProductionHistoryDto>>(prop.Value.GetRawText(), options);
                                if (nestedList != null) return nestedList;
                            }
                            catch { }
                        }
                    }
                }

                return new();
            }
            catch
            {
                return new();
            }
        }
    }
}