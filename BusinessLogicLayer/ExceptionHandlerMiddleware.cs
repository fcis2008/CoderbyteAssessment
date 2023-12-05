using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mime;
using System.Text;

namespace BusinessLogicLayer
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandlerMiddleware> logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (InternalServiceException ex)
            {
                await HandleInternalServiceException(context, ex);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private async Task HandleInternalServiceException(HttpContext context, InternalServiceException exception)
        {
            logger.LogCritical("CRITICAL: Unhandled exception {@exception}", exception);

            await UpdateHttpResponse(context, exception);
        }

        private async Task HandleException(HttpContext context, Exception exception)
        {
            logger.LogCritical("CRITICAL: Unhandled exception {@exception}", exception);

            InternalServiceException internalServiceException;

            switch (exception)
            {
                case ApplicationException:
                    // custom application error
                    internalServiceException = new InternalServiceException(HttpStatusCode.BadRequest, exception.Message);
                    break;
                case KeyNotFoundException:
                    // not found error
                    internalServiceException = new InternalServiceException(HttpStatusCode.NotFound, exception.Message);
                    break;
                default:
                    // unhandled error
                    internalServiceException = new InternalServiceException(HttpStatusCode.InternalServerError, exception.Message);
                    break;
            }
            await UpdateHttpResponse(context, internalServiceException);
        }

        private static async Task UpdateHttpResponse(HttpContext context, InternalServiceException exception)
        {
            context.Response.StatusCode = (int)exception.StatusCode;
            context.Response.ContentType = MediaTypeNames.Text.Plain;
            context.Response.ContentLength = exception.InternalServiceExceptionMessage.Length;

            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(exception.InternalServiceExceptionMessage));
        }
    }
}