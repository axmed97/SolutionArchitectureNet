using System.Net;

namespace Core.Utilities.Results.Concrete.ErrorResult
{
    public class ErrorDataResult<T> : DataResult<T>
    {
        public ErrorDataResult(T data, string message, HttpStatusCode statusCode) : base(data, false, message, statusCode)
        {
        }
        public ErrorDataResult(T data, HttpStatusCode statusCode) : base(data, false, statusCode)
        {
        }
        public ErrorDataResult(string message, HttpStatusCode statusCode) : base(default, false, message, statusCode)
        {
        }
        public ErrorDataResult(HttpStatusCode statusCode) : base(default, false, statusCode)
        {
        }
    }
}
