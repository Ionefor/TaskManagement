﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domain;
using TaskManagement.Domain.Aggregate;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Infrastructure.Configurations.Write;

public class IssueConfiguration : IEntityTypeConfiguration<Issue>
{
    public void Configure(EntityTypeBuilder<Issue> builder)
    {
        builder.ToTable("issues");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id).HasConversion(
            id => id.Id,
            value => IssueId.Create(value));
        
        builder.ComplexProperty(i => i.Title,
            t =>
            {
                t.Property(v => v.Value).
                    IsRequired().
                    HasColumnName("title");
            });
        
        builder.ComplexProperty(i => i.Description,
            d =>
            {
                d.Property(v => v.Value).
                    IsRequired().
                    HasColumnName("description");
            });
        
        builder.ComplexProperty(i => i.Author,
            t =>
            {
                t.Property(v => v.Value).
                    IsRequired().
                    HasColumnName("author");
            });
        
        builder.ComplexProperty(i => i.Assignee,
            t =>
            {
                t.Property(v => v.Value).
                    IsRequired().
                    HasColumnName("assignee");
            });
        
        builder.Property(i => i.Status)
            .HasConversion<string>(); 
        
        builder.Property(i => i.Priority)
            .HasConversion<string>(); 
        
        builder.HasMany(i => i.SubIssues).
            WithOne().
            HasForeignKey("issue_id").
            OnDelete(DeleteBehavior.Cascade).
            IsRequired();
    }
}