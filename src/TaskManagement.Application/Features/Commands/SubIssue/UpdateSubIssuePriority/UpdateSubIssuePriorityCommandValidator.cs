using FluentValidation;
using TaskManagement.Application.Extensions;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Features.Commands.SubIssue.UpdateSubIssuePriority;

public class UpdateSubIssuePriorityCommandValidator :
    AbstractValidator<UpdateSubIssuePriorityCommand>
{
    public UpdateSubIssuePriorityCommandValidator()
    {
        RuleFor(x => x.SubIssueId).
            NotEmpty();
        
        RuleFor(x => x.IssueId).
            NotEmpty();
        
        RuleFor(a => a.Priority).
            MustBeEnum<UpdateSubIssuePriorityCommand, string, IssuePriority>();
    }
}