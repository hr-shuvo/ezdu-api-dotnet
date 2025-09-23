using Core.App.Services;
using Core.DTOs;
using Core.Entities;
using Core.Enums;
using Core.Errors;
using Core.QueryParams;
using Core.Repositories;
using Core.Services;
using Core.Shared.Models.Pagination;

namespace Infrastructure.Services;

public class TopicContentService : BaseService<TopicContent>, ITopicContentService
{
    private readonly ITopicContentRepository _repository;
    private readonly ITopicService _topicService;
    
    public TopicContentService(ITopicContentRepository repository, ITopicService topicService) : base(repository)
    {
        _repository = repository;
        _topicService = topicService;
    }

    public async Task<PagedList<TopicContent>> LoadAsync(TopicContentParams @params)
    {
        var query = _repository.Query(@params.WithDeleted);

        // TODO: Add more filters as needed
        if (!string.IsNullOrWhiteSpace(@params.Search))
        {
            var search = @params.Search.Trim().ToLower();
            query = query.Where(x =>
                x.Name.Contains(search, StringComparison.CurrentCultureIgnoreCase));
        }


        if (@params.OrderBy != null)
        {
            query = @params.SortBy.ToLower() switch
            {
                "name" => @params.OrderBy == "desc"
                    ? query.OrderByDescending(x => x.Name)
                    : query.OrderBy(x => x.Name),
                "createdat" => @params.OrderBy == "desc"
                    ? query.OrderByDescending(x => x.CreatedAt)
                    : query.OrderBy(x => x.CreatedAt),
                "updatedat" => @params.OrderBy == "desc"
                    ? query.OrderByDescending(x => x.UpdatedAt)
                    : query.OrderBy(x => x.UpdatedAt),
                _ => query.OrderByDescending(x => x.CreatedAt)
            };
        }
        else
        {
            query = @params.SortBy == "desc"
                ? query.OrderByDescending(x => x.CreatedAt)
                : query.OrderBy(x => x.CreatedAt);
        }

        var result = await _repository.ExecuteListAsync(query, @params.PageNumber, @params.PageSize);

        return new PagedList<TopicContent>(result.Items, result.Count, @params.PageNumber, @params.PageSize);
    }

    public async Task<ApiResponse> SaveAsync(TopicContentDto dto)
    {
        if (dto.TopicId > 0 && !await _topicService.ExistsAsync(dto.TopicId.Value))
            throw new AppException(404, "Topic not found");
        
        
        bool duplicateTitle;

        if (dto.Id > 0)
        {
            var existingEntity = await _repository.GetByIdAsync(dto.Id);

            if (existingEntity is null)
                throw new AppException(404, "Topic not found");

            if (existingEntity.Name != dto.Name)
            {
                duplicateTitle = await _repository.ExistsAsync(x => x.Name == dto.Name);

                if (duplicateTitle)
                    throw new AppException(400, "A Topic with this title already exists");
            }

            MapDtoToEntity(dto, existingEntity);

            await _repository.UpdateAsync(existingEntity);
            await _repository.SaveChangesAsync();

            return new ApiResponse(200, "Topic updated successfully");
        }

        duplicateTitle = await _repository.ExistsAsync(x => x.Name == dto.Name);

        if (duplicateTitle)
            throw new AppException(400, "A Topic with this title already exists");

        var newEntity = MapDtoToEntity(dto);

        await _repository.AddAsync(newEntity);
        await _repository.SaveChangesAsync();

        return new ApiResponse(200, "Topic added successfully");
    }
    
    
    
    
    
    
    #region Private Methods

    private static TopicContent MapDtoToEntity(TopicContentDto dto, TopicContent entity = null)
    {
        entity ??= new TopicContent();

        entity.Id = dto.Id;
        entity.Name = dto.Name;

        entity.TopicId = dto.TopicId.Value;
        entity.UpdatedAt = DateTime.UtcNow;
        
        entity.Type = (ContentType)dto.Type;
        entity.Content = dto.Content;
        entity.Order = dto.Order;
        entity.Status = dto.Status;

        return entity;
    }

    private static SubjectDto MapEntityToDto(TopicContent entity)
    {
        return new SubjectDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Status = entity.Status,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }

    #endregion
}