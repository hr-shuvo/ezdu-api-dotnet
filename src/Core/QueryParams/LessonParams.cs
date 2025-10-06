using Core.Shared.Models.Pagination;

namespace Core.QueryParams;

public class LessonParams : PaginationParams
{
    public long SubjectId { get; set; }
}