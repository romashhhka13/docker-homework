using System.Text.Json.Serialization;
using Npgsql;

namespace ShpaginApp.Models
{
  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum RussianAgeRatingEnum
  {
    ZERO,
    SIX,
    TWELVE,
    SIXTEEN,
    EIGHTEEN,
  }

  public enum BookStatusEnum
  {
    WANT_TO_READ,
    READING,
    COMPLETED,
  }

  public class UpperCaseEnumTranslator : INpgsqlNameTranslator
  {
    public string TranslateMemberName(string clrName) => clrName.ToUpper();
    public string TranslateTypeName(string clrName) => clrName.ToUpper();
  }

  [Flags]
  public enum BookIncludeOptions { Details = 1, Tags = 2 }
}
