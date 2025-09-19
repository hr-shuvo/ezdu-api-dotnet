using Core.App.Services;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Infrastructure.Services;

public class ProgressService : BaseService<Progress>, IProgressService
{
    public ProgressService(IProgressRepository repository) : base(repository)
    {
    }
}