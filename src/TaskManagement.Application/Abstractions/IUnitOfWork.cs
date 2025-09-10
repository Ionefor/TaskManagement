using Microsoft.EntityFrameworkCore.Storage;

namespace TaskManagement.Application.Abstractions;

public interface IUnitOfWork : IAsyncDisposable
{
    Task<IDbContextTransaction> BeginTransaction(
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}