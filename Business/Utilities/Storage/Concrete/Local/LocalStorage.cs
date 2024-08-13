using Business.Utilities.Storage.Abstract.Local;
using Entities.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Business.Utilities.Storage.Concrete.Local
{
    public class LocalStorage : Storage, ILocalStorage
    {
        private readonly IWebHostEnvironment _env;
        public LocalStorage(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task DeleteAsync(string pathOrContainerName, string fileName)
            => System.IO.File.Delete($"{pathOrContainerName}\\{fileName}");

        public Task<List<string>> GetAllFilesAsync(string? pathOrContainerName)
        {
            if (pathOrContainerName == null)
            {
                var wwwrootPath = Path.Combine(_env.WebRootPath);
                var files = GetFilesFromDirectory(wwwrootPath);
                return Task.FromResult(files);
            }
            var path = Path.Combine(_env.WebRootPath, pathOrContainerName);
            DirectoryInfo directoryInfo = new(path);
            return Task.FromResult(directoryInfo.GetFiles().Select(x => x.Name).ToList());
        }

        private List<string> GetFilesFromDirectory(string directory)
        {
            var filesList = new List<string>();
            var files = Directory.GetFiles(directory);
            var directories = Directory.GetDirectories(directory);

            foreach (var file in files)
            {
                filesList.Add(file.Replace(_env.WebRootPath, "").Replace("\\", "/"));
            }

            foreach (var dir in directories)
            {
                filesList.AddRange(GetFilesFromDirectory(dir));
            }

            return filesList;
        }

        public async Task<Upload> UploadFileAsync(string containerName, IFormFile file)
        {
            string uploadPath = Path.Combine(_env.WebRootPath, containerName);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var newFileName = Guid.NewGuid() + file.FileName;
            var path = Path.Combine(uploadPath, newFileName);

            if (Path.GetExtension(file.FileName).ToLower() == ".svg")
            {
                await CopyFileAsync(path, file);
            }
            else
            {
                await CopyFileAsync(path, file);
                await CompressAndSaveImageAsync(file, path);
            }

            return new Upload
            {
                FileName = newFileName,
                Path = uploadPath
            };
        }

        private static async Task CompressAndSaveImageAsync(IFormFile file, string outputPath)
        {
            try
            {
                using var image = await SixLabors.ImageSharp.Image.LoadAsync(file.OpenReadStream());
                var extension = Path.GetExtension(file.FileName).ToLower();
                // Check the image format and set the appropriate encoder
                IImageEncoder encoder = extension switch
                {
                    ".jpg" or ".jpeg" => new JpegEncoder
                    {
                        Quality = 75 // Adjust the quality to reduce the file size
                    },
                    ".png" => new PngEncoder
                    {
                        CompressionLevel = PngCompressionLevel.BestCompression // Adjust the compression level
                    },
                    ".bmp" => new BmpEncoder(),
                    ".gif" => new GifEncoder(),
                    _ => throw new InvalidOperationException("Unsupported image format."),
                };

                // Save the image with the specified encoder
                await image.SaveAsync(outputPath, encoder);
            }
            catch (UnknownImageFormatException)
            {
                throw new InvalidOperationException("Unsupported image format.");
            }
        }


        private async Task MinifyAndSaveImageAsync(IFormFile file, string path)
        {
            try
            {
                using var image = await SixLabors.ImageSharp.Image.LoadAsync<Rgba32>(file.OpenReadStream());
                // Resize the image
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(800, 800) // Adjust the size as needed
                }));

                // Save the image with compression
                var encoder = new JpegEncoder
                {
                    Quality = 75 // Adjust the quality as needed (1-100)
                };

                await image.SaveAsync(path, encoder);
            }
            catch (UnknownImageFormatException)
            {
                throw new InvalidOperationException("Unsupported image format.");
            }
        }

        public async Task<List<Upload>> UploadFilesAsync(string containerName, IFormFileCollection files)
        {
            var uploadList = new List<Upload>();
            foreach (var file in files)
            {
                var uploadDTO = await UploadFileAsync(containerName, file);
                uploadList.Add(uploadDTO);
            }

            return uploadList;
        }

        static async Task<bool> CopyFileAsync(string filePath, IFormFile file)
        {
            try
            {
                await using FileStream fileStream = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: true);
                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                return true;
            }
            catch (Exception)
            {
                //todo log
                return false;
            }
        }
    }
}
