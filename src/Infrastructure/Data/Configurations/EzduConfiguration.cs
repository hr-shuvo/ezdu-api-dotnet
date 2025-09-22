using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Data.Configurations;

public class ClassConfiguration : IEntityTypeConfiguration<Class>
{
    public void Configure(EntityTypeBuilder<Class> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        
        builder.Property(x => x.Segment).IsRequired();
        builder.Property(x => x.Groups).HasMaxLength(200);
    }
}

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).IsRequired().HasMaxLength(250);
        builder.Property(x => x.SubTitle).HasMaxLength(250);
        builder.Property(x => x.Code).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Groups).IsRequired().HasMaxLength(250);
    }
}

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).IsRequired().HasMaxLength(250);
        builder.Property(x => x.SubTitle).HasMaxLength(250);
        builder.Property(x => x.Content).IsRequired().HasMaxLength(1000);
        builder.Property(x => x.VideoUrl).IsRequired().HasMaxLength(500);
        builder.Property(x => x.ResourceUrl).HasMaxLength(500);
        
        builder.HasOne<Subject>()
            .WithMany()
            .HasForeignKey(x => x.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).IsRequired().HasMaxLength(250);
        builder.Property(x => x.Description).HasMaxLength(1000);
        
        builder.HasOne<Subject>()
            .WithMany()
            .HasForeignKey(x => x.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne<Lesson>()
            .WithMany()
            .HasForeignKey(x => x.LessonId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ContentConfiguration : IEntityTypeConfiguration<TopicContent>
{
    public void Configure(EntityTypeBuilder<TopicContent> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).IsRequired().HasMaxLength(250);
        
        builder.Property(x => x.Content).HasMaxLength(2000);
        
        builder.Property(x => x.Url).HasMaxLength(500);
        builder.Property(x => x.Type).IsRequired();
        
        
        builder.HasOne<Topic>()
            .WithMany()
            .HasForeignKey(x => x.TopicId)
            .OnDelete(DeleteBehavior.Restrict);
        
        
        
        
    }
}

public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).IsRequired().HasMaxLength(250);
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.TotalMarks).IsRequired();
    }
}

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).IsRequired().HasMaxLength(1000);
        builder.Property(x => x.Options).IsRequired().HasMaxLength(2000);
        builder.Property(x => x.CorrectAnswer).IsRequired().HasMaxLength(500);
        
        builder.Property(x => x.Passage).HasMaxLength(1000);
        builder.Property(x => x.ImageUrl).HasMaxLength(500);
        builder.Property(x => x.ImagePublicId).HasMaxLength(200);
        
        builder.Property(x => x.Explanation).HasMaxLength(2000);
        builder.Property(x => x.ExplanationImageUrl).HasMaxLength(500);
        builder.Property(x => x.ExplanationImagePublicId).HasMaxLength(200);
        builder.Property(x => x.ExplanationVideoUrl).HasMaxLength(500);
        builder.Property(x => x.ExplanationResourceUrl).HasMaxLength(500);
        
        builder.Property(x => x.Hint).HasMaxLength(500);
        builder.Property(x => x.DifficultyLevel).HasMaxLength(100);
        builder.Property(x => x.QuestionType).HasMaxLength(100);
        builder.Property(x => x.Tags).HasMaxLength(250);
        
        builder.HasOne<Subject>()
            .WithMany()
            .HasForeignKey(x => x.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne<Lesson>()
            .WithMany()
            .HasForeignKey(x => x.LessonId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne<Topic>()
            .WithMany()
            .HasForeignKey(x => x.TopicId)
            .OnDelete(DeleteBehavior.Restrict);
        
    }
}

public class AchievementConfiguration : IEntityTypeConfiguration<Achievement>
{
    public void Configure(EntityTypeBuilder<Achievement> builder)
    {
        builder.HasKey(x => x.Id);
        
        // todo: add other properties
        // builder.Property(x => x.Name).IsRequired().HasMaxLength(250);
        // builder.Property(x => x.Description).HasMaxLength(1000);
        // builder.Property(x => x.IconUrl).HasMaxLength(500);
        // builder.Property(x => x.Criteria).HasMaxLength(1000);
    }
}

public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
{
    public void Configure(EntityTypeBuilder<Assignment> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).IsRequired().HasMaxLength(250);
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.Type).IsRequired();
        
        builder.HasOne<Subject>()
            .WithMany()
            .HasForeignKey(x => x.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne<Lesson>()
            .WithMany()
            .HasForeignKey(x => x.LessonId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}










