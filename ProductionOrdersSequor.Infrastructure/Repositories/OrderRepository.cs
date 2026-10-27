using Microsoft.EntityFrameworkCore;
using ProductionOrdersSequor.Domain.Entities;
using ProductionOrdersSequor.Domain.Interfaces;
using ProductionOrdersSequor.Infrastructure.Context;

namespace ProductionOrdersSequor.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersWithDetailsAsync()
    {
        return await _context.Orders
            .Include(o => o.Product)
                .ThenInclude(p => p.ProductMaterials)
                    .ThenInclude(pm => pm.Material)
            .ToListAsync();
    }

    public async Task<Order> GetByCodeAsync(string orderCode)
    {
        return await _context.Orders
            .Include(o => o.Product)
                .ThenInclude(p => p.ProductMaterials)
            .FirstOrDefaultAsync(o => o.OrderCode == orderCode);
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<Production>> GetProductionsByEmailAsync(string email)
    {
        return await _context.Productions.Where(p => p.Email == email).ToListAsync();
    }

    public async Task AddProductionAsync(Production production)
    {
        await _context.Productions.AddAsync(production);
    }
}
