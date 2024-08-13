using Business.Abstract;
using Business.ValidationRules.FluenValidation.TestValidators;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Entities.Concrete;
using Core.Helpers.PaginationHelper.EntityFramework;
using Core.Utilities.Message.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Abstract;
using Entities.Concrete;
using System.Net;

namespace Business.Concrete
{
    public class TestManager : ITestService
    {
        private readonly ITestDAL _testDAL;
        private readonly ISmsService _smsService;
        private readonly IEmailService _emailService;
        public TestManager(ITestDAL testDAL, ISmsService smsService, IEmailService emailService)
        {
            _testDAL = testDAL;
            _smsService = smsService;
            _emailService = emailService;
        }

        [ValidationAspect(typeof(TestValidator))]
        [CacheRemoveAspect("ITestService.Get")]
        public IResult AddTest(Test model)
        {
            _testDAL.Add(model);
            return new SuccessResult(System.Net.HttpStatusCode.OK);
        }

        public async Task<IDataResult<PagedList<Test>>> GetAllByPage(int page)
        {
            var result = await _testDAL.GetAllByPage(page, 4);
            return new SuccessDataResult<PagedList<Test>>(data: result, HttpStatusCode.OK);
        }

        [CacheAspect<SuccessDataResult<List<Test>>>]
        public IDataResult<List<Test>> GetAllTest()
        {
            var data = _testDAL.GetAll(tracking: false);
            return new SuccessDataResult<List<Test>>(data: data, statusCode: HttpStatusCode.OK);
        }
        [CacheAspect<SuccessDataResult<Test>>]
        public IDataResult<Test> GetById(string id)
        {
            var result = _testDAL.Get(x => x.Id == Guid.Parse(id), tracking: false);
            return new SuccessDataResult<Test>(data: result, statusCode: System.Net.HttpStatusCode.OK);
        }

        public async Task<IResult> SendEmailAsync(EmailMetadata emailData)
        {
            await _emailService.SendEmailAsync(emailData);
            return new SuccessResult(HttpStatusCode.OK);
        }

        public async Task<IResult> SendTestSms(string phoneNumber, string otp)
        {
            var result = await _smsService.SendOtpSmsAsync(phoneNumber, otp);
            if (result)
                return new SuccessResult(HttpStatusCode.OK);
            return new ErrorResult(HttpStatusCode.BadRequest);
        }

        [CacheRemoveAspect("ITestService.Get")]
        public IResult UpdateTest(string id, Test model)
        {
            var data = _testDAL.Get(x => x.Id == Guid.Parse(id));
            data.Name = model.Name;
            _testDAL.Update(data);
            return new SuccessResult(System.Net.HttpStatusCode.OK);
        }
    }
}
