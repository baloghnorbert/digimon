using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Core.Enums;
using Backend.Core.Variables;

namespace Backend.Core.Helper
{
    public static class ImageUploadHandler
    {
        public static async Task<string> SaveProfileImageAsync(IFormFile file)
        {
            if (file.Length < 1)
            {
                return null;
            }

            string result = string.Empty;

            string extension = file.FileName.Reverse().Split(".")[0].Reverse();
            string fileName = $"{GenerateName()}.{extension}";

            string subFolder = UploadedFileType.Profile.ToString().ToLower();

#if DEBUG
            result = $@"\{subFolder}\{fileName}";
#else
            result = $@"/{subFolder}/{fileName}";
#endif
            await SaveOnDisk(result, file, extension, 720, 405);

            return result;
        }

        public static async Task<string> SaveDocumentImageAsync(IFormFile file)
        {
            if (file.Length < 1)
            {
                return null;
            }

            string result = string.Empty;

            string extension = file.FileName.Reverse().Split(".")[0].Reverse();
            string fileName = $"{GenerateName()}.{extension}";

            string subFolder = UploadedFileType.Document.ToString().ToLower();

#if DEBUG
            string path = $@"{ServerPathEnviorement.Base()}\{subFolder}\{fileName}";
            result = $@"\{subFolder}\{fileName}";
#else
            string path = $@"{ServerPathEnviorement.Base()}/{subFolder}/{fileName}";
            result = $@"/{subFolder}/{fileName}";
#endif
            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                await file.CopyToAsync(fileStream);
            }

            return result;
        }

        public static async Task<string> SaveCompetitionImageAsync(IFormFile file)
        {
            if (file.Length < 1)
            {
                return null;
            }

            string result = string.Empty;

            string extension = file.FileName.Reverse().Split(".")[0].Reverse();
            string fileName = $"{GenerateName()}.{extension}";

            string subFolder = UploadedFileType.Image.ToString().ToLower();

#if DEBUG
            result = $@"\{subFolder}\{fileName}";
#else
            result = $@"/{subFolder}/{fileName}";
#endif
            await SaveOnDisk(result, file, extension);

            return result;
        }

        public static async Task DeleteFile(string file)
        {
            if (file.Length < 1)
            {
                return;
            }

#if DEBUG
            string path = $@"{ServerPathEnviorement.Base()}{file}"
                         .Replace(@"/", @"\")
                         .Replace(@"\\", @"\"); ;
#else
            string path = $@"{ServerEnviorement.BaseImageURL()}{file}";
#endif
            using (FileStream stream = new FileStream(path, FileMode.Truncate, FileAccess.Write, FileShare.Delete, 4096, true))
            {
                await stream.FlushAsync();
                File.Delete(path);
            }
        }

        private static async Task SaveOnDisk(string pathPart, IFormFile file, string extension, int width = 1920, int height = 1080)
        {

#if DEBUG
            string path = $@"{ServerPathEnviorement.Base()}\{pathPart}"
                         .Replace(@"/", @"\")
                         .Replace(@"\\", @"\");

#else
            string path = $@"{ServerPathEnviorement.Base()}/{pathPart}";
#endif

            Image image = null;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                image = Image.FromStream(memoryStream);
            }

            using (Bitmap bitmap = new Bitmap(image))
            {
                byte[] data = ImageCompressor.ScaleImage(bitmap, width, height, false);

                using (MemoryStream ms = new MemoryStream(data))
                {
                    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        ms.WriteTo(fs);
                    }
                }
            }
        }

        private static string GenerateName()
        {
            string[] data = Guid.NewGuid().ToString().Split("-");

            for (int i = 1; i < 4; i++)
            {
                data[i] += Generate(4);
            }

            string result = string.Join('-', data);

            return result;
        }

        private static string Generate(int lenght)
        {
            StringBuilder builder = new StringBuilder();
            Enumerable.Range(65, 26)
                      .Select(e => ((char)e).ToString())
                      .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                      .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
                      .OrderBy(e => Guid.NewGuid())
                      .Take(lenght)
                      .ToList().ForEach(e => builder.Append(e));

            return builder.ToString();
        }

        private static ImageFormat GetImageFormat(string format)
        {
            return (format.ToLower()) switch
            {
                "jpg" or "jpeg" => ImageFormat.Jpeg,
                "png" => ImageFormat.Png,
                "gif" => ImageFormat.Gif,
                _ => throw new Exception("Nem támogatott kép formátum."),
            };
        }
    }
}
