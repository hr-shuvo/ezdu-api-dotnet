using Core.App.Services;
using Core.DTOs;
using Core.Entities;
using Core.Enums;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Infrastructure.Services;

public class ClassService : BaseService<Class>, IClassService
{
    private readonly IClassRepository _classRepository;

    public ClassService(IClassRepository repository) : base(repository)
    {
        _classRepository = repository;
    }

    public async Task<ApiResponse> SaveAsync(ClassDto classDto)
    {
        if (classDto.Id > 0)
        {
            var existingEntity = await _classRepository.GetByIdAsync(classDto.Id);

            if (existingEntity is null)
                throw new AppException(404, "Class not found");
            
            MapDtoToEntity(classDto, existingEntity);

            await _classRepository.UpdateAsync(existingEntity);
            return new ApiResponse(200, "Class updated successfully");
        }

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
        entity.CreatedAt = classDto.CreatedAt ?? DateTime.UtcNow;
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