using FluentValidation;
using TaskManagement.Application.Extensions;
using TaskManagement.Application.Features.Commands.Issue.CreateIssue;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Application.Features.Commands.SubIssue.CreateSubIssue;

public class CreateSubIssueCommandValidator : AbstractValidator<CreateSubIssueCommand>
{
    public CreateSubIssueCommandValidator()
    {
        RuleFor(c => c.IssueId).NotEmpty();
        
        RuleFor(c => c.Title).
            MustBeValueObject(Title.Create);
        
        RuleFor(c => c.Description).
            MustBeValueObject(Description.Create);
        
        RuleFor(c => c.Author).
            MustBeValueObject(Name.Create);
        
        RuleFor(a => a.Status).
            MustBeEnum<CreateSubIssueCommand, string, IssueStatus>();
        
        RuleFor(a => a.Priority).
            MustBeEnum<CreateSubIssueCommand, string, IssuePriority>();
    }
}