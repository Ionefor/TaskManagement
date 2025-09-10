using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.Abstractions;
using TaskManagement.Application.Abstractions.CQ;
using TaskManagement.Domain.Errors;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Application.Features.Commands.Issue.DeleteIssue;

public class DeleteIssueHandler :
    ICommandHandler<Guid, DeleteIssueCommand>
{    
    private readonly ILogger<DeleteIssueHandler> _logger;
    private readonly IIssueRepository _issueRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReadDbContext _readDbContext;

    public DeleteIssueHandler(
        ILogger<DeleteIssueHandler> logger,
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
        DeleteIssueCommand command,
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
        
            var issueResult = await _issueRepository.
                GetById(IssueId.Create(command.IssueId),
                    cancellationToken);
        
            if (issueResult.IsFailure)
                return issueResult.Error.ToErrorList();
        
            _issueRepository.Delete(issueResult.Value);
        
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        
            _logger.LogInformation(
                "Issue with id: {IssueId} was deleted successfully",
                issueResult.Value.Id.Id);

            await transaction.CommitAsync(cancellationToken);
            
            return issueResult.Value.Id.Id;
        }
        catch (Exception e)
        {
            _logger.LogError(
                e, "Failed to delete issue {IssueId}",
                command.IssueId);
            
            await transaction.RollbackAsync(cancellationToken);

            return Errors.General.
                Failed("Fail to delete issue").ToErrorList();
        }
    }
}