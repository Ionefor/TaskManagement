using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.Abstractions;
using TaskManagement.Application.Abstractions.CQ;
using TaskManagement.Application.Extensions;
using TaskManagement.Domain.Errors;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Application.Features.Commands.SubIssue.UpdateSubIssueTitle;

public class UpdateSubIssueTitleHandler :
    ICommandHandler<Guid, UpdateSubIssueTitleCommand>
{
    private readonly ILogger<UpdateSubIssueTitleHandler> _logger;
    private readonly IIssueRepository _issueRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReadDbContext _readDbContext;
    private readonly IValidator<UpdateSubIssueTitleCommand> _validator;

    public UpdateSubIssueTitleHandler(
        ILogger<UpdateSubIssueTitleHandler> logger,
        IIssueRepository issueRepository,
        IUnitOfWork unitOfWork,
        IReadDbContext readDbContext,
        IValidator<UpdateSubIssueTitleCommand> validator)
    {
        _logger = logger;
        _issueRepository = issueRepository;
        _unitOfWork = unitOfWork;
        _readDbContext = readDbContext;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateSubIssueTitleCommand command,
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

            var title = Title.Create(command.Title).Value;

            var updateTitleResult = issueResult.Value.
                UpdateSubIssueTitle(subIssueId, title);
        
            if(updateTitleResult.IsFailure)
                return updateTitleResult.Error.ToErrorList();
        
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        
            _logger.LogInformation(
                "Title {Title} was update for SubIssue" +
                " {SubIssueId} in Issue {IssueId}",
                title.Value,
                subIssueId.Id,
                issueResult.Value.Id.Id);
        
            await transaction.CommitAsync(cancellationToken);
            
            return subIssueId.Id;
        }
        catch (Exception e)
        {
            _logger.LogError(
                e, "Failed to update title SubIssue {SubIssueId}" +
                   " in Issue {IssueId}",
                command.SubIssueId,
                command.IssueId);
            
            await transaction.RollbackAsync(cancellationToken);

            return Errors.General.
                Failed("Fail to update title").ToErrorList();
        }
    }
}