using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.Abstractions;
using TaskManagement.Application.Abstractions.CQ;
using TaskManagement.Application.Extensions;
using TaskManagement.Domain.Errors;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Application.Features.Commands.SubIssue.UpdateSubIssueDescription;

public class UpdateSubIssueDescriptionHandler :
    ICommandHandler<Guid, UpdateSubIssueDescriptionCommand>
{
    private readonly ILogger<UpdateSubIssueDescriptionHandler> _logger;
    private readonly IIssueRepository _issueRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateSubIssueDescriptionCommand> _validator;
    private readonly IReadDbContext _readDbContext;

    public UpdateSubIssueDescriptionHandler(
        ILogger<UpdateSubIssueDescriptionHandler> logger,
        IIssueRepository issueRepository,
        IUnitOfWork unitOfWork,
        IValidator<UpdateSubIssueDescriptionCommand> validator,
        IReadDbContext readDbContext)
    {
        _logger = logger;
        _issueRepository = issueRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _readDbContext = readDbContext;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateSubIssueDescriptionCommand command,
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
                    NotFound(nameof(Issue)).ToErrorList();
            }
        
            var subIssueExist = await _readDbContext.SubIssues.
                AnyAsync(i => i.Id == command.SubIssueId,
                    cancellationToken);

            if (!subIssueExist)
            {
                return Errors.General.
                    NotFound(nameof(SubIssue)).ToErrorList();
            }
        
            var issueResult = await _issueRepository.
                GetById(IssueId.Create(command.IssueId),
                    cancellationToken);
        
            if (issueResult.IsFailure)
                return issueResult.Error.ToErrorList();
        
            var subIssueId = SubIssueId.
                Create(command.SubIssueId);
        
            var description = Description.
                Create(command.Description).Value;
        
            var descriptionUpdateResult = issueResult.Value.
                UpdateSubIssueDescription(subIssueId, description);
        
            if(descriptionUpdateResult.IsFailure)
                return descriptionUpdateResult.Error.ToErrorList();
        
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        
            _logger.LogInformation(
                "Description {Description} was updated for" +
                " SubIssue {SubIssueId}" +
                " in Issue {IssueId}",
                description.Value,
                subIssueId.Id,
                issueResult.Value.Id.Id);
        
            await transaction.CommitAsync(cancellationToken);
            
            return subIssueId.Id;
        }
        catch (Exception e)
        {
            _logger.LogError(
                e, "Failed to update description SubIssue" +
                   " {SubIssueId} in Issue {IssueId}",
                command.SubIssueId,
                command.IssueId);
            
            await transaction.RollbackAsync(cancellationToken);

            return Errors.General.
                Failed("Fail to update description").ToErrorList();
        }
    }
}