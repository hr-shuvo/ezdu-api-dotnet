using Core.App.Repositories.Interfaces;
using Core.App.Services;
using Core.App.Utils;
using Core.DTOs;
using Core.Entities;
using Core.Errors;
using Core.QueryParams;
using Core.Services;
using Core.Shared.Models.Pagination;

namespace Infrastructure.Services;

public class ExamArchiveService : BaseService<ExamArchive>, IExamArchiveService
{
    private readonly IBaseRepository<ExamArchive> _repository;
    private readonly IQuestionService _questionService;

    public ExamArchiveService(IBaseRepository<ExamArchive> repository, IQuestionService questionService) :
        base(repository)
    {
        _repository = repository;
        _questionService = questionService;
    }

    public async Task<PagedList<ExamArchive>> LoadAsync(ExamArchiveParams @params)
    {
        var query = _repository.Query(@params.WithDeleted);

        if (!string.IsNullOrWhiteSpace(@params.Search))
        {
            var search = @params.Search.Trim().ToLower();
            query = query.Where(x =>
                x.Name.Contains(search, StringComparison.CurrentCultureIgnoreCase));
        }

        if (@params.ClassId > 0)
        {
            query = query.Where(x => x.ClassId == @params.ClassId);
        }

        if (@params.SubjectId > 0)
        {
            query = query.Where(x => x.SubjectId == @params.SubjectId);
        }

        if (@params.InstituteId > 0)
        {
            query = query.Where(x => x.InstituteId == @params.InstituteId);
        }

        if (@params.Year > 0)
        {
            query = query.Where(x => x.Year == @params.Year);
        }


        if (@params.OrderBy != null)
        {
            query = @params.OrderBy.ToLower() switch
            {
                "year" => @params.SortBy == "desc"
                    ? query.OrderByDescending(x => x.Year)
                    : query.OrderBy(x => x.Year),
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
                ? query.OrderByDescending(x => x.Year).ThenByDescending(x => x.SubjectId)
                    .ThenByDescending(x => x.InstituteId)
                : query.OrderBy(x => x.CreatedAt);
        }

        var result = await _repository.ExecuteListAsync(query, @params.PageNumber, @params.PageSize);

        return new PagedList<ExamArchive>(result.Items, result.Count, @params.PageNumber, @params.PageSize);
    }

    public async Task<ExamArchive> GetByIdAsync(long id)
    {
        var query = _repository.Query().Where(x => x.Id == id);

        var entity = await _repository.ExecuteAsync(query);
        if (entity == null) throw new AppException(404, "Exam not found");

        var questions = await _questionService.LoadAsync(new QuestionParams()
        {
            PageSize = 200,
            ExamId = id,
            SortBy = "asc"
        });
        
        entity.Questions = questions.Items;

        return entity;
    }

    public async Task<ApiResponse> SaveAsync(ExamArchiveDto dto)
    {
        bool duplicateName;

        if (dto.Id > 0)
        {
            var existingEntity = await _repository.GetByIdAsync(dto.Id);

            if (existingEntity is null)
                throw new AppException(404, "Exam not found");

            if (existingEntity.Name != dto.Name)
            {
                duplicateName = await _repository.ExistsAsync(x => x.Name == dto.Name);

                if (duplicateName)
                    throw new AppException(400, "A Exam with this Name already exists");
            }

            MapDtoToEntity(dto, existingEntity);

            await _repository.UpdateAsync(existingEntity);
            await _repository.SaveChangesAsync();

            return new ApiResponse(200, "Exam updated successfully");
        }

        duplicateName = await _repository.ExistsAsync(x => x.Name == dto.Name);

        if (duplicateName)
            throw new AppException(400, "A Exam with this Name already exists");

        var newEntity = MapDtoToEntity(dto);

        await _repository.AddAsync(newEntity);
        await _repository.SaveChangesAsync();

        return new ApiResponse(200, "Exam added successfully");
    }


    #region Private Methods

    private static ExamArchive MapDtoToEntity(ExamArchiveDto dto, ExamArchive entity = null)
    {
        entity ??= new ExamArchive { CreatedBy = UserContext.UserId };

        entity.Id = dto.Id;
        entity.Name = dto.Name;
        entity.ClassId = dto.ClassId;
        entity.SubjectId = dto.SubjectId ?? 0;
        entity.InstituteId = dto.InstituteId ?? 0;
        entity.Year = dto.Year ?? 0;

        entity.Status = dto.Status;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = UserContext.UserId;

        return entity;
    }

    private static ExamArchiveDto MapEntityToDto(ExamArchive classEntity)
    {
        return new ExamArchiveDto
        {
            Id = classEntity.Id,
            Name = classEntity.Name,
            Status = classEntity.Status,
        };
    }

    #endregion
}