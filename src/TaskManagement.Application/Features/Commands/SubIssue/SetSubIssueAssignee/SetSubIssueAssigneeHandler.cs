using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.Abstractions;
using TaskManagement.Application.Abstractions.CQ;
using TaskManagement.Application.Extensions;
using TaskManagement.Domain.Errors;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Application.Features.Commands.SubIssue.SetSubIssueAssignee;

public class SetSubIssueAssigneeHandler :
    ICommandHandler<Guid, SetSubIssueAssigneeCommand>
{
    private readonly ILogger<SetSubIssueAssigneeHandler> _logger;
    private readonly IIssueRepository _issueRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<SetSubIssueAssigneeCommand> _validator;
    private readonly IReadDbContext _readDbContext;

    public SetSubIssueAssigneeHandler(
        ILogger<SetSubIssueAssigneeHandler> logger,
        IIssueRepository issueRepository,
        IUnitOfWork unitOfWork,
        IValidator<SetSubIssueAssigneeCommand> validator,
        IReadDbContext readDbContext)
    {
        _logger = logger;
        _issueRepository = issueRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _readDbContext = readDbContext;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        SetSubIssueAssigneeCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.
            ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        await using var transaction = await
            _unitOfWork.BeginTransaction(cancellationToken);

        try
        {
            var issueExist = await _readDbContext.Issues.
                AnyAsync(i => i.Id == command.IssueId,
                    cancellationToken);

            if (!issueExist)
            {
                return Errors.General.
                    NotFound("Issue not found").ToErrorList();
            }
        
            var subIssueExist = await _readDbContext.SubIssues.
                AnyAsync(i => i.Id == command.SubIssueId,
                    cancellationToken);

            if (!subIssueExist)
            {
                return Errors.General.
                    NotFound("SubIssue not found").ToErrorList();
            }
        
            var issueResult = await _issueRepository.
                GetById(IssueId.Create(command.IssueId),
                    cancellationToken);
        
            if (issueResult.IsFailure)
                return issueResult.Error.ToErrorList();
        
            var subIssueId = SubIssueId.Create(command.SubIssueId);

            var assignee = Name.Create(command.Assignee).Value;
        
            var assigneeResult = issueResult.Value.
                SetSubIssueAssignee(subIssueId, assignee);
        
            if(assigneeResult.IsFailure)
                return assigneeResult.Error.ToErrorList();
        
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        
            _logger.LogInformation(
                "Assignee {Assignee} was set for SubIssue" +
                " {SubIssueId} in Issue {IssueId}",
                assignee.Value,
                subIssueId.Id,
                issueResult.Value.Id.Id);
        
            await transaction.CommitAsync(cancellationToken);
            
            return subIssueId.Id;
        }
        catch (Exception e)
        {
            _logger.LogError(
                e, "Failed to set assignee SubIssue {SubIssueId}" +
                   " in Issue {IssueId}",
                command.SubIssueId,
                command.IssueId);
            
            await transaction.RollbackAsync(cancellationToken);

            return Errors.General.
                Failed("Fail to set assignee").ToErrorList();
        }
    }
}