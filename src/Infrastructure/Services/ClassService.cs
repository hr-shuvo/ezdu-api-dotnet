using Core.App.Services;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Infrastructure.Services;

public class ClassService : BaseService<Class>, IClassService
{
    public ClassService(IClassRepository repository) : base(repository)
    {
    }
}