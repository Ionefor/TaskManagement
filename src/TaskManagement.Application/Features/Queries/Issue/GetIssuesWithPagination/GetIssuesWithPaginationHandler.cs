using TaskManagement.Application.Abstractions;
using TaskManagement.Application.Abstractions.CQ;
using TaskManagement.Application.Abstractions.Models;
using TaskManagement.Application.Dto;
using TaskManagement.Application.Extensions;

namespace TaskManagement.Application.Features.Queries.Issue.GetIssuesWithPagination;

public class GetIssuesWithPaginationHandler : 
    IQueryHandler<PageList<IssueDto>, GetIssuesWithPaginationQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetIssuesWithPaginationHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PageList<IssueDto>> Handle(
        GetIssuesWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        var issues = _readDbContext.Issues;
        
        return await issues.
            ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}