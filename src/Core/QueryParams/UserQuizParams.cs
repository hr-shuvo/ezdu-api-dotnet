using Core.Shared.Models.Pagination;

namespace Core.QueryParams;

public class UserQuizParams : PaginationParams
{
    public long UserId { get; set; }
}