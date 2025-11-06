using System.ComponentModel.DataAnnotations;

namespace ShpaginApp.Models.Validators
{
  public class EnumAttribute : ValidationAttribute
  {
    private readonly Type _enumType;

    public EnumAttribute(Type enumType)
    {
      if (!enumType.IsEnum)
        throw new ArgumentException("Type must be an enum.");

      _enumType = enumType;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
      if (value is null)
        return ValidationResult.Success;

      if (!Enum.IsDefined(_enumType, value))
      {
        var namesAndValues = Enum.GetValues(_enumType)
                .Cast<object>()
                .Select(v => $"{v} ({Convert.ToInt32(v)})");

        var validValues = string.Join(", ", namesAndValues);

        return new ValidationResult(
            $"Value '{value}' is not valid for enum {_enumType.Name}. Valid values: {validValues}");
      }

      return ValidationResult.Success;
    }
  }
}
