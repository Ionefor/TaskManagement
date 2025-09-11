using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Abstractions;
using TaskManagement.Domain;
using TaskManagement.Domain.Aggregate;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Errors;
using TaskManagement.Domain.ValueObjects;
using TaskManagement.Infrastructure.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;

public class IssueRepository : IIssueRepository
{
    private readonly WriteDbContext _dbContext;

    public IssueRepository(WriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Guid> Add(
        Issue issue, CancellationToken cancellationToken = default)
    {
        await _dbContext.Issues.
            AddAsync(issue, cancellationToken);
        
        return issue.Id;
    }

    public Guid Delete(Issue issue)
    {
        _dbContext.Issues.Remove(issue);
        
        return issue.Id;
    }

    public async Task<Result<Issue, Error>> GetById(
        IssueId issueId, CancellationToken cancellationToken = default)
    {
        var issue = await _dbContext.Issues.
            Include(i => i.SubIssues)
            .FirstOrDefaultAsync(i => i.Id == issueId,
                cancellationToken);

        if (issue is null)
        {
            return Errors.General.
                NotFound(nameof(Issue));
        }

        return issue;
    }

    public async Task<Result<SubIssue, Error>> GetSubIssueById(
        SubIssueId issueId, CancellationToken cancellationToken = default)
    {
        var issues = _dbContext.Issues.
            Include(i => i.SubIssues);
        
        var issue = await issues.
            FirstOrDefaultAsync(v => v.SubIssues!.
                Any(p => p.Id == issueId), cancellationToken);
        
        if (issue is null)
        {
            return Errors.General.
                NotFound(nameof(Issue));
        }
        
        var subIssue = issue.SubIssues.
            FirstOrDefault(p => p.Id == issueId);
        
        if (subIssue is null)
        {
            return Errors.General.
                NotFound(nameof(SubIssue));
        }
        
        return subIssue;
    }
}