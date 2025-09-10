using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.Abstractions;
using TaskManagement.Application.Abstractions.CQ;
using TaskManagement.Domain.Errors;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Application.Features.Commands.SubIssue.DeleteSubIssue;

public class DeleteSubIssueHandler :
    ICommandHandler<Guid, DeleteSubIssueCommand>
{
    private readonly ILogger<DeleteSubIssueHandler> _logger;
    private readonly IIssueRepository _issueRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReadDbContext _readDbContext;

    public DeleteSubIssueHandler(
        ILogger<DeleteSubIssueHandler> logger,
        IIssueRepository issueRepository,
        IUnitOfWork unitOfWork,
        IReadDbContext readDbContext)
    {
        _logger = logger;
        _issueRepository = issueRepository;
        _unitOfWork = unitOfWork;
        _readDbContext = readDbContext;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteSubIssueCommand command,
        CancellationToken cancellationToken = default)
    {
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
        
            var subIssueResult = await _issueRepository.
                GetSubIssueById(SubIssueId.
                    Create(command.SubIssueId),
                    cancellationToken);
        
            if (subIssueResult.IsFailure)
                return subIssueResult.Error.ToErrorList();
        
            issueResult.Value.RemoveSubIssue(subIssueResult.Value);
        
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        
            _logger.LogInformation(
                "SubIssue with id: {SubIssueId} in Issue {IssueId}" +
                " was deleted successfully",
                subIssueResult.Value.Id.Id,
                issueResult.Value.Id.Id);
        
            await transaction.CommitAsync(cancellationToken);
            
            return subIssueResult.Value.Id.Id;
        }
        catch (Exception e)
        {
            _logger.LogError(
                e, "Failed to delete SubIssue {SubIssueId}" +
                   " in issue {IssueId}",
                command.SubIssueId,
                command.IssueId);
            
            await transaction.RollbackAsync(cancellationToken);

            return Errors.General.
                Failed("Fail to delete SubIssue").ToErrorList();
        }
    }
}