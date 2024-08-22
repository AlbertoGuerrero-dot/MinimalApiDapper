
using System.IO;

namespace MinimalAPIPeliculas.Services
{
    public class LocalFileStorer : IFileStorer
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor;

        public LocalFileStorer(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor )
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
        }
        public Task delete(string path, string container)
        {
            if (string.IsNullOrEmpty(path))
            {
                return Task.CompletedTask;
            }
            var fileName = Path.GetFileName(path);
            var filedirectory = Path.Combine(env.WebRootPath, container, fileName);

            if ( File.Exists(filedirectory)) 
            {
                File.Delete(filedirectory);
            }
            return Task.CompletedTask;  
        }

        public async Task<string> store(string container, IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(env.WebRootPath, container);
            if (!Directory.Exists(folder)) 
            {
                Directory.CreateDirectory(folder);
            }
            string path = Path.Combine(folder, fileName);
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                var content = ms.ToArray();
                await File.WriteAllBytesAsync(path, content); 
            }
            var url = $"{httpContextAccessor.HttpContext!.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}"; 
            var fileUrl = Path.Combine(url, container, fileName).Replace("\\", "/"); 
            return fileUrl;
        }
    }
}
