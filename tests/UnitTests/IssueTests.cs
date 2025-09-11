using TaskManagement.Domain.Aggregate;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.ValueObjects;
using FluentAssertions;
using TaskManagement.Domain.Entities;

namespace UnitTests;

public class IssueTests
{
    [Fact]
    public void Constructor_ShouldInitializeAllProperties_WhenValidParametersProvided()
    {
        // Arrange
        var issueId = IssueId.NewGuid();
        var title = Title.Create("Test Issue").Value;
        var description = Description.Create("Test Description").Value;
        var status = IssueStatus.InProgress;
        var priority = IssuePriority.Medium;
        var author = Name.Create("Alex").Value;

        // Act
        var issue = new Issue(
            issueId, title, description, status, priority, author);

        // Assert
        issue.Id.Should().Be(issueId);
        issue.Title.Should().Be(title);
        issue.Description.Should().Be(description);
        issue.Status.Should().Be(status);
        issue.Priority.Should().Be(priority);
        issue.Author.Should().Be(author);
        issue.Assignee.Should().BeNull();
        issue.SubIssues.Should().BeEmpty();
    }
    
    [Fact]
    public void SetAssignee_ShouldSetAssignee_WhenValidAssigneeProvided()
    {
        // Arrange
        var issue = CreateIssue();
        var assignee = Name.Create("Anna").Value;

        // Act
        issue.SetAssignee(assignee);

        // Assert
        issue.Assignee.Should().Be(assignee);
    }
    
    [Fact]
    public void UpdateTitle_ShouldUpdateTitle_WhenValidTitleProvided()
    {
        // Arrange
        var issue = CreateIssue();
        var newTitle = Title.Create("Updated Title").Value;

        // Act
        issue.UpdateTitle(newTitle);

        // Assert
        issue.Title.Should().Be(newTitle);
    }

    [Fact]
    public void UpdateDescription_ShouldUpdateDescription_WhenValidDescriptionProvided()
    {
        // Arrange
        var issue = CreateIssue();
        var newDescription = Description.Create("Updated Description").Value;

        // Act
        issue.UpdateDescription(newDescription);

        // Assert
        issue.Description.Should().Be(newDescription);
    }
    
    [Fact]
    public void UpdateStatus_ShouldUpdateStatus_WhenValidStatusProvided()
    {
        // Arrange
        var issue = CreateIssue();
        var newStatus = IssueStatus.InProgress;

        // Act
        issue.UpdateStatus(newStatus);

        // Assert
        issue.Status.Should().Be(newStatus);
    }
    
    [Fact]
    public void AddSubIssue_ShouldAddToCollection_WhenValidSubIssue()
    {
        // Arrange
        var issue = CreateIssue();
        var subIssue = CreateSubIssue();

        // Act
        issue.AddSubIssue(subIssue);

        // Assert
        issue.SubIssues.Should().Contain(subIssue);
        issue.SubIssues.Should().HaveCount(1);
    }
    
    [Fact]
    public void RemoveSubIssue_ShouldRemoveFromCollection_WhenSubIssueExists()
    {
        // Arrange
        var issue = CreateIssue();
        var subIssue = CreateSubIssue();
        issue.AddSubIssue(subIssue);

        // Act
        issue.RemoveSubIssue(subIssue);

        // Assert
        issue.SubIssues.Should().NotContain(subIssue);
        issue.SubIssues.Should().BeEmpty();
    }
    
    [Fact]
    public void SetSubIssueAssignee_ShouldReturnSuccess_WhenSubIssueExists()
    {
        // Arrange
        var issue = CreateIssue();
        var subIssue = CreateSubIssue();
        issue.AddSubIssue(subIssue);
        var assignee = Name.Create("Jane").Value;

        // Act
        var result = issue.SetSubIssueAssignee(subIssue.Id, assignee);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
    
    [Fact]
    public void UpdateSubIssueDescription_ShouldReturnNotFoundError_WhenSubIssueDoesNotExist()
    {
        // Arrange
        var issue = CreateIssue();
        var nonExistentId = SubIssueId.NewGuid();
        var newDescription = Description.Create("New Description").Value;

        // Act
        var result = issue.UpdateSubIssueDescription(nonExistentId, newDescription);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("value.not.found");
    }
    
    private static Issue CreateIssue(
        string? title = null,
        string? description = null,
        IssueStatus? status = null,
        IssuePriority? priority = null,
        string? author = null)
    {
        return new Issue(
            IssueId.NewGuid(),
            title is null ? Title.Create("Default Issue Title").Value :
                Title.Create(title).Value,
            description is null ? Description.Create("Default Issue Description").Value :
                Description.Create(description).Value,
            status ?? IssueStatus.InProgress,
            priority ?? IssuePriority.Medium,
            author is null ? Name.Create("Default Issue Author").Value :
                Name.Create(author).Value
        );
    }
    
    private static SubIssue CreateSubIssue(
        string? title = null,
        string? description = null,
        IssueStatus? status = null,
        IssuePriority? priority = null,
        string? author = null)
    {
        return new SubIssue(
            SubIssueId.NewGuid(),
            title is null ? Title.Create("Default SubIssue Title").Value :
                Title.Create(title).Value,
            description is null ? Description.Create("Default SubIssue Description").Value :
                Description.Create(description).Value,
            status ?? IssueStatus.InProgress,
            priority ?? IssuePriority.Medium,
            author is null ? Name.Create("Default Author").Value :
                Name.Create(author).Value
        );
    }
}