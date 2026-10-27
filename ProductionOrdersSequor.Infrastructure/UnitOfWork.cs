using ProductionOrdersSequor.Domain.Interfaces;
using ProductionOrdersSequor.Infrastructure.Context;

namespace ProductionOrdersSequor.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CommitAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
