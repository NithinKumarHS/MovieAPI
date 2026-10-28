namespace MovieAPI.Services.Interfaces
{
    public interface ISupabaseStorageService
    {
        Task<string> UploadAsync(IFormFile file, string fileName);
    }
}
