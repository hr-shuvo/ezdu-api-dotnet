using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Core.DTOs;

public  class UserProfileDto
{
    public Segment Segment { get; set; }
    
    [Required(ErrorMessage = "Category is required")]
    public long? ClassId { get; set; }
    
    public string Group { get; set; }
    public string ExamType { get; set; }
    public int? ExamYear { get; set; }
    public long? InstituteId { get; set; }
    
    public float? TargetScore { get; set; }
}