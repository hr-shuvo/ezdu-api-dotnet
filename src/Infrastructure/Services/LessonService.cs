using Core.App.Services;
using Core.DTOs;
using Core.Entities;
using Core.Errors;
using Core.QueryParams;
using Core.Repositories;
using Core.Services;
using Core.Shared.Models.Pagination;

namespace Infrastructure.Services;

public class LessonService : BaseService<Lesson>, ILessonService
{
    private readonly ILessonRepository _repository;
    private readonly ISubjectService _subjectService;
    
    public LessonService(ILessonRepository repository, ISubjectService subjectService) : base(repository)
    {
        _repository = repository;
        _subjectService = subjectService;
    }

    public async Task<PagedList<Lesson>> LoadAsync(LessonParams @params)
    {
        var query = _repository.Query(@params.WithDeleted);

        // TODO: Add more filters as needed
        if (!string.IsNullOrWhiteSpace(@params.Search))
        {
            var search = @params.Search.Trim().ToLower();
            query = query.Where(x =>
                x.Name.ToLower().Contains(search));
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

        return new PagedList<Lesson>(result.Items, result.Count, @params.PageNumber, @params.PageSize);
    }

    public async Task<ApiResponse> SaveAsync(LessonDto dto)
    {
        if (dto.SubjectId > 0)
        {
            if (!await _subjectService.ExistsAsync(dto.SubjectId))
                throw new AppException(404, "Subject not found");
        }
        else
        {
            throw new AppException(400, "Subject is required");
        }
        
        bool duplicateTitle;

        if (dto.Id > 0)
        {
            var existingEntity = await _repository.GetByIdAsync(dto.Id);

            if (existingEntity is null)
                throw new AppException(404, "Subject not found");

            if (existingEntity.Name != dto.Name)
            {
                duplicateTitle = await _repository.ExistsAsync(x => x.Name == dto.Name);

                if (duplicateTitle)
                    throw new AppException(400, "A Subject with this title already exists");
            }

            MapDtoToEntity(dto, existingEntity);

            await _repository.UpdateAsync(existingEntity);
            await _repository.SaveChangesAsync();

            return new ApiResponse(200, "Subject updated successfully");
        }

        duplicateTitle = await _repository.ExistsAsync(x => x.Name == dto.Name);

        if (duplicateTitle)
            throw new AppException(400, "A Subject with this title already exists");

        var newEntity = MapDtoToEntity(dto);

        await _repository.AddAsync(newEntity);
        await _repository.SaveChangesAsync();

        return new ApiResponse(200, "Subject added successfully");
    }
    
    
    
    
    
    
    
    
    
    #region Private Methods

    private static Lesson MapDtoToEntity(LessonDto dto, Lesson entity = null)
    {
        entity ??= new Lesson();

        entity.Id = dto.Id;
        entity.Name = dto.Name;
        
        entity.SubTitle = dto.SubTitle;
        entity.Content = dto.Content;
        entity.SubjectId = dto.SubjectId;
        
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