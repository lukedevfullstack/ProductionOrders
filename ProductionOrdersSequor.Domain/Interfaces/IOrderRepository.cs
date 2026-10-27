using ProductionOrdersSequor.Domain.Entities;

namespace ProductionOrdersSequor.Domain.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllOrdersWithDetailsAsync();
    Task<Order> GetByCodeAsync(string orderCode);
    Task<User> GetUserByEmailAsync(string email);
    Task<IEnumerable<Production>> GetProductionsByEmailAsync(string email);
    Task AddProductionAsync(Production production);
}