using Core.Entities;
using Core.Repositories;
using Infrastructure.Core.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class LessonRepository : BaseRepository<Lesson>, ILessonRepository
{
    public LessonRepository(AppDbContext context) : base(context)
    {
    }
}