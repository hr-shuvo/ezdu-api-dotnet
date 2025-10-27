using Core.Enums;
using Core.Shared.Models.Pagination;

namespace Core.QueryParams;

public class ClassParams : PaginationParams
{
    public Segment? Segment { get; set; }
}