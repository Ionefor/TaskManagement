using TaskManagement.Application.Abstractions;
using TaskManagement.Application.Abstractions.CQ;

namespace TaskManagement.Application.Features.Queries.Issue.GetIssuesWithPagination;

public record GetIssuesWithPaginationQuery(int Page, int PageSize) : IQuery;