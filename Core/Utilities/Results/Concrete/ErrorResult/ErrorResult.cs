using System.Net;

namespace Core.Utilities.Results.Concrete.ErrorResult
{
    public class ErrorResult : Result
    {
        public ErrorResult(string message, HttpStatusCode statusCode) : base(false, message, statusCode)
        {
        }
        public ErrorResult(HttpStatusCode statusCode) : base(false, statusCode)
        {
        }
    }
}
