namespace Core.App.DTOs.Auth;

public class SendOtpDto
{
    public string Recipient { get; set; }
    public bool IsPhone { get; set; } = false;
}