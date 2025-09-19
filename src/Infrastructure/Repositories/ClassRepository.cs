using Core.Entities;
using Core.Repositories;
using Infrastructure.Core.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class ClassRepository : BaseRepository<Class>, IClassRepository
{
    public ClassRepository(AppDbContext context) : base(context)
    {
    }
}