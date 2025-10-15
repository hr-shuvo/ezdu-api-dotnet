using Core.App.DTOs.Common;

namespace Core.Entities;

public class QuestionReference : BaseEntity // previously used in exam | DU 25, BRUR 22
{
    public long QuestionId { get; set; }
    public long InstituteId { get; set; }

    public int? Year { get; set; }
}