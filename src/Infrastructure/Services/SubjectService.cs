using Core.App.Services;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Infrastructure.Services;

public class SubjectService : BaseService<Subject>, ISubjectService
{
    public SubjectService(ISubjectRepository repository) : base(repository)
    {
    }
}