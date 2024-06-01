using Microsoft.IdentityModel.Tokens;

namespace Finanzauto.FileRoot
{
    public interface IUploadFileRepository
    {
        Task<string> UploadFilesAsync(IFormFile formFile, string nameFolder);
        Task<string> SaveFileAsync(byte[] file, string contentType, string extension, string container, string nameFile);
        Task DeleteFile(string ruta, string container);
        string GetUrlBase(string relativePath);

    }


    public class UploadFileRepository : IUploadFileRepository
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UploadFileRepository(IWebHostEnvironment hostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _hostEnvironment = hostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task DeleteFile(string ruta, string container)
        {
            string wwwrootPath = _hostEnvironment.WebRootPath;
            if (string.IsNullOrEmpty(wwwrootPath))
            {
                throw new ArgumentException();
            }

            var nombreArchivo = Path.GetFileName(ruta);
            string rutaFinal = Path.Combine(wwwrootPath, container, nombreArchivo);

            if (File.Exists(rutaFinal))
            {
                File.Delete(rutaFinal);
            }

            return Task.CompletedTask;
        }

        public string GetUrlBase(string relativePath)
        {
            string path = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            string urlActual = relativePath.IsNullOrEmpty() ? $"{Path.Combine(path, "images/noimage.png").Replace("\\", "/")}" : Path.Combine(path, relativePath).Replace("\\", "/");

            return urlActual;
        }

        public async Task<string> SaveFileAsync(byte[] file, string contentType, string extension, string container, string nameFile)
        {
            string wwwrootPath = _hostEnvironment.WebRootPath;
            if (string.IsNullOrEmpty(wwwrootPath))
            {
                throw new ArgumentException();
            }

            string carpetaArchivo = Path.Combine(wwwrootPath, container);

            if (!Directory.Exists(carpetaArchivo))
            {
                Directory.CreateDirectory(carpetaArchivo);//SI NO EXISTE LA CARPETA SE CREA
            }

            string nameFileEnd = $"{nameFile}{extension}";

            string rutaFinal = Path.Combine(carpetaArchivo, nameFileEnd);

            await File.WriteAllBytesAsync(rutaFinal, file);



            //string url = Path.Combine(urlActual, container, nameFileEnd).Replace("\\", "/");
            string relativePath = Path.Combine(container, nameFileEnd);

            return relativePath;

        }

        public async Task<string> UploadFilesAsync(IFormFile formFile, string nameFolder)
        {
            using var stream = new MemoryStream();

            await formFile.CopyToAsync(stream);

            var fileBytes = stream.ToArray();
            string contentType = formFile.ContentType;
            string extension = Path.GetExtension(formFile.FileName);
            string nameFile = Guid.NewGuid().ToString();

            string wwwrootPath = _hostEnvironment.WebRootPath;

            if (string.IsNullOrEmpty(wwwrootPath))
            {
                throw new ArgumentException();
            }

            string carpetaArchivo = Path.Combine(wwwrootPath, nameFolder);

            if (!Directory.Exists(carpetaArchivo))
            {
                Directory.CreateDirectory(carpetaArchivo);//SI NO EXISTE LA CARPETA SE CREA
            }

            string nameFileEnd = $"{nameFile}{extension}";

            string rutaFinal = Path.Combine(carpetaArchivo, nameFileEnd);

            await File.WriteAllBytesAsync(rutaFinal, fileBytes);

            string relativePath = Path.Combine(nameFolder, nameFileEnd);

            return relativePath;  // Return the relative path instead of the full URL
        }

    }
}
