using FluentValidation;
using TaskManagement.Application.Extensions;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Application.Features.Commands.SubIssue.SetSubIssueAssignee;

public class SetSubIssueAssigneeCommandValidator :
    AbstractValidator<SetSubIssueAssigneeCommand>
{
    public SetSubIssueAssigneeCommandValidator()
    {
        RuleFor(c => c.IssueId).
            NotEmpty();
        
        RuleFor(c => c.SubIssueId).
            NotEmpty();
        
        RuleFor(c => c.Assignee).
            MustBeValueObject(Name.Create);
    }
}