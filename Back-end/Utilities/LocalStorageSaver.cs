
using Back_end.Utilities;

namespace PeliculasAPI.Utilidades
{
    public class LocalStorageSaver : IFileSaver
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor;

        public LocalStorageSaver(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> SaveFile(string container, IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(env.WebRootPath, container);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string path = Path.Combine(folder, fileName);
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();
                await File.WriteAllBytesAsync(path, content);
            }

            var currentURL = $"{httpContextAccessor.HttpContext?.Request.Scheme}://{httpContextAccessor.HttpContext?.Request.Host}";
            var dbPath = Path.Combine(currentURL, container, fileName).Replace("\\", "/");
            return dbPath;
        }

        public Task DeleteFile(string path, string container)
        {
            if (string.IsNullOrEmpty(path))
            {
                return Task.CompletedTask;
            }

            var fileName = Path.GetFileName(path);
            var fileDirectory = Path.Combine(env.WebRootPath, container, fileName);

            if (File.Exists(fileDirectory))
            {
                File.Delete(fileDirectory);
            }
            return Task.CompletedTask;
        }

        public async Task<string> UpdateFile(string container, IFormFile file, string path)
        {
            await DeleteFile(path, container);
            return await SaveFile(container, file);
        }
    }
}