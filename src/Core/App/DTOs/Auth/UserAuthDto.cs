namespace Core.App.DTOs.Auth;

public class UserAuthDto
{
    public long Id { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string Token { get; set; }
}