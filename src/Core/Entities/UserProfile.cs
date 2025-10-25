using Core.App.DTOs.Common;
using Core.Enums;

namespace Core.Entities;

public class UserProfile : BaseEntity
{
    public long UserId { get; set; }
    public Segment Segment { get; set; }

    public long ClassId { get; set; }
    public string ClassName { get; set; } // class 7, 8, 12, BCS, Bank, IELTS

    public string Group { get; set; }
    public string ExamType { get; set; } // "SSC", "HSC" (required if class >= 9)
    public int? ExamYear { get; set; }
    public long InstituteId { get; set; }
    public string Institute { get; set; }

    // International
    public float TargetScore { get; set; }

    public UserProfile(long userId)
    {
        Id = userId;
        UserId = userId;
    }
}