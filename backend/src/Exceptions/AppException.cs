using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics;

namespace ShpaginApp.Exceptions
{
  internal class AppException(int statusCode, string message, string? detail = null) : Exception(message)
  {
    public int StatusCode { get; } = statusCode;
    public string? Detail { get; } = detail;

    public AppError ToAppError()
    {
      return new AppError { Status = StatusCode, Error = Message, Detail = Detail };
    }
  }

  internal class AppError
  {
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyOrder(-3)]
    [JsonPropertyName("status")]
    public int? Status { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyOrder(-2)]
    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyOrder(-1)]
    [JsonPropertyName("detail")]
    public string? Detail { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyOrder(0)]
    [JsonPropertyName("details")]
    public object? Details { get; set; }
  }

  internal sealed class AppExceptionHandler : IExceptionHandler
  {
    public async ValueTask<bool> TryHandleAsync(
      HttpContext httpContext,
      Exception exception,
      CancellationToken cancellationToken)
    {
      if (exception is not AppException appException)
      {
        return false;
      }
      await HandleException.HandleAsync(httpContext, appException.ToAppError());
      return true;
    }
  }
}
