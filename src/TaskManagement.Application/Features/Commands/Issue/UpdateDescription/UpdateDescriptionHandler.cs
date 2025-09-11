using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.Abstractions;
using TaskManagement.Application.Abstractions.CQ;
using TaskManagement.Application.Extensions;
using TaskManagement.Domain.Errors;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Application.Features.Commands.Issue.UpdateDescription;

public class UpdateDescriptionHandler
    : ICommandHandler<Guid, UpdateDescriptionCommand>
{
    private readonly ILogger<UpdateDescriptionHandler> _logger;
    private readonly IIssueRepository _issueRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateDescriptionCommand> _validator;
    private readonly IReadDbContext _readDbContext;

    public UpdateDescriptionHandler(
        ILogger<UpdateDescriptionHandler> logger,
        IIssueRepository issueRepository,
        IUnitOfWork unitOfWork,
        IValidator<UpdateDescriptionCommand> validator,
        IReadDbContext readDbContext)
    {
        _logger = logger;
        _issueRepository = issueRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _readDbContext = readDbContext;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateDescriptionCommand command,
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
        
            var issueResult = await _issueRepository.
                GetById(IssueId.Create(command.IssueId),
                    cancellationToken);
        
            if (issueResult.IsFailure)
                return issueResult.Error.ToErrorList();
        
            var description = Description.
                Create(command.Description).Value;
        
            issueResult.Value.UpdateDescription(description);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Updated Description {Description} in issue" +
                " with id: {issueId}",
                description, issueResult.Value.Id.Id);
        
            await transaction.CommitAsync(cancellationToken);
            
            return issueResult.Value.Id.Id;
        }
        catch (Exception e)
        {
            _logger.LogError(
                e, "Failed to update description in issue {IssueId}",
                command.IssueId);
            
            await transaction.RollbackAsync(cancellationToken);

            return Errors.General.
                Failed("Fail to update description").ToErrorList();
        }
    }
}