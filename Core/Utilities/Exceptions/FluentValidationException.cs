using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Exceptions
{
    public class FluentValidationException(string? message, Exception? inner, HttpStatusCode? statusCode) : HttpRequestException
    {
    }
}
