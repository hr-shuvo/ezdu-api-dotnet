using Core.Shared.Models.Pagination;

namespace Core.QueryParams;

public class ExamArchiveParams : PaginationParams
{
    public long ClassId { get; set; }
    public long SubjectId { get; set; }

    public long InstituteId { get; set; }
    public int Year { get; set; }
}