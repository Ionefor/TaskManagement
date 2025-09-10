using FluentValidation;
using TaskManagement.Application.Extensions;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Application.Features.Commands.Issue.CreateIssue;

public class CreateIssueCommandValidator : AbstractValidator<CreateIssueCommand>
{
    public CreateIssueCommandValidator()
    {
        RuleFor(c => c.Title).
            MustBeValueObject(Title.Create);
        
        RuleFor(c => c.Description).
            MustBeValueObject(Description.Create);
        
        RuleFor(c => c.Author).
            MustBeValueObject(Name.Create);
        
        RuleFor(a => a.Status).
            MustBeEnum<CreateIssueCommand, string, IssueStatus>();
        
        RuleFor(a => a.Priority).
            MustBeEnum<CreateIssueCommand, string, IssuePriority>();
    }
}