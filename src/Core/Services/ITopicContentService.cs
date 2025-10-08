using Core.App.Services.Interfaces;
using Core.DTOs;
using Core.Entities;
using Core.Errors;
using Core.QueryParams;
using Core.Shared.Models.Pagination;

namespace Core.Services;

public interface ITopicContentService : IBaseService<TopicContent>
{
    Task<PagedList<TopicContent>> LoadAsync(TopicContentParams query);
    Task<TopicContent> GetByIdAsync(long id, bool asTracking = false, bool withDeleted = false);
    Task<ApiResponse> SaveAsync(TopicContentDto dto);
}