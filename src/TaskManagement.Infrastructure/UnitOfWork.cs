using Microsoft.EntityFrameworkCore.Storage;
using TaskManagement.Application.Abstractions;
using TaskManagement.Infrastructure.DbContexts;

namespace TaskManagement.Infrastructure;

public class UnitOfWork(WriteDbContext dbContext) : IUnitOfWork
{
    public async Task<IDbContextTransaction> BeginTransaction(
        CancellationToken cancellationToken = default)
    {
        return await dbContext.
            Database.BeginTransactionAsync(cancellationToken);
    }
    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await dbContext.DisposeAsync();
    }
}