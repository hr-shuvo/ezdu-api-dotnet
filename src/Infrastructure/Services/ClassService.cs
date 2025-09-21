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

public class ClassService : BaseService<Class>, IClassService
{
    private readonly IClassRepository _classRepository;

    public ClassService(IClassRepository repository) : base(repository)
    {
        _classRepository = repository;
    }

    public async Task<PagedList<Class>> LoadAsync(ClassParams @params)
    {
        // var result = await _classRepository.LoadAsync(query);

        var query = _classRepository.Query(@params.WithDeleted);

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

        var result = await _classRepository.ExecuteListAsync(query, @params.PageNumber, @params.PageSize);

        return new PagedList<Class>(result.Items, result.Count, @params.PageNumber, @params.PageSize);
    }

    public async Task<ApiResponse> SaveAsync(ClassDto classDto)
    {
        bool duplicateTitle;

        if (classDto.Id > 0)
        {
            var existingEntity = await _classRepository.GetByIdAsync(classDto.Id);

            if (existingEntity is null)
                throw new AppException(404, "Class not found");

            if (existingEntity.Title != classDto.Title)
            {
                duplicateTitle = await _classRepository.ExistsAsync(x => x.Title == classDto.Title);

                if (duplicateTitle)
                    throw new AppException(400, "A class with this title already exists");
            }

            MapDtoToEntity(classDto, existingEntity);

            await _classRepository.UpdateAsync(existingEntity);
            await _classRepository.SaveChangesAsync();

            return new ApiResponse(200, "Class updated successfully");
        }

        duplicateTitle = await _classRepository.ExistsAsync(x => x.Title == classDto.Title);

        if (duplicateTitle)
            throw new AppException(400, "A class with this title already exists");

        var newEntity = MapDtoToEntity(classDto);

        await _classRepository.AddAsync(newEntity);
        await _classRepository.SaveChangesAsync();

        return new ApiResponse(200, "Class added successfully");
    }


    #region Private Methods

    private static Class MapDtoToEntity(ClassDto classDto, Class entity = null)
    {
        entity ??= new Class();

        entity.Id = classDto.Id;
        entity.Title = classDto.Title;
        entity.Segment = (Segment)classDto.Segment;
        entity.Groups = string.Join(",",
            (classDto.Groups ?? []).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()));

        entity.HasBatch = classDto.HasBatch;
        entity.Status = classDto.Status;
        entity.UpdatedAt = DateTime.UtcNow;

        return entity;
    }

    private static ClassDto MapEntityToDto(Class classEntity)
    {
        return new ClassDto
        {
            Id = classEntity.Id,
            Title = classEntity.Title,
            Segment = (int)classEntity.Segment,
            Groups = classEntity.Groups?.Split(',').ToList() ?? [],
            HasBatch = classEntity.HasBatch,
            Status = classEntity.Status,
            CreatedAt = classEntity.CreatedAt,
            UpdatedAt = classEntity.UpdatedAt
        };
    }

    #endregion
}