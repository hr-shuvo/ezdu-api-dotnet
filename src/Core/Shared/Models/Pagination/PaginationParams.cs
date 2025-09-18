namespace Core.Shared.Models.Pagination;

public class PaginationParams
{
    private const int MaxPageSize = 50;
    public int PageNumber { get; set; } = 1;

    private int _pageSize = 10;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    public string Search { get; set; }
    public virtual string OrderBy { get; set; }

    private string _sortOrder = "asc";

    public string SortBy
    {
        get => _sortOrder;
        set => _sortOrder = (value?.ToLower() == "desc" ? "desc" : "asc");
    }

    public bool? IsDeleted { get; set; }
}