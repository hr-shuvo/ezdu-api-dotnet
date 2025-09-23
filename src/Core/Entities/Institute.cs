using Core.App.DTOs.Common;
using Core.Enums;

namespace Core.Entities;

public class Institute : BaseEntity
{
    public string Code { get; set; }
    public string Address { get; set; }
    public string WebsiteUrl { get; set; }

    public InstituteType InstituteType { get; set; }


    public ICollection<QuestionReference> QuestionReferences { get; set; } = [];
}