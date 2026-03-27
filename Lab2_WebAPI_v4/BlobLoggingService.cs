using Azure.Storage.Blobs;
using System.Text;

namespace Lab2_WebAPI_v4
{
    public class BlobLoggingService
    {
        private readonly string _connectionString;

        public BlobLoggingService(IConfiguration config)
        {
            _connectionString = config["Storage:ConnectionString"];
        }

        public async Task LogAsync(string message)
        {
            var containerClient = new BlobContainerClient(_connectionString, "logs");
            await containerClient.CreateIfNotExistsAsync();

            string fileName = $"log-{DateTime.UtcNow:yyyy-MM-dd-HH-mm-ss-fff}.txt";
            var blobClient = containerClient.GetBlobClient(fileName);

            var content = $"{DateTime.UtcNow}: {message}";
            var bytes = Encoding.UTF8.GetBytes(content);

            using var stream = new MemoryStream(bytes);

            await blobClient.UploadAsync(stream, overwrite: false);
        }
    }
}
