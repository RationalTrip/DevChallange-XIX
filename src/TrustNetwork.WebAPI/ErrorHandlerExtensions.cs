using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TrustNetwork.Domain.Exceptions.Results;

namespace TrustNetwork.WebAPI
{
    internal static class ErrorHandlerExtensions
    {
        public static ActionResult HandleError(this Exception exception)
            => exception switch
            {
                NotFoundException exc => 
                    new NotFoundObjectResult(exc.Message),
                ValidationException exc => 
                    new BadRequestObjectResult(exc.GetDetails()),
                _ => throw exception
            };

        static ValidationProblemDetails GetDetails(this ValidationException exp) =>
            new ValidationProblemDetails() { Detail = exp.Message };
    }
}
