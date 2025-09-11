using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Abstractions;
using TaskManagement.Application.Abstractions.CQ;
using TaskManagement.Application.Dto;
using TaskManagement.Domain.Errors;

namespace TaskManagement.Application.Features.Queries.SubIssue.GetSubIssuesById;

public class GetSubIssuesByIdHandler : 
    IQueryHandler<Result<SubIssueDto, Error>, GetSubIssuesByIdQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetSubIssuesByIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<SubIssueDto, Error>> Handle(
        GetSubIssuesByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var subIssue = await _readDbContext.SubIssues.
            FirstOrDefaultAsync(
                v => v.Id == query.SubIssueId, cancellationToken);
        
        if (subIssue is null)
        {
            return Errors.
                General.NotFound(nameof(Issue));
        }
        
        return subIssue;
    }
}