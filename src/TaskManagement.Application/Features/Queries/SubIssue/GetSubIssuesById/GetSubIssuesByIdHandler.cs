using CSharpFunctionalExtensions;
using TaskManagement.Application.Abstractions.CQ;
using TaskManagement.Application.Dto;
using TaskManagement.Domain.Errors;

namespace TaskManagement.Application.Features.Queries.SubIssue.GetSubIssuesById;

public class GetSubIssuesByIdHandler : 
    IQueryHandler<Result<SubIssueDto, Error>, GetSubIssuesByIdQuery>
{
    public Task<Result<SubIssueDto, Error>> Handle(
        GetSubIssuesByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}