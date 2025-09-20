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

    public async Task<PagedList<Class>> LoadAsync(ClassParams query)
    {
        var result = await _classRepository.LoadAsync(query);
        
        return new PagedList<Class>(result.Items, result.Count, query.PageNumber, query.PageSize);
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
            return new ApiResponse(200, "Class updated successfully");
        }

        duplicateTitle = await _classRepository.ExistsAsync(x => x.Title == classDto.Title);

        if (duplicateTitle)
            throw new AppException(400, "A class with this title already exists");

        var newEntity = MapDtoToEntity(classDto);
        await _classRepository.AddAsync(newEntity);

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