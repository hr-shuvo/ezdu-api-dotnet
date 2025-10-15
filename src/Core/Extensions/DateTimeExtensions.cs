namespace Core.Extensions;

public static class DateTimeExtensions
{
    public static DateTime? ToUtcSafe(this DateTime? dateTime)
    {
        if (!dateTime.HasValue)
            return null;

        var value = dateTime.Value;

        return value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            DateTimeKind.Unspecified => DateTime.SpecifyKind(value, DateTimeKind.Local).ToUniversalTime(),
            _ => value
        };
    }
    
    public static int CalculateAge(this DateOnly dateOfBirth)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth > today.AddYears(-age)) age--;
        return age;
    }
}