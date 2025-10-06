using Core.Shared.Models.Pagination;

namespace Core.QueryParams;

public class TopicParams:PaginationParams
{
    public long SubjectId { get; set; }
    public long LessonId { get; set; }
}