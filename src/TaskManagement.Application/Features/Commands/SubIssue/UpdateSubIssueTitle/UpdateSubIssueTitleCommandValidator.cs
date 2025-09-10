using FluentValidation;
using TaskManagement.Application.Extensions;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Application.Features.Commands.SubIssue.UpdateSubIssueTitle;

public class UpdateSubIssueTitleCommandValidator :
    AbstractValidator<UpdateSubIssueTitleCommand>
{
    public UpdateSubIssueTitleCommandValidator()
    {
        RuleFor(x => x.SubIssueId).
            NotEmpty();
        
        RuleFor(x => x.IssueId).
            NotEmpty();
        
        RuleFor(c => c.Title).
            MustBeValueObject(Title.Create);
    }
}