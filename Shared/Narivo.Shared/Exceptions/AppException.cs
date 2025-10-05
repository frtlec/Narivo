using System.Net;

namespace Narivo.Shared.Exceptions
{
    public class AppException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string ErrorCode { get; }

        public AppException(string? message, HttpStatusCode statusCode = HttpStatusCode.BadRequest, string errorCode = "APP_ERROR")
            : base(message ?? "Beklenmedik bir hata oluştu")
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }

    }
}
