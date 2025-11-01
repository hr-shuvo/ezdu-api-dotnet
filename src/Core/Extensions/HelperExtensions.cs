namespace Core.Extensions;

public static class HelperExtensions
{
    public static int XpByPercentage(this int percentage)
    {
        if (percentage is > 100 or < 40)
        {
            percentage = 0;
        }
        
        const int maxXp = 30;
        var normalized = percentage / 100.0;
        var scaled = Math.Pow(normalized, 2.0);
        return (int)Math.Round(maxXp * scaled);
    }
}