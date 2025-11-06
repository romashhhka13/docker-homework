namespace ShpaginApp.Auth
{
  public class AuthOptions
  {
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required TimeSpan Lifetime { get; set; }
    public required string SecretKey { get; set; }
  }

  public static class AuthRoles
  {
    public const string Admin = "admin";
    public const string User = "user";
  }

}
