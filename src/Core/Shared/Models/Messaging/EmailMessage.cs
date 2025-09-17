namespace Core.Shared.Models.Messaging;

public class EmailMessage
{
    // Recipient
    public string To { get; set; } = string.Empty;
    public string ToName { get; set; } = string.Empty;

    // Email content
    public string Subject { get; set; } = string.Empty; 
    public string Body { get; set; } = string.Empty;
    public bool IsHtml { get; set; } = true;

    // Optional CC/BCC (with optional names)
    public List<string> Cc { get; set; } = [];
    public List<string> Bcc { get; set; } = [];

    // Attachments
    public List<string> Attachments { get; set; } = [];
}