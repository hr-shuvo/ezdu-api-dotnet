using Core.App.Services;
using Core.DTOs;
using Core.Entities;
using Core.Errors;
using Core.QueryParams;
using Core.Repositories;
using Core.Services;
using Core.Shared.Models.Pagination;

namespace Infrastructure.Services;

public class TopicService : BaseService<Topic>, ITopicService
{
    private readonly ITopicRepository _repository;
    private readonly ISubjectService _subjectService;
    private readonly ILessonService _lessonService;
    
    public TopicService(ITopicRepository repository, ISubjectService subjectService, ILessonService lessonService) : base(repository)
    {
        _repository = repository;
        _subjectService = subjectService;
        _lessonService = lessonService;
    }

    public async Task<PagedList<Topic>> LoadAsync(TopicParams @params)
    {
        var query = _repository.Query(@params.WithDeleted);

        if (!string.IsNullOrWhiteSpace(@params.Search))
        {
            var search = @params.Search.Trim().ToLower();
            query = query.Where(x =>
                x.Name.Contains(search, StringComparison.CurrentCultureIgnoreCase));
        }

        if (@params.SubjectId > 0)
        {
            query = query.Where(x => x.SubjectId == @params.SubjectId);
        }

        if (@params.LessonId > 0)
        {
            query = query.Where(x => x.LessonId == @params.LessonId);
        }


        if (@params.OrderBy != null)
        {
            query = @params.OrderBy.ToLower() switch
            {
                "name" => @params.SortBy == "desc"
                    ? query.OrderByDescending(x => x.Name)
                    : query.OrderBy(x => x.Name),
                "createdat" => @params.SortBy == "desc"
                    ? query.OrderByDescending(x => x.CreatedAt)
                    : query.OrderBy(x => x.CreatedAt),
                "updatedat" => @params.SortBy == "desc"
                    ? query.OrderByDescending(x => x.UpdatedAt)
                    : query.OrderBy(x => x.UpdatedAt),
                _ => query.OrderByDescending(x => x.Id)
            };
        }
        else
        {
            query = @params.SortBy == "desc"
                ? query.OrderByDescending(x => x.CreatedAt)
                : query.OrderBy(x => x.CreatedAt);
        }

        var result = await _repository.ExecuteListAsync(query, @params.PageNumber, @params.PageSize);

        return new PagedList<Topic>(result.Items, result.Count, @params.PageNumber, @params.PageSize);
    }

    public async Task<Topic> GetByIdAsync(long id, bool asTracking = false, bool withDeleted = false)
    {
        var entity = await _repository.GetByIdAsync(id, false, withDeleted);
        var subject = await _subjectService.GetByIdAsync(entity.SubjectId, false, withDeleted);
        var lesson = await _lessonService.GetByIdAsync(entity.LessonId, false, withDeleted);

        entity.Subject = subject;
        entity.Lesson = lesson;

        return entity;
    }

    public async Task<ApiResponse> SaveAsync(TopicDto dto)
    {
        if (dto.SubjectId > 0 && !await _subjectService.ExistsAsync(dto.SubjectId.Value))
            throw new AppException(404, "Subject not found");
        
        if (dto.LessonId > 0 && !await _lessonService.ExistsAsync(dto.LessonId.Value))
            throw new AppException(404, "Lesson not found");
        
        
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

    private static Topic MapDtoToEntity(TopicDto dto, Topic entity = null)
    {
        entity ??= new Topic();

        entity.Id = dto.Id;
        entity.Name = dto.Name;
        entity.SubTitle = dto.SubTitle;
        entity.Description = dto.Description;

        if (dto.SubjectId != null) entity.SubjectId = dto.SubjectId.Value;
        if (dto.LessonId != null) entity.LessonId = dto.LessonId.Value;

        entity.Status = dto.Status;
        entity.UpdatedAt = DateTime.UtcNow;

        return entity;
    }

    private static SubjectDto MapEntityToDto(Subject subjectEntity)
    {
        return new SubjectDto
        {
            Id = subjectEntity.Id,
            Name = subjectEntity.Name,
            Segment = (int)subjectEntity.Segment,
            Groups = subjectEntity.Groups?.Split(',').ToList() ?? [],
            HasPaper = subjectEntity.HasPaper,
            Status = subjectEntity.Status,
            CreatedAt = subjectEntity.CreatedAt,
            UpdatedAt = subjectEntity.UpdatedAt
        };
    }

    #endregion
}