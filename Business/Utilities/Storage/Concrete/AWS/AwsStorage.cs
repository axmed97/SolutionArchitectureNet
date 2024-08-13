using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Business.Utilities.Storage.Abstract.AWS;
using Entities.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace Business.Utilities.Storage.Concrete.AWS
{
    public class AwsStorage : Storage, IAwsStorage
    {
        private readonly IConfiguration _configuration;
        private readonly AmazonS3Client _amazonS3Client;
        public AwsStorage(IConfiguration configuration)
        {
            _configuration = configuration;
            _amazonS3Client = new AmazonS3Client(_configuration["AWS:AccessKey"], _configuration["AWS:SecretKey"], RegionEndpoint.EUNorth1);
        }


        public async Task DeleteAsync(string pathOrContainerName, string fileName)
        {
            var transferUtility = new TransferUtility(_amazonS3Client);
            await transferUtility.S3Client.DeleteObjectAsync(new DeleteObjectRequest()
            {
                BucketName = pathOrContainerName,
                Key = fileName
            });
        }

        public async Task<List<string>> GetAllFilesAsync(string? pathOrContainerName = null)
        {
            var files = new List<string>();
            try
            {
                ListObjectsV2Request request = new()
                {
                    BucketName = pathOrContainerName
                };
                ListObjectsV2Response response = new();
                do
                {
                    response = await _amazonS3Client.ListObjectsV2Async(request);

                    foreach (var item in response.S3Objects)
                    {
                        files.Add($"https://{pathOrContainerName}.s3.eu-north-1.amazonaws.com/" + item.Key);
                    }
                    request.ContinuationToken = response.NextContinuationToken;
                } while (response.IsTruncated);
            }
            catch (Exception)
            {
                throw;
            }

            return files;
        }

        public async Task<Upload> UploadFileAsync(string containerName, IFormFile file)
        {
            try
            {
                Upload upload = new();
                string key = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string compressedFilePath = Path.GetTempFileName();

                // Compress the image
                using (var inputStream = file.OpenReadStream())
                using (var outputStream = new FileStream(compressedFilePath, FileMode.Create))
                {
                    using Image image = Image.Load(inputStream);
                    // Adjust the quality here, 75 is an example
                    var encoder = new JpegEncoder { Quality = 75 };
                    image.Save(outputStream, encoder);
                }

                // Upload the compressed file
                using (var fileStream = new FileStream(compressedFilePath, FileMode.Open, FileAccess.Read))
                {
                    var fileTransferUtility = new TransferUtility(_amazonS3Client);
                    await fileTransferUtility.UploadAsync(fileStream, containerName, key);
                }

                // Clean up the temporary file
                if (System.IO.File.Exists(compressedFilePath))
                {
                    System.IO.File.Delete(compressedFilePath);
                }

                upload.FileName = key;
                upload.Path = $"https://{containerName}.s3.eu-north-1.amazonaws.com/{key}";
                return upload;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Upload>> UploadFilesAsync(string containerName, IFormFileCollection files)
        {
            List<Upload> uploads = new();
            foreach (var file in files)
            {
                var response = await UploadFileAsync(containerName, file);
                uploads.Add(response);
            }
            return uploads;
        }
    }
}
