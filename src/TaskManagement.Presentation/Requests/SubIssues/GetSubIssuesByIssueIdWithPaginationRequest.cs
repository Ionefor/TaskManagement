using TaskManagement.Application.Features.Queries.SubIssue.GetSubIssuesByIssueIdWithPagination;

namespace TaskManagement.Presentation.Requests.SubIssues;

public record GetSubIssuesByIssueIdWithPaginationRequest(int Page, int PageSize)
{
    public GetSubIssuesByIssueIdWithPaginationQuery ToQuery(Guid issueId) =>
        new(issueId, Page, PageSize);
}