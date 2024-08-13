using Asp.Versioning;
using Business.Abstract;
using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class TestController(ITestService testService) : ControllerBase
    {
        [MapToApiVersion("1.0")]
        [HttpPost]
        public IActionResult Post(Test model)
        {
            var result = testService.AddTest(model);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        public IActionResult Get()
        {
            var result = testService.GetAllTest();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [MapToApiVersion("1.0")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetByPage(int page)
        {
            var result = await testService.GetAllByPage(page);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [MapToApiVersion("1.0")]
        [HttpPost("[action]")]
        public async Task<IActionResult> SendSms(string phoneNumber, string otp)
        {
            var result = await testService.SendTestSms(phoneNumber, otp);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [MapToApiVersion("1.0")]
        [HttpPost("[action]")]
        public async Task<IActionResult> SendEmail(EmailMetadata emailData)
        {
            var result = await testService.SendEmailAsync(emailData);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
