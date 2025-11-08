using Microsoft.AspNetCore.Diagnostics;

namespace ShpaginApp.Exceptions
{
  internal static class HandleException
  {
    public static Task HandleAsync(HttpContext context, AppError error)
    {
      context.Response.StatusCode = error.Status ?? StatusCodes.Status500InternalServerError;
      return context.Response.WriteAsJsonAsync(error);
    }
  }

  internal sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
  {
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
      HttpContext httpContext,
      Exception exception,
      CancellationToken cancellationToken)
    {
      _logger.LogError(exception, "Unhandled error");
      await HandleException.HandleAsync(
        httpContext, new AppError
        {
          Status = StatusCodes.Status500InternalServerError,
          Error = "Unexpected server error",
          Detail = GetExceptionChainAndMessages(exception),
        });
      return true;
    }

    private static string GetExceptionChainAndMessages(Exception ex)
    {
      var chain = new List<Exception>();
      var current = ex;
      while (current != null)
      {
        chain.Add(current);
        current = current.InnerException;
      }

      // Создаём строку цепочки типов исключений
      var exceptionTypes = string.Join(" → ", chain.Select(e => e.GetType().Name));

      // Объединяем все сообщения
      var messages = string.Join(Environment.NewLine, chain.Select(e => e.Message));

      return $"{exceptionTypes}: {messages}";
    }

  }
}
