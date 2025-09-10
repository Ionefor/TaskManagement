using TaskManagement.Application.Abstractions;
using TaskManagement.Application.Abstractions.CQ;
using TaskManagement.Application.Abstractions.Models;
using TaskManagement.Application.Dto;
using TaskManagement.Application.Extensions;

namespace TaskManagement.Application.Features.Queries.SubIssue.GetSubIssuesByIssueIdWithPagination;

public class GetSubIssuesByIssueIdWithPaginationHandler : 
    IQueryHandler<PageList<SubIssueDto>, GetSubIssuesByIssueIdWithPaginationQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetSubIssuesByIssueIdWithPaginationHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PageList<SubIssueDto>> Handle(
        GetSubIssuesByIssueIdWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        var subIssues = _readDbContext.SubIssues.
            Where(s => s.IssueId == query.IssueId);
        
        return await subIssues.
            ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}