using System.ComponentModel.DataAnnotations;

namespace Core.DTOs;

public class ExamArchiveDto
{
    public long Id { get; set; }
    [Required]
    public string Name { get; set; }
    
    public int Status { get; set; }
    
    public long ClassId { get; set; }
    [Required]
    public long? SubjectId { get; set; }

    public long? InstituteId { get; set; }
    public int? Year { get; set; }
}