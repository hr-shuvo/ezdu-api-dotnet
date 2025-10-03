using Core.App.Services;
using Core.App.Utils;
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
                x.Name.ToLower().Contains(search) || (x.Groups != null && x.Groups.ToLower().Contains(search)));
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

        var result = await _classRepository.ExecuteListAsync(query, @params.PageNumber, @params.PageSize);

        return new PagedList<Class>(result.Items, result.Count, @params.PageNumber, @params.PageSize);
    }

    public async Task<ApiResponse> SaveAsync(ClassDto classDto)
    {
        bool duplicateName;

        if (classDto.Id > 0)
        {
            var existingEntity = await _classRepository.GetByIdAsync(classDto.Id);

            if (existingEntity is null)
                throw new AppException(404, "Class not found");

            if (existingEntity.Name != classDto.Name)
            {
                duplicateName = await _classRepository.ExistsAsync(x => x.Name == classDto.Name);

                if (duplicateName)
                    throw new AppException(400, "A class with this Name already exists");
            }

            MapDtoToEntity(classDto, existingEntity);

            await _classRepository.UpdateAsync(existingEntity);
            await _classRepository.SaveChangesAsync();

            return new ApiResponse(200, "Class updated successfully");
        }

        duplicateName = await _classRepository.ExistsAsync(x => x.Name == classDto.Name);

        if (duplicateName)
            throw new AppException(400, "A class with this Name already exists");

        var newEntity = MapDtoToEntity(classDto);

        await _classRepository.AddAsync(newEntity);
        await _classRepository.SaveChangesAsync();

        return new ApiResponse(200, "Class added successfully");
    }


    #region Private Methods

    private static Class MapDtoToEntity(ClassDto classDto, Class entity = null)
    {
        entity ??= new Class { CreatedBy = UserContext.UserId };

        entity.Id = classDto.Id;
        entity.Name = classDto.Name;
        entity.Segment = (Segment)classDto.Segment;
        entity.Groups = string.Join(",",
            (classDto.Groups ?? []).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()));

        entity.HasBatch = classDto.HasBatch;
        entity.Status = classDto.Status;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = UserContext.UserId;

        return entity;
    }

    private static ClassDto MapEntityToDto(Class classEntity)
    {
        return new ClassDto
        {
            Id = classEntity.Id,
            Name = classEntity.Name,
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