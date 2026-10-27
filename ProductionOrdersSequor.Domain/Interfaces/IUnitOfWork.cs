namespace ProductionOrdersSequor.Domain.Interfaces;

public interface IUnitOfWork
{
    Task<bool> CommitAsync();
}