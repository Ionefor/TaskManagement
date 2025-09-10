using FluentValidation;
using TaskManagement.Application.Extensions;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Application.Features.Commands.SubIssue.UpdateSubIssueDescription;

public class UpdateDescriptionCommandValidator : 
    AbstractValidator<UpdateSubIssueDescriptionCommand>
{
    public UpdateDescriptionCommandValidator()
    {
        RuleFor(x => x.SubIssueId).
            NotEmpty();
        
        RuleFor(x => x.IssueId).
            NotEmpty();
        
        RuleFor(c => c.Description).
            MustBeValueObject(Description.Create);
    }
}