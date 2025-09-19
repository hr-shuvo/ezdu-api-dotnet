using System.ComponentModel.DataAnnotations;

namespace Core.App.Attributes;

public class AllowedValuesAttribute : ValidationAttribute
{
    private readonly string[] _allowedValues;
    private readonly StringComparison _comparisonType;


    public AllowedValuesAttribute(params string[] allowedValues)
    {
        _allowedValues = allowedValues ?? throw new ArgumentNullException(nameof(allowedValues));
        _comparisonType = StringComparison.OrdinalIgnoreCase;
        ErrorMessage = $"Invalid value. Allowed values are: {string.Join(", ", allowedValues)}";
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
            return ValidationResult.Success;

        if (value is not IEnumerable<string> items)
        {
            return new ValidationResult(
                $"{validationContext.DisplayName ?? validationContext.MemberName} must be a list of strings.",
                new[] { validationContext.MemberName });
        }

        var itemList = items
            .Where(item => !string.IsNullOrWhiteSpace(item))
            .Select(item => item.Trim())
            .ToList();

        // ✅ Empty list is allowed
        if (!itemList.Any())
        {
            return ValidationResult.Success;
        }

        var seen = new HashSet<string>(StringComparer.FromComparison(_comparisonType));
        var invalidItems = new List<string>();
        var duplicates = new List<string>();

        foreach (var item in itemList)
        {
            // Check if value is allowed
            if (!_allowedValues.Any(allowed => string.Equals(allowed, item, _comparisonType)))
            {
                if (!invalidItems.Contains(item, StringComparer.FromComparison(_comparisonType)))
                {
                    invalidItems.Add(item);
                }

                continue;
            }

            // Check for duplicates
            if (!seen.Add(item))
            {
                if (!duplicates.Contains(item, StringComparer.FromComparison(_comparisonType)))
                {
                    duplicates.Add(item);
                }
            }
        }

        if (invalidItems.Any())
        {
            return new ValidationResult(
                $"Invalid {validationContext.DisplayName ?? validationContext.MemberName}: {string.Join(", ", invalidItems)}. " +
                $"Allowed values are: {string.Join(", ", _allowedValues)}",
                new[] { validationContext.MemberName });
        }

        if (duplicates.Any())
        {
            return new ValidationResult(
                $"Duplicate {validationContext.DisplayName ?? validationContext.MemberName} found: {string.Join(", ", duplicates)}",
                new[] { validationContext.MemberName });
        }

        return ValidationResult.Success;
    }
}