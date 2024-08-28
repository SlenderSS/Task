using System.Globalization;
using System.Windows.Controls;
using DryIoc.ImTools;

namespace Task.Helpers;

public class DataGridCellValitationRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        
        var cellValue = value.ToString();

        if(cellValue?.Length > 10)
            return new ValidationResult(false, "Lenght can`t be more than 10!");
        if ( string.IsNullOrWhiteSpace(cellValue))
            return new ValidationResult(false, "Value can`t be empty!");
        
        
        return ValidationResult.ValidResult;
    }
}

