using TaskManagement.Application.Dto;

namespace TaskManagement.Application.Abstractions;

public interface IReadDbContext
{
    IQueryable<IssueDto> Issues { get; }
    
    IQueryable<SubIssueDto> SubIssues { get; }
}