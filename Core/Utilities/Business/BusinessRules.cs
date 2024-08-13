using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using System.Net;

namespace Core.Utilities.Business
{
    public static class BusinessRules
    {
        public static IResult CheckLogic(params IResult[] logics)
        {
            foreach (var logic in logics)
            {
                if (!logic.Success)
                    return new ErrorResult(statusCode: HttpStatusCode.BadRequest);
            }
            return new SuccessResult(statusCode: HttpStatusCode.OK);
        }
    }
}
