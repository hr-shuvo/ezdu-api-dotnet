using PhoneNumbers;

namespace Core.App.Utils;

public static class NormalizeHelper
{
    public static string NormalizePhoneNumber(string input, string defaultRegion = "BD")
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;
        
        var phoneUtil = PhoneNumberUtil.GetInstance();

        try
        {
            var numberProto = phoneUtil.Parse(input, defaultRegion);
            if (!phoneUtil.IsValidNumber(numberProto))
                return null;
            
            return phoneUtil.Format(numberProto, PhoneNumberFormat.E164);
        }
        catch (NumberParseException)
        {
            return null;
        }
        
    }
    
    public static string NormalizeEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        return email.Trim().ToLowerInvariant();
    }
    
    
    
    
    
}