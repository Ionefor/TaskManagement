using FluentValidation;
using TaskManagement.Application.Extensions;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Features.Commands.Issue.UpdatePriority;

public class UpdatePriorityCommandValidator :
    AbstractValidator<UpdatePriorityCommand>
{
    public UpdatePriorityCommandValidator()
    {
        RuleFor(u => u.IssueId).NotEmpty();
        
        RuleFor(u => u.Priority).
            MustBeEnum<UpdatePriorityCommand, string, IssuePriority>();
    }
}