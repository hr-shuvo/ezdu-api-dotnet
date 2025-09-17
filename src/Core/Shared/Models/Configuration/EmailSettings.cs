namespace Core.Shared.Models.Configuration;

public class EmailSettings
{
    // SMTP connection
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool UseSsl { get; set; }
    
    // Email identity
    public string FromAddress { get; set; } = string.Empty;
    public string FromName { get; set; } = string.Empty;
    public string ReplyToAddress { get; set; }
}