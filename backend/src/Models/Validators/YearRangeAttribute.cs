using System.ComponentModel.DataAnnotations;

namespace ShpaginApp.Models.Validators
{
  public class YearRangeAttribute(int minYear) : ValidationAttribute
  {
    public int MinYear { get; } = minYear;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
      if (value == null)
        return ValidationResult.Success;

      if (value is not int year)
        return new ValidationResult("Year must be integer");

      int maxYear = DateTime.Now.Year;
      if (year < MinYear || year > maxYear)
      {
        return new ValidationResult(
            $"Year must be between {MinYear} and {maxYear} (current year)");
      }

      return ValidationResult.Success;
    }
  }
}
