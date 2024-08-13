using System.Net;

namespace Core.Utilities.Results.Concrete.SuccessResult
{
    public class SuccessResult : Result
    {
        public SuccessResult(string message, HttpStatusCode statusCode) : base(true, message, statusCode)
        {
        }
        public SuccessResult(HttpStatusCode statusCode) : base(true, statusCode)
        {
        }
    }
}
