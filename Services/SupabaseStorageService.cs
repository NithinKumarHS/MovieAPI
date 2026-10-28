using MovieAPI.Services.Interfaces;

namespace MovieAPI.Services
{
    public class SupabaseStorageService : ISupabaseStorageService
    {
        private readonly Supabase.Client _supabase;

        public SupabaseStorageService(Supabase.Client supabase)
        {   
            _supabase = supabase;
        }

        public async Task<string> UploadAsync(IFormFile file, string fileName)
        {
            var bucket = _supabase.Storage.From("movie-poster");

            using var memoryStream = new MemoryStream();

            await file.CopyToAsync(memoryStream);

            var fileBytes = memoryStream.ToArray();

            var filePath = await 
                bucket.Upload(
                    fileBytes,
                    fileName,
                    new Supabase.Storage.FileOptions
                    {
                        CacheControl = "3600",
                        Upsert = true
                    });


            var publicUrl =
                bucket.GetPublicUrl(fileName);

            return publicUrl;


        }
    }
}