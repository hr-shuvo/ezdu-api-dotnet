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
        builder.Property(x => x.ClassId).IsRequired();

        builder.Property(x => x.Groups).HasMaxLength(200);
    }
}

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(250);
        builder.Property(x => x.SubTitle).HasMaxLength(250);
        builder.Property(x => x.Content).HasMaxLength(1000);
        builder.Property(x => x.VideoUrl).HasMaxLength(500);
        builder.Property(x => x.ResourceUrl).HasMaxLength(500);

        builder.HasOne(x => x.Subject)
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
        builder.Property(x => x.SubTitle).HasMaxLength(250);
        builder.Property(x => x.Description).HasMaxLength(1000);

        builder.HasOne(x => x.Subject)
            .WithMany()
            .HasForeignKey(x => x.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Lesson)
            .WithMany()
            .HasForeignKey(x => x.LessonId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class TopicContentConfiguration : IEntityTypeConfiguration<TopicContent>
{
    public void Configure(EntityTypeBuilder<TopicContent> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(250);

        builder.Property(x => x.Content).HasMaxLength(2000);

        builder.Property(x => x.Url).HasMaxLength(500);
        builder.Property(x => x.Type).IsRequired();


        builder.HasOne(x => x.Topic)
            .WithMany()
            .HasForeignKey(x => x.TopicId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.SubjectId);
        builder.HasIndex(x => x.LessonId);
        builder.HasIndex(x => x.TopicId);
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
        builder.Property(x => x.QuestionType).IsRequired();

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

        // builder.Property(x => x.TopicId).IsRequired(false);

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

        builder.HasMany(x => x.Options)
            .WithOne(x => x.Question)
            .HasForeignKey(x => x.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Answers)
            .WithOne(x => x.Question)
            .HasForeignKey(x => x.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.References)
            .WithOne()
            .HasForeignKey(x => x.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.SubjectId);
        builder.HasIndex(x => x.LessonId);
        builder.HasIndex(x => x.TopicId);
    }
}

public class QuizQuestionConfiguration : IEntityTypeConfiguration<QuizQuestion>
{
    public void Configure(EntityTypeBuilder<QuizQuestion> builder)
    {
        builder.HasKey(x => new { x.QuizId, x.QuestionId });
        
        builder.HasIndex(x => x.QuizId);
    }
}

public class OptionConfiguration : IEntityTypeConfiguration<Option>
{
    public void Configure(EntityTypeBuilder<Option> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(500);

        builder.HasOne<Question>()
            .WithMany(x => x.Options)
            .HasForeignKey(x => x.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.QuestionId);
    }
}

public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(1000);

        builder.HasOne<Question>()
            .WithMany(x => x.Answers)
            .HasForeignKey(x => x.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.QuestionId);
    }
}

public class QuestionReferenceConfiguration : IEntityTypeConfiguration<QuestionReference>
{
    public void Configure(EntityTypeBuilder<QuestionReference> builder)
    {
        builder.Ignore(x => x.Id);
        builder.Ignore(x => x.Name);

        builder.HasKey(x => new { x.QuestionId, x.InstituteId });

        builder.HasOne<Question>()
            .WithMany(x => x.References)
            .HasForeignKey(x => x.QuestionId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Institute>()
            .WithMany(x => x.QuestionReferences)
            .HasForeignKey(x => x.InstituteId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.QuestionId);
        builder.HasIndex(x => x.InstituteId);
    }
}

public class InstituteConfiguration : IEntityTypeConfiguration<Institute>
{
    public void Configure(EntityTypeBuilder<Institute> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(250);
        builder.Property(x => x.Code).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Address).HasMaxLength(500);
        builder.Property(x => x.WebsiteUrl).HasMaxLength(500);
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

public class ProgressConfiguration : IEntityTypeConfiguration<Progress>
{
    public void Configure(EntityTypeBuilder<Progress> builder)
    {
        builder.HasKey(x => x.Id);
    }
}

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasKey(x => x.UserId);
        builder.Property(x => x.Id).ValueGeneratedNever();
        
        builder.Property(x => x.ClassName).IsRequired().HasMaxLength(250);
    }
}