using ProductionOrdersSequor.Application.DTOs;

namespace ProductionOrdersSequor.Application.Interfaces;

public interface IOrderService
{
    Task<object> GetOrdersAsync();
    Task<object> GetProductionsByEmailAsync(string email);
    Task<ProductionResponseDto> SetProductionAsync(SetProductionDto dto);
}
