using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace ShpaginApp.Exceptions
{
  internal sealed class ValidationExceptionHandler : IExceptionHandler
  {
    public async ValueTask<bool> TryHandleAsync(
      HttpContext httpContext,
      Exception exception,
      CancellationToken cancellationToken)
    {
      if (exception is not ValidationException validationException)
      {
        return false;
      }

      await HandleException.HandleAsync(httpContext, new AppError
      {
        Status = StatusCodes.Status400BadRequest,
        Error = "Validation Error",
        Detail = "One or more validation details occurred",
        Details = validationException.Errors
          .GroupBy(e => e.PropertyName)
          .ToDictionary(
            g => g.Key,
            g => g.Select(e => e.ErrorMessage).ToArray()
          )
      });
      return true;
    }
  }
}
