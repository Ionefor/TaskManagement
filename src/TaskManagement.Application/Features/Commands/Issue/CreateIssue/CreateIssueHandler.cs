using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.Abstractions;
using TaskManagement.Application.Abstractions.CQ;
using TaskManagement.Application.Extensions;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Errors;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Application.Features.Commands.Issue.CreateIssue;

public class CreateIssueHandler
    : ICommandHandler<Guid, CreateIssueCommand>
{
    private readonly ILogger<CreateIssueHandler> _logger;
    private readonly IValidator<CreateIssueCommand> _validator;
    private readonly IIssueRepository _issueRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateIssueHandler(
        ILogger<CreateIssueHandler> logger,
        IValidator<CreateIssueCommand> validator,
        IIssueRepository issueRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _validator = validator;
        _issueRepository = issueRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CreateIssueCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.
            ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var issueId = IssueId.NewGuid();

        var title = Title.Create(command.Title).Value;
        
        var description = Description.
            Create(command.Description).Value;
        
        var author = Name.Create(command.Author).Value;
        
        Enum.TryParse(typeof(IssueStatus),
            command.Status, out var status);
        
        var statusEnum = (IssueStatus)status!;
        
        Enum.TryParse(typeof(IssuePriority),
            command.Priority, out var priority);
        
        var priorityEnum = (IssuePriority)priority!;
        
        var issue = new Domain.Aggregate.Issue(
            issueId, title, description,
            statusEnum, priorityEnum, author);
        
        await _issueRepository.Add(issue, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation(
            "Issue created with id: {issueId}", issueId);
        
        return issue.Id.Id;
    }
}