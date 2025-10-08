using Core.Shared.Models.Pagination;

namespace Core.QueryParams;

public class TopicContentParams : PaginationParams
{
    public long TopicId { get; set; }
}