using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Core.App.Attributes;

public class UniqueValuesAttribute : ValidationAttribute
{
    private readonly StringComparison _comparisonType;

    public UniqueValuesAttribute(StringComparison comparisonType)
    {
        _comparisonType = comparisonType;
    }
    
    public override bool IsValid(object value)
    {
        if (value == null)
            return true;
        
        if(value is not IEnumerable enumerable)
            return false;

        var items = new List<string>();

        foreach (var item in enumerable)
        {
            if(item == null)
                continue;
            
            var stringItem = item.ToString();
            
            if(string.IsNullOrWhiteSpace(stringItem)) continue;
            
            items.Add(stringItem.Trim());
        }

        var uniqueItems = new HashSet<string>(StringComparer.FromComparison(_comparisonType));
        
        foreach (var item in items)
        {
            if (!uniqueItems.Add(item))
                return false;
        }

        return true;
    }
}