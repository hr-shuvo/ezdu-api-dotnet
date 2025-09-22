using Core.App.Services;
using Core.App.Services.Interfaces;
using Core.DTOs;
using Core.Entities;
using Core.Enums;
using Core.Errors;
using Core.QueryParams;
using Core.Repositories;
using Core.Services;
using Core.Shared.Models.Pagination;

namespace Infrastructure.Services;

public class SubjectService : BaseService<Subject>, ISubjectService
{
    private readonly ISubjectRepository _repository;
    
    public SubjectService(ISubjectRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<PagedList<Subject>> LoadAsync(SubjectParams @params)
    {
        // var result = await _subjectRepository.LoadAsync(query);

        var query = _repository.Query(@params.WithDeleted);

        // TODO: Add more filters as needed
        if (!string.IsNullOrWhiteSpace(@params.Search))
        {
            var search = @params.Search.Trim().ToLower();
            query = query.Where(x =>
                x.Title.ToLower().Contains(search) || (x.Groups != null && x.Groups.ToLower().Contains(search)));
        }


        if (@params.OrderBy != null)
        {
            query = @params.SortBy.ToLower() switch
            {
                "title" => @params.OrderBy == "desc"
                    ? query.OrderByDescending(x => x.Title)
                    : query.OrderBy(x => x.Title),
                "createdat" => @params.OrderBy == "desc"
                    ? query.OrderByDescending(x => x.CreatedAt)
                    : query.OrderBy(x => x.CreatedAt),
                "updatedat" => @params.OrderBy == "desc"
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

        return new PagedList<Subject>(result.Items, result.Count, @params.PageNumber, @params.PageSize);
    }

    public async Task<ApiResponse> SaveAsync(SubjectDto dto)
    {
        bool duplicateTitle;

        if (dto.Id > 0)
        {
            var existingEntity = await _repository.GetByIdAsync(dto.Id);

            if (existingEntity is null)
                throw new AppException(404, "Subject not found");

            if (existingEntity.Title != dto.Title)
            {
                duplicateTitle = await _repository.ExistsAsync(x => x.Title == dto.Title);

                if (duplicateTitle)
                    throw new AppException(400, "A Subject with this title already exists");
            }

            MapDtoToEntity(dto, existingEntity);

            await _repository.UpdateAsync(existingEntity);
            await _repository.SaveChangesAsync();

            return new ApiResponse(200, "Subject updated successfully");
        }

        duplicateTitle = await _repository.ExistsAsync(x => x.Title == dto.Title);

        if (duplicateTitle)
            throw new AppException(400, "A Subject with this title already exists");

        var newEntity = MapDtoToEntity(dto);

        await _repository.AddAsync(newEntity);
        await _repository.SaveChangesAsync();

        return new ApiResponse(200, "Subject added successfully");
    }
    
    
    
    
    
    
    
    
    
    #region Private Methods

    private static Subject MapDtoToEntity(SubjectDto dto, Subject entity = null)
    {
        entity ??= new Subject();

        entity.Id = dto.Id;
        entity.Title = dto.Title;
        entity.Segment = (Segment)dto.Segment;
        entity.Groups = string.Join(",",
            (dto.Groups ?? []).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()));

        entity.HasPaper = dto.HasPaper;
        entity.Status = dto.Status;
        entity.UpdatedAt = DateTime.UtcNow;

        return entity;
    }

    private static SubjectDto MapEntityToDto(Subject subjectEntity)
    {
        return new SubjectDto
        {
            Id = subjectEntity.Id,
            Title = subjectEntity.Title,
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