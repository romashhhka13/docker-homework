using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ShpaginApp.Models.Validators
{
  public class DateAttribute(int minYear = 1900) : ValidationAttribute
  {
    private readonly int _minYear = minYear;
    private readonly string _dateFormat = "yyyy-MM-dd";

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
      if (value == null)
        return ValidationResult.Success;

      if (value is DateOnly dateValue)
      {
        var today = DateOnly.FromDateTime(DateTime.Now);
        if (dateValue < new DateOnly(_minYear, 1, 1) || dateValue > today)
        {
          return new ValidationResult(
              $"Date must be between {_minYear}-01-01 and {today:yyyy-MM-dd}");
        }
        return ValidationResult.Success;
      }

      if (value is string stringValue)
      {
        if (!DateOnly.TryParseExact(stringValue, _dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
        {
          return new ValidationResult($"Date must be in '{_dateFormat}' format");
        }

        var today = DateOnly.FromDateTime(DateTime.Now);
        if (parsedDate < new DateOnly(_minYear, 1, 1) || parsedDate > today)
        {
          return new ValidationResult(
              $"Date must be between {_minYear}-01-01 and {today:yyyy-MM-dd}");
        }
        return ValidationResult.Success;
      }

      return new ValidationResult("Invalid date value");
    }
  }
}
