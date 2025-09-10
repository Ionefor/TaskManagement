using TaskManagement.Application.Abstractions.CQ;

namespace TaskManagement.Application.Features.Queries.SubIssue.GetSubIssuesByIssueIdWithPagination;

public record GetSubIssuesByIssueIdWithPaginationQuery(
    Guid IssueId, int Page, int PageSize) : IQuery;