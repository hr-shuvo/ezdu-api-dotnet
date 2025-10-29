using Core.Shared.Models.Pagination;

namespace Core.QueryParams;

public class QuestionParams : PaginationParams
{
    public long SubjectId { get; set; }
    public long LessonId { get; set; }
    public long TopicId { get; set; }
    public long ExamId { get; set; }
}