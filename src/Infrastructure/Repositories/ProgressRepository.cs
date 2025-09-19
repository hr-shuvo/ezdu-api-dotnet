using Core.Entities;
using Core.Repositories;
using Infrastructure.Core.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class ProgressRepository : BaseRepository<Progress>, IProgressRepository
{
    public ProgressRepository(AppDbContext context) : base(context)
    {
    }
}