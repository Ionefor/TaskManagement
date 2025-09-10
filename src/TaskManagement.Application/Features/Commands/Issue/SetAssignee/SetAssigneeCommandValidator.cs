using FluentValidation;
using TaskManagement.Application.Extensions;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Application.Features.Commands.Issue.SetAssignee;

public class SetAssigneeCommandValidator : AbstractValidator<SetAssigneeCommand>
{
    public SetAssigneeCommandValidator()
    {
        RuleFor(u => u.IssueId).NotEmpty();
        
        RuleFor(c => c.Assignee).
            MustBeValueObject(Name.Create);
    }
}