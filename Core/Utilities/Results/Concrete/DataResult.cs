using Core.Utilities.Results.Abstract;
using System.Net;

namespace Core.Utilities.Results.Concrete
{
    public class DataResult<T> : Result, IDataResult<T>
    {
        public T Data { get; }
        public DataResult(T data, bool success, string message, HttpStatusCode statusCode) : base(success, message, statusCode)
        {
            Data = data;
        }

        public DataResult(T data, bool success, HttpStatusCode statusCode) : base(success, statusCode)
        {
            Data = data;
        }
    }
}
