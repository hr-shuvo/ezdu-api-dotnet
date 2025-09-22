using System.ComponentModel.DataAnnotations.Schema;
using Core.App.DTOs.Common;
using Core.Enums;

namespace Core.Entities;

public class Subject : BaseEntity
{
    public string SubTitle { get; set; }
    public string Code { get; set; }
    public string Groups { get; set; }
    public bool HasPaper { get; set; }
    public bool HasPractical { get; set; }

    public long ClassId { get; set; }
    public Segment Segment { get; set; }

    [NotMapped]
    public List<Group> GroupList {
        get => string.IsNullOrWhiteSpace(Groups) ? [] : Groups.Split(',').Select(Enum.Parse<Group>).ToList();
        set => Groups = string.Join(",", value);
    }
    
}