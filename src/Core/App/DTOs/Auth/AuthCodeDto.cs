namespace Core.App.DTOs.Auth;

public class SendOtpDto
{
    public string Recipient { get; set; }
    public bool IsPhone { get; set; } = false;
}

public class VerifyCodeDto
{
    public string Recipient { get; set; }
    public int Code { get; set; }
}