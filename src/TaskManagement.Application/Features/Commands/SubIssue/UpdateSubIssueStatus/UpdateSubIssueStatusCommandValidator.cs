using FluentValidation;
using TaskManagement.Application.Extensions;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Features.Commands.SubIssue.UpdateSubIssueStatus;

public class UpdateSubIssueStatusCommandValidator :
    AbstractValidator<UpdateSubIssueStatusCommand>
{
    public UpdateSubIssueStatusCommandValidator()
    {
        RuleFor(x => x.SubIssueId).
            NotEmpty();
        
        RuleFor(x => x.IssueId).
            NotEmpty();
        
        RuleFor(a => a.Status).
            MustBeEnum<UpdateSubIssueStatusCommand, string, IssueStatus>();
    }
}