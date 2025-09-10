using CSharpFunctionalExtensions;
using TaskManagement.Domain;
using TaskManagement.Domain.Aggregate;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Errors;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Application.Abstractions;

public interface IIssueRepository
{
    Task<Guid> Add(
        Issue issue, CancellationToken cancellationToken = default);
    
    Guid Delete(Issue issue);
    
    Task<Result<Issue, Error>> GetById(
        IssueId issueId, CancellationToken cancellationToken = default);
    
    Task<Result<SubIssue, Error>> GetSubIssueById(
        SubIssueId issueId, CancellationToken cancellationToken = default);
}