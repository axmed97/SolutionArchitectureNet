using Entities.Common;
using Microsoft.AspNetCore.Http;

namespace Business.Utilities.Storage.Abstract
{
    public interface IStorage
    {
        Task<Upload> UploadFileAsync(string containerName, IFormFile file);
        Task<List<Upload>> UploadFilesAsync(string containerName, IFormFileCollection files);
        Task DeleteAsync(string pathOrContainerName, string fileName);
        Task<List<string>> GetAllFilesAsync(string? pathOrContainerName = null);
    }
}
