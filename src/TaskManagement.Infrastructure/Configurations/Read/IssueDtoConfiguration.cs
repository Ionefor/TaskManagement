using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Application.Dto;

namespace TaskManagement.Infrastructure.Configurations.Read;

public class IssueDtoConfiguration : IEntityTypeConfiguration<IssueDto>
{
    public void Configure(EntityTypeBuilder<IssueDto> builder)
    {
        builder.ToTable("issues");
        
        builder.HasKey(i => i.Id);
        
        builder.HasMany(i => i.SubIssues).
            WithOne().
            HasForeignKey(i => i.IssueId);
        
        builder.Property(i => i.Title);
        
        builder.Property(i => i.Description);
        
        builder.Property(i => i.Author);
        
        builder.Property(i => i.Assignee);
        
        builder.Property(i => i.Status);
        
        builder.Property(i => i.Priority);
    }
}