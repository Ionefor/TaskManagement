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

namespace TaskManagement.Application.Features.Commands.SubIssue.CreateSubIssue;

public class CreateSubIssueHandler
    : ICommandHandler<Guid, CreateSubIssueCommand>
{
    private readonly ILogger<CreateSubIssueHandler> _logger;
    private readonly IValidator<CreateSubIssueCommand> _validator;
    private readonly IIssueRepository _issueRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReadDbContext _readDbContext;

    public CreateSubIssueHandler(
        ILogger<CreateSubIssueHandler> logger,
        IValidator<CreateSubIssueCommand> validator,
        IIssueRepository issueRepository,
        IUnitOfWork unitOfWork,
        IReadDbContext readDbContext)
    {
        _logger = logger;
        _validator = validator;
        _issueRepository = issueRepository;
        _unitOfWork = unitOfWork;
        _readDbContext = readDbContext;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CreateSubIssueCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.
            ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var issueExist = await _readDbContext.Issues.
            AnyAsync(i => i.Id == command.IssueId, cancellationToken);

        if (!issueExist)
        {
            return Errors.General.
                NotFound("Issue not found").ToErrorList();
        }
        
        var issueResult = await _issueRepository.
            GetById(IssueId.Create(command.IssueId), cancellationToken);
        
        if (issueResult.IsFailure)
            return issueResult.Error.ToErrorList();
        
        var subIssueId = SubIssueId.NewGuid();

        var title = Title.Create(command.Title).Value;
        
        var description = Description.Create(command.Description).Value;
        
        var author = Name.Create(command.Author).Value;
        
        Enum.TryParse(typeof(IssueStatus), command.Status, out var status);
        var statusEnum = (IssueStatus)status!;
        
        Enum.TryParse(typeof(IssuePriority), command.Priority, out var priority);
        var priorityEnum = (IssuePriority)priority!;
        
        var subIssue = new Domain.Entities.SubIssue(
            subIssueId, title, description,
            statusEnum, priorityEnum, author);
        
        issueResult.Value.AddSubIssue(subIssue);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation(
            "SubIssue {subIssue} has been added to the Issue with id: {IssueId}",
            subIssueId.Id, issueResult.Value.Id.Id);

        return subIssueId.Id;
    }
}