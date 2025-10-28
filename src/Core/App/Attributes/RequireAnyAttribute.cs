using System.ComponentModel.DataAnnotations;

namespace Core.App.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RequireAnyAttribute : ValidationAttribute
{
    private readonly string[] _propertyNames;

    public RequireAnyAttribute(params string[] propertyNames)
    {
        _propertyNames = propertyNames;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var type = validationContext.ObjectType;
        bool anyHasValue = _propertyNames.Any(propName =>
        {
            var prop = type.GetProperty(propName);
            var val = prop?.GetValue(validationContext.ObjectInstance);
            return val != null;
        });

        if (anyHasValue)
            return ValidationResult.Success;

        string message = ErrorMessage ??
                         $"At least one of [{string.Join(", ", _propertyNames)}] must be provided.";

        return new ValidationResult(message);
    }
}