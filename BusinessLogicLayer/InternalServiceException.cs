using System.Net;

namespace BusinessLogicLayer
{
    [Serializable]
    public class InternalServiceException: Exception
    {
        public HttpStatusCode StatusCode { get; } = HttpStatusCode.InternalServerError;
        public string InternalServiceExceptionMessage { get; } = string.Empty;

        public InternalServiceException(HttpStatusCode statusCode, string message)
        {
            StatusCode = statusCode;
            InternalServiceExceptionMessage = message;
        }
    }
}
