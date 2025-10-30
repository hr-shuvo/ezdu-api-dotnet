using Core.App.DTOs.Common;
namespace Core.Entities;

/// <summary>
/// Definition: Specific exam like “SSC 2020 Physics (MCQ)” or “BCS Preliminary 43rd”
/// Class: SSC, HSC, Class 8, BCS, Bank, etc.
/// Subject: Physics, Bangla, English, General Knowledge, etc.
/// Institute: Dinajpur Board, Dhaka University, Rangpur Zilla School, etc.
/// </summary>
public class ArchiveExam : BaseEntity
{
    public long ClassId { get; set; }
    public long SubjectId { get; set; }

    public long InstituteId { get; set; }
    public int Year { get; set; }

    public ICollection<Question> Questions { get; set; } = [];
}