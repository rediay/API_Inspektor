using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Inspektor_API_REST.Servicios.Files
{
    public class FileShare : IFileShare
    {
        private readonly IConfiguration _config;

        private string storageConnectionString, containerNameAditionalService, containerName;

        public FileShare(IConfiguration configuration)
        {
            _config = configuration;
            storageConnectionString = configuration.GetConnectionString("default");
            containerName = configuration.GetSection("FileShareDetails")["FileShareName"];
            containerNameAditionalService = configuration.GetSection("FileShareDetails")["FileShareNameAditionalService"];
            string aesKeyBase64 = configuration["FileShareDetails:AesKey"];
            string aesIVBase64 = configuration["FileShareDetails:AesIV"];

        }

        public async Task<String> FileDownloadAsync(decimal idConsultaReport)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference($"{idConsultaReport}.json");

            using (var memoryStream = new MemoryStream())
            {
                await blockBlob.DownloadToStreamAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                using (var dataReaded = new StreamReader(memoryStream))
                {
                    string jsonData = dataReaded.ReadToEnd();
                    return await Task.FromResult(jsonData);
                }
            }

        }

        public async Task FileUploadAsync(string jsonData, decimal isConsulta)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            CloudBlockBlob blockBlob = container.GetBlockBlobReference($"{isConsulta}.json");
            byte[] jsonByteData = Encoding.UTF8.GetBytes(jsonData);
            await blockBlob.UploadFromByteArrayAsync(jsonByteData, 0, jsonByteData.Length);
        }
    }
}