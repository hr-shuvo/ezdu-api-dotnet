using Core.Shared.Models.Pagination;

namespace Core.QueryParams;

public class UserParams : PaginationParams
{
    public string CurrentUsername { get; set; }
    public override string OrderBy { get; set; } = "lastActive";
}