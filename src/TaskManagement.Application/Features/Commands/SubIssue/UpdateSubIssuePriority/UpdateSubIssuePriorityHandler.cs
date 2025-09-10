using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.Abstractions;
using TaskManagement.Application.Abstractions.CQ;
using TaskManagement.Application.Extensions;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Errors;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Application.Features.Commands.SubIssue.UpdateSubIssuePriority;

public class UpdateSubIssuePriorityHandler :
    ICommandHandler<Guid, UpdateSubIssuePriorityCommand>
{
    private readonly ILogger<UpdateSubIssuePriorityHandler> _logger;
    private readonly IIssueRepository _issueRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateSubIssuePriorityCommand> _validator;
    private readonly IReadDbContext _readDbContext;

    public UpdateSubIssuePriorityHandler(
        ILogger<UpdateSubIssuePriorityHandler> logger,
        IIssueRepository issueRepository,
        IUnitOfWork unitOfWork,
        IValidator<UpdateSubIssuePriorityCommand> validator,
        IReadDbContext readDbContext)
    {
        _logger = logger;
        _issueRepository = issueRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _readDbContext = readDbContext;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateSubIssuePriorityCommand command,
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
        
            var subIssueId = SubIssueId.
                Create(command.SubIssueId);
        
            Enum.TryParse(typeof(IssuePriority),
                command.Priority, out var priority);
        
            var priorityEnum = (IssuePriority)priority!;
        
            var updatePriorityResult = issueResult.Value.
                UpdateSubIssuePriority(subIssueId, priorityEnum);
        
            if(updatePriorityResult.IsFailure)
                return updatePriorityResult.Error.ToErrorList();
        
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        
            _logger.LogInformation(
                "Priority {Priority} was updated for SubIssue" +
                " {SubIssueId} in Issue {IssueId}",
                priorityEnum,
                subIssueId.Id,
                issueResult.Value.Id.Id);
        
            await transaction.CommitAsync(cancellationToken);
            
            return subIssueId.Id;
        }
        catch (Exception e)
        {
            _logger.LogError(
                e, "Failed to update priority SubIssue {SubIssueId}" +
                   " in Issue {IssueId}",
                command.SubIssueId,
                command.IssueId);
            
            await transaction.RollbackAsync(cancellationToken);

            return Errors.General.
                Failed("Fail to update priority").ToErrorList();
        }
    }
}