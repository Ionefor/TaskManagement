using FluentValidation;
using TaskManagement.Application.Extensions;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Application.Features.Commands.Issue.UpdateDescription;

public class UpdateDescriptionCommandValidator :
    AbstractValidator<UpdateDescriptionCommand>
{
    public UpdateDescriptionCommandValidator()
    {
        RuleFor(u => u.IssueId).NotEmpty();
        
        RuleFor(u => u.Description).
            MustBeValueObject(Description.Create);
    }
}