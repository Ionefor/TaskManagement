using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Application.Dto;

namespace TaskManagement.Infrastructure.Configurations.Read;

public class SubIssueDtoConfiguration : IEntityTypeConfiguration<SubIssueDto>
{
    public void Configure(EntityTypeBuilder<SubIssueDto> builder)
    {
        builder.ToTable("sub_issues");
        
        builder.HasKey(i => i.Id);
        
        builder.Property(i => i.IssueId);
        
        builder.Property(i => i.Title);
        
        builder.Property(i => i.Description);
        
        builder.Property(i => i.Author);
        
        builder.Property(i => i.Assignee);
        
        builder.Property(i => i.Status);
        
        builder.Property(i => i.Priority);
    }
}