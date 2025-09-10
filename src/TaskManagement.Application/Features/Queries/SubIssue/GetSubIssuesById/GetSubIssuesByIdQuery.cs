using TaskManagement.Application.Abstractions.CQ;

namespace TaskManagement.Application.Features.Queries.SubIssue.GetSubIssuesById;

public record GetSubIssuesByIdQuery(Guid IssueId, Guid SubIssueId) : IQuery;