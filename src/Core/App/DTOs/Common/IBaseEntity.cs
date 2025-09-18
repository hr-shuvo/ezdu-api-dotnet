namespace Core.App.DTOs.Common;

public interface IBaseEntity
{
    long Id { get; set; }
    
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
    
    int Status { get; set; }
    
    long? CreatedBy { get; set; }
    long? UpdatedBy { get; set; }
}