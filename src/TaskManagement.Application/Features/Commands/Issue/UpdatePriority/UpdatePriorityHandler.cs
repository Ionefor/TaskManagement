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

namespace TaskManagement.Application.Features.Commands.Issue.UpdatePriority;

public class UpdatePriorityHandler
    : ICommandHandler<Guid, UpdatePriorityCommand>
{
    private readonly ILogger<UpdatePriorityHandler> _logger;
    private readonly IIssueRepository _issueRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdatePriorityCommand> _validator;
    private readonly IReadDbContext _readDbContext;

    public UpdatePriorityHandler(
        ILogger<UpdatePriorityHandler> logger,
        IIssueRepository issueRepository,
        IUnitOfWork unitOfWork,
        IValidator<UpdatePriorityCommand> validator,
        IReadDbContext readDbContext)
    {
        _logger = logger;
        _issueRepository = issueRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _readDbContext = readDbContext;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdatePriorityCommand command,
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
        
            var issueResult = await _issueRepository.
                GetById(IssueId.Create(command.IssueId),
                    cancellationToken);
        
            if (issueResult.IsFailure)
                return issueResult.Error.ToErrorList();
        
            Enum.TryParse(typeof(IssuePriority),
                command.Priority, out var priority);
            
            var priorityEnum = (IssuePriority)priority!;
        
            issueResult.Value.UpdatePriority(priorityEnum);
        
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        
            _logger.LogInformation(
                "Update priority {priority} successfully" +
                " in issue with id: {issueId}",
                priorityEnum, issueResult.Value.Id.Id);

            await transaction.CommitAsync(cancellationToken);
            
            return issueResult.Value.Id.Id;
        }
        catch (Exception e)
        {
            _logger.LogError(
                e, "Failed to to update priority in issue {IssueId}",
                command.IssueId);
            
            await transaction.RollbackAsync(cancellationToken);

            return Errors.General.
                Failed("Fail to update priority").ToErrorList();
        }
    }
}