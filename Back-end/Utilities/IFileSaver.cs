namespace Back_end.Utilities
{
    public interface IFileSaver
    {
        Task DeleteFile(string path, string container);
        Task<string> SaveFile(string container, IFormFile file);
        Task<string> UpdateFile(string container, IFormFile file, string path);
    }
}