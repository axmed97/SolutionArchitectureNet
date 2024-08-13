using Core.Entities.Concrete;
using Core.Helpers.PaginationHelper.EntityFramework;
using Core.Utilities.Results.Abstract;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ITestService
    {
        IResult AddTest(Test model);
        IResult UpdateTest(string id, Test model);
        IDataResult<List<Test>> GetAllTest();
        IDataResult<Test> GetById(string id);
        Task<IDataResult<PagedList<Test>>> GetAllByPage(int page);
        Task<IResult> SendTestSms(string phoneNumber, string otp);
        Task<IResult> SendEmailAsync(EmailMetadata emailData);

    }
}
