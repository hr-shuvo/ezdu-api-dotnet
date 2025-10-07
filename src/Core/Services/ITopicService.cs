using Core.App.Services.Interfaces;
using Core.DTOs;
using Core.Entities;
using Core.Errors;
using Core.QueryParams;
using Core.Shared.Models.Pagination;

namespace Core.Services;

public interface ITopicService : IBaseService<Topic>
{
    Task<PagedList<Topic>> LoadAsync(TopicParams query);
    Task<Topic> GetByIdAsync(long id, bool asTracking = false, bool withDeleted = false);
    Task<ApiResponse> SaveAsync(TopicDto dto);
}