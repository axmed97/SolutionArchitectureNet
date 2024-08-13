using Business.Utilities.Storage.Abstract;
using Entities.Common;
using Microsoft.AspNetCore.Http;

namespace Business.Utilities.Storage.Concrete
{
    public class StorageService : IStorageService
    {
        private readonly IStorage _storage;

        public StorageService(IStorage storage)
        {
            _storage = storage;
        }

        public string StorageName { get => _storage.GetType().Name; }

        public async Task DeleteAsync(string pathOrContainerName, string fileName)
            => await _storage.DeleteAsync(pathOrContainerName, fileName);

        public async Task<List<string>> GetAllFilesAsync(string? pathOrContainerName = null)
            => await _storage.GetAllFilesAsync(pathOrContainerName);

        public async Task<Upload> UploadFileAsync(string containerName, IFormFile file)
            => await _storage.UploadFileAsync(containerName, file);

        public async Task<List<Upload>> UploadFilesAsync(string containerName, IFormFileCollection files)
            => await _storage.UploadFilesAsync(containerName, files);
    }
}
