using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Abstractions;
using TaskManagement.Application.Abstractions.CQ;
using TaskManagement.Application.Dto;
using TaskManagement.Domain.Errors;

namespace TaskManagement.Application.Features.Queries.Issue.GetIssueById;

public class GetIssueByIdHandler : 
    IQueryHandler<Result<IssueDto, Error>, GetIssueByIdQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetIssueByIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<IssueDto, Error>> Handle(
        GetIssueByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var issue = await _readDbContext.Issues.
            FirstOrDefaultAsync(
                v => v.Id == query.IssueId, cancellationToken);

        if (issue is null)
        {
            return Errors.General.
                NotFound("Issue not found");
        }
        
        return issue;
    }
}