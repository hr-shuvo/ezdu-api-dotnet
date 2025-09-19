using Core.App.Services;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Infrastructure.Services;

public class LessonService : BaseService<Lesson>, ILessonService
{
    public LessonService(ILessonRepository repository) : base(repository)
    {
    }
}