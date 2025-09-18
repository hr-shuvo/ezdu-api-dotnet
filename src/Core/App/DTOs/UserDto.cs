namespace Core.App.DTOs;

public class UserDto
{
    public long Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string PhotoUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastActive { get; set; }

    public int Status { get; set; }
    
}