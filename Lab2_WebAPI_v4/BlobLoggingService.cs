using Azure.Storage.Blobs;
using System.Text;

namespace Lab2_WebAPI_v4
{
    public class BlobLoggingService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IConfiguration _config;

        public BlobLoggingService(BlobServiceClient blobServiceClient, IConfiguration config)
        {
            _blobServiceClient = blobServiceClient;
            _config = config;
        }

        public async Task LogAsync(string message)
        {
            var containerName = _config["Storage:ContainerName"] ?? "logs";

            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            string fileName = $"log-{DateTime.UtcNow:yyyy-MM-dd-HH-mm-ss-fff}.txt";
            var blobClient = containerClient.GetBlobClient(fileName);

            var content = $"{DateTime.UtcNow:O}: {message}";
            var bytes = Encoding.UTF8.GetBytes(content);

            using var stream = new MemoryStream(bytes);

            await blobClient.UploadAsync(stream, overwrite: false);
        }
    }
}