using Core.Repositories;
using Core.Services;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class AppDependencyExtensions
{
    public static void AddAppDependencyInjections(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IAchievementService, AchievementService>();
        services.AddScoped<IAssignmentService, AssignmentService>();
        services.AddScoped<IClassService, ClassService>();
        services.AddScoped<ILessonService, LessonService>();
        services.AddScoped<IProgressService, ProgressService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<ISubjectService, SubjectService>();
        services.AddScoped<ITopicContentService, TopicContentService>();
        services.AddScoped<ITopicService, TopicService>();
        services.AddScoped<IQuizService, QuizService>();
        services.AddScoped<IExamArchiveService, ExamArchiveService>();
        
        
        
        // Repositories
        services.AddScoped<IAchievementRepository, AchievementRepository>();
        services.AddScoped<IAssignmentRepository, AssignmentRepository>();
        services.AddScoped<IClassRepository, ClassRepository>();
        services.AddScoped<ILessonRepository, LessonRepository>();
        services.AddScoped<IProgressRepository, ProgressRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<ISubjectRepository, SubjectRepository>();
        services.AddScoped<ITopicContentRepository, TopicContentRepository>();
        services.AddScoped<ITopicRepository, TopicRepository>();
        services.AddScoped<IOptionRepository, OptionRepository>(); // note: no service used

        services.AddScoped<IQuizRepository, QuizRepository>();

    }
}