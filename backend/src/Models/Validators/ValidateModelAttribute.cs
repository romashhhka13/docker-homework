using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ShpaginApp.Models.Validators
{
  internal sealed class ValidateModelAttribute : ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext context)
    {
      if (!context.ModelState.IsValid)
      {
        IEnumerable<ValidationFailure> validationFailures = context.ModelState
          .Where(kvp => kvp.Value != null && kvp.Value.Errors.Count > 0)
          .SelectMany(kvp => kvp.Value!.Errors.Select(error =>
              new ValidationFailure(kvp.Key, error.ErrorMessage)
          ));

        throw new ValidationException(validationFailures);
      }
    }
  }
}
