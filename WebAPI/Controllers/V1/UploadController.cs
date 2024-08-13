using Asp.Versioning;
using Business.Utilities.Storage.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {

        private readonly IStorageService _storageService;

        public UploadController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [MapToApiVersion("1.0")]
        [HttpPost("[action]")]
        public async Task<IActionResult> UploadFile(string pathOrContainer, IFormFile file)
        {
            var result = await _storageService.UploadFileAsync(pathOrContainer, file);
            return Ok(result);
        }

        [MapToApiVersion("1.0")]
        [HttpPost("[action]")]
        public async Task<IActionResult> UploadFiles(string pathOrContainer, IFormFileCollection files)
        {
            var result = await _storageService.UploadFilesAsync(pathOrContainer, files);
            return Ok(result);
        }

        [MapToApiVersion("1.0")]
        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteFile(string pathOrContainer, string filename)
        {
            await _storageService.DeleteAsync(pathOrContainer, filename);
            return Ok();
        }

        [MapToApiVersion("1.0")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllFiles(string? pathOrContainer = null)
        {
            var response = await _storageService.GetAllFilesAsync(pathOrContainer);
            return Ok(response);
        }
    }
}
