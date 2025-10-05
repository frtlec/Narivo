using System.Net;

namespace Narivo.Shared.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string message)
            : base(message, HttpStatusCode.NotFound, "NOT_FOUND") { }
    }
}
