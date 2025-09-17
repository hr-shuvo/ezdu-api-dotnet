using Core.Errors;
using Core.Shared.Models.Configuration;
using Core.Shared.Models.Messaging;
using Core.Shared.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Core.Shared.Services;

// var message = new EmailMessage()
// {
//     To = user.Email,
//     ToName = user.UserName,
//     Subject = "New Login Notification",
//     Body =
//         $"Hello {user.UserName},\n\nWe noticed a new login to your account " +
//         $"on {DateTime.UtcNow} UTC.\n\nIf this was you, no further action is needed.\n\n" +
//         $"If you did not log in, please reset your password immediately and contact support.\n\n" +
//         $"Best regards,\nYour Company",
//     IsHtml = true,
// };

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailService> _logger;
    
    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _logger = logger;
        _emailSettings = configuration.GetSection("SMTP").Get<EmailSettings>() ?? new EmailSettings();
    }
    
    public async Task<bool> SendEmailAsync(EmailMessage message)
    {
        try
        {
            // Validate email settings
            if (string.IsNullOrEmpty(_emailSettings.Host) || 
                string.IsNullOrEmpty(_emailSettings.Username) || 
                string.IsNullOrEmpty(_emailSettings.FromAddress))
            {
                throw new AppException("SMTP configuration is incomplete");
            }

            // Validate message
            if (string.IsNullOrEmpty(message.To) || 
                string.IsNullOrEmpty(message.Subject) ||
                string.IsNullOrEmpty(message.ToName)
                )
            {
                throw new AppException("Email message must include recipient, subject, and recipient name");
            }
            
            
            var mimeMessage = new MimeMessage();
            
            // Set From address
            mimeMessage.From.Add(new MailboxAddress(
                _emailSettings.FromName,
                _emailSettings.FromAddress
            ));

            
            // Set To address
            
            mimeMessage.To.Add(new MailboxAddress(message.ToName, message.To));

            
            // Add CC recipients
            if (message.Cc?.Count > 0)
            {
                foreach (var cc in message.Cc.Where(x => !string.IsNullOrEmpty(x)))
                {
                    mimeMessage.Cc.Add(MailboxAddress.Parse(cc));
                }
            }
            
            // Add BCC recipients
            if (message.Bcc?.Count > 0)
            {
                foreach (var bcc in message.Bcc.Where(x => !string.IsNullOrEmpty(x)))
                {
                    mimeMessage.Bcc.Add(MailboxAddress.Parse(bcc));
                }
            }
            
            // Set subject
            mimeMessage.Subject = message.Subject;
            
            // Create body
            var bodyBuilder = new BodyBuilder();
            if (message.IsHtml)
            {
                bodyBuilder.HtmlBody = message.Body;
            }
            else
            {
                bodyBuilder.TextBody = message.Body;
            }
            
            // Add attachments
            if (message.Attachments?.Count > 0)
            {
                foreach (var attachmentPath in message.Attachments.Where(x => !string.IsNullOrEmpty(x)))
                {
                    if (File.Exists(attachmentPath))
                    {
                        await bodyBuilder.Attachments.AddAsync(attachmentPath);
                    }
                }
            }
            
            mimeMessage.Body = bodyBuilder.ToMessageBody();
            
            // Send email using MailKit SmtpClient
            using var client = new SmtpClient();
            
            // Connect to server
            await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
            
            // Authenticate
            await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
            
            // Send message
            var result = await client.SendAsync(mimeMessage);
            
            if(string.IsNullOrWhiteSpace(result))
                throw new AppException("Failed to send email");
            
            
            
            // Disconnect
            await client.DisconnectAsync(true);
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email");
            throw;
        }
    }

    public Task<bool> SendWelcomeEmailAsync(string toEmail, string userName)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SendPasswordResetEmailAsync(string toEmail, string resetToken)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SendLoginNotificationEmailAsync(string toEmail, string userName)
    {
        try
        {
            // Validate email settings
            if (string.IsNullOrEmpty(_emailSettings.Host) || 
                string.IsNullOrEmpty(_emailSettings.Username) || 
                string.IsNullOrEmpty(_emailSettings.FromAddress))
            {
                throw new AppException("SMTP configuration is incomplete");
            }

            // Validate message
            if (string.IsNullOrEmpty(toEmail) || 
                string.IsNullOrEmpty(userName)
                )
            {
                throw new AppException("Email message must include recipient, subject, and recipient name");
            }
            
            
            var mimeMessage = new MimeMessage();
            
            // Set From address
            mimeMessage.From.Add(new MailboxAddress(
                _emailSettings.FromName,
                _emailSettings.FromAddress
            ));

            
            // Set To address
            
            mimeMessage.To.Add(new MailboxAddress(userName, toEmail));
            
            // Set subject
            mimeMessage.Subject = "New Login Notification";
            
            // Create body
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody =$"Hello {userName},\n\nWe noticed a new login to your account " +
                          $"on {DateTime.UtcNow} UTC.\n\nIf this was you, no further action is needed.\n\n" +
                          $"If you did not log in, please reset your password immediately and contact support.\n\n" +
                          $"Best regards,\nYour Company"
            };
            
            
            mimeMessage.Body = bodyBuilder.ToMessageBody();
            
            // Send email using MailKit SmtpClient
            using var client = new SmtpClient();
            
            await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
            
            var result = await client.SendAsync(mimeMessage);
            
            if(string.IsNullOrWhiteSpace(result))
                throw new AppException("Failed to send email");
            
            await client.DisconnectAsync(true);
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email");
            throw;
        }
    }

    public Task<bool> SendPasswordChangeConfirmationEmailAsync(string toEmail, string userName)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SendAccountLockoutEmailAsync(string toEmail, string userName, DateTime lockoutEnd)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SendAccountUnlockEmailAsync(string toEmail, string userName)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SendSubscriptionExpiryEmailAsync(string toEmail, string userName, DateTime expiryDate)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SendPlanUpgradeConfirmationEmailAsync(string toEmail, string userName, string planName)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SendNewFeatureAnnouncementEmailAsync(string toEmail, string featureName)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SendMaintenanceNotificationEmailAsync(string toEmail, string maintenanceTime)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SendFeedbackRequestEmailAsync(string toEmail, string userName)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SendSurveyInvitationEmailAsync(string toEmail, string userName, string surveyLink)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SendVerificationEmailAsync(string toEmail, string verificationCode)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SendTwoFactorCodeEmailAsync(string toEmail, string code)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SendPromotionalEmailAsync(string toEmail, string subject, string body)
    {
        throw new NotImplementedException();
    }
}