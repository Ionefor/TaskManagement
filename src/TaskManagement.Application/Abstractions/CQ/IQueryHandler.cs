namespace TaskManagement.Application.Abstractions.CQ;

public interface IQueryHandler<TResponse, in TQuery> where TQuery : IQuery
{
    public Task<TResponse> Handle(
        TQuery query, CancellationToken cancellationToken = default);
}