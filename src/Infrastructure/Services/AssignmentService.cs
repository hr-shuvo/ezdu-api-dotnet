using Core.App.Services;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Infrastructure.Services;

public class AssignmentService : BaseService<Assignment>, IAssignmentService
{
    public AssignmentService(IAssignmentRepository repository) : base(repository)
    {
    }
}