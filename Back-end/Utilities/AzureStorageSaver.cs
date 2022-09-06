using Azure.Storage.Blobs;

namespace Back_end.Utilities
{
    public class AzureStorageSaver : IFileSaver
    {
        private string connectionString;
        public AzureStorageSaver(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("AzureStorage");
        }

        public async Task<string> SaveFile(string container, IFormFile file)
        {
            var client = new BlobContainerClient(connectionString, container);
            await client.CreateIfNotExistsAsync();
            client.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var blob = client.GetBlobClient(fileName);
            await blob.UploadAsync(file.OpenReadStream());
            return blob.Uri.ToString();
        }

        public async Task DeleteFile(string path, string container)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            var client = new BlobContainerClient(connectionString, container);
            await client.CreateIfNotExistsAsync();
            var file = Path.GetFileName(path);
            var blob = client.GetBlobClient(file);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string> UpdateFile(string container, IFormFile file, string path)
        {
            await DeleteFile(path, container);
            return await SaveFile(container, file);
        }
    }
}
