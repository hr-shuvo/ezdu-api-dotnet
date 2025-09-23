using Core.App.DTOs.Common;

namespace Core.Entities;

public class QuestionReference : BaseEntity
{
    public long QuestionId { get; set; }
    public long InstituteId { get; set; }

    public int? Year { get; set; }
}