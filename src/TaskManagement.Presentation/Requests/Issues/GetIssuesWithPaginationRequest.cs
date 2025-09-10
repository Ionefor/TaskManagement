using TaskManagement.Application.Features.Queries.Issue.GetIssuesWithPagination;

namespace TaskManagement.Presentation.Requests.Issues;

public record GetIssuesWithPaginationRequest(int Page, int PageSize)
{
    public GetIssuesWithPaginationQuery ToQuery() =>
        new(Page, PageSize);
}