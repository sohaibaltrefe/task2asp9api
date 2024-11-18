using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace task2.Errors
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            this.logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception,"catch error",exception.Message);
            var ProblemDetail = new ProblemDetails()
            {Status=StatusCodes.Status500InternalServerError,
            Title="server Error",
            Detail=exception.Message,
            };
            httpContext.Response.StatusCode=ProblemDetail.Status.Value;

            await httpContext.Response
            .WriteAsJsonAsync(ProblemDetail);

            return true;
        }
    }
}
