using Core.Shared.Models.Messaging;

namespace Core.Shared.Services.Interfaces;

public interface IEmailService
{
    // Generic send
    Task<bool> SendEmailAsync(EmailMessage message);

    // Account-related emails
    Task<bool> SendWelcomeEmailAsync(string toEmail, string userName);
    Task<bool> SendPasswordResetEmailAsync(string toEmail, string resetToken);
    Task<bool> SendLoginNotificationEmailAsync(string toEmail, string userName);
    Task<bool> SendPasswordChangeConfirmationEmailAsync(string toEmail, string userName);
    Task<bool> SendAccountLockoutEmailAsync(string toEmail, string userName, DateTime lockoutEnd);
    Task<bool> SendAccountUnlockEmailAsync(string toEmail, string userName);

    // Subscription / plan emails
    Task<bool> SendSubscriptionExpiryEmailAsync(string toEmail, string userName, DateTime expiryDate);
    Task<bool> SendPlanUpgradeConfirmationEmailAsync(string toEmail, string userName, string planName);

    // Product / feature emails
    Task<bool> SendNewFeatureAnnouncementEmailAsync(string toEmail, string featureName);
    Task<bool> SendMaintenanceNotificationEmailAsync(string toEmail, string maintenanceTime);

    // Feedback / survey
    Task<bool> SendFeedbackRequestEmailAsync(string toEmail, string userName);
    Task<bool> SendSurveyInvitationEmailAsync(string toEmail, string userName, string surveyLink);

    // Verification / security
    Task<bool> SendVerificationEmailAsync(string toEmail, string verificationCode);
    Task<bool> SendTwoFactorCodeEmailAsync(string toEmail, string code);

    // Optional: marketing / promotions
    Task<bool> SendPromotionalEmailAsync(string toEmail, string subject, string body);
    
    
}