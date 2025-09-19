
namespace Core.App.Utils;

public static class Helper
{
    public static int GenerateLoginCode(int n = 6)
    {
        if (n is < 2 or > 9) n = 6;
        
        var random = new Random();
        var min = (int)Math.Pow(10, n - 1);
        var max = (int)Math.Pow(10, n) - 1;
        
        return random.Next(min, max);
    }
    
    
    
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}