using System.ComponentModel;

namespace MinimalAPIPeliculas.Services
{
    public interface IFileStorer
    {
        Task delete(string? path, string container);
        Task<string> store(string container, IFormFile file);
        async Task<string> Edit (string? path, string container, IFormFile file)
        {
            await delete(path, container);
            return await store(container, file);
        }
    }
}
