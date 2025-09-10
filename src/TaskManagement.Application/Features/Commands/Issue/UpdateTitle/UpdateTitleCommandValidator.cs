using FluentValidation;
using TaskManagement.Application.Extensions;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Application.Features.Commands.Issue.UpdateTitle;

public class UpdateTitleCommandValidator : AbstractValidator<UpdateTitleCommand>
{
    public UpdateTitleCommandValidator()
    {
        RuleFor(u => u.IssueId).NotEmpty();
        
        RuleFor(u => u.Title).
            MustBeValueObject(Title.Create);
    }
}