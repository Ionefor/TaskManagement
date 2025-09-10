using FluentValidation;
using TaskManagement.Application.Extensions;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Application.Features.Commands.Issue.UpdateStatus;

public class UpdateStatusCommandValidator :
    AbstractValidator<UpdateStatusCommand>
{
    public UpdateStatusCommandValidator()
    {
        RuleFor(u => u.IssueId).NotEmpty();
        
        RuleFor(u => u.Status).
            MustBeEnum<UpdateStatusCommand, string, IssueStatus>();
    }
}