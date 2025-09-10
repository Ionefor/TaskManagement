using TaskManagement.Application.Abstractions;
using TaskManagement.Application.Abstractions.CQ;

namespace TaskManagement.Application.Features.Queries.Issue.GetIssueById;

public record GetIssueByIdQuery(Guid IssueId) : IQuery;