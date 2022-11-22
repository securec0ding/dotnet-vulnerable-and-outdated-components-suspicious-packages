using Ionic.Zip;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;

namespace OldDependency.Services
{
    public class ImageService : IImageService
    {
        private readonly string uploadDirectoryPath;

        public ImageService(IHostingEnvironment environment)
        {
            this.uploadDirectoryPath = Path.Combine(environment.WebRootPath, "gallery");
        }

        public void ExtractAndStoreImages(string userName, IFormFile fileFromUser)
        {
            if (fileFromUser == null)
                return;
            if (fileFromUser.Length == 0)
                return;

            using (var stream = fileFromUser.OpenReadStream())
            {
                using (var zipFile = ZipFile.Read(stream))
                {
                    foreach (var entry in zipFile.Entries)
                    {
                        var usersImageDirectoryPath = Path.Combine(this.uploadDirectoryPath, userName);
                        entry.Extract(usersImageDirectoryPath);
                    }
                }
            }
        }

        public Dictionary<string, List<string>> GetAll()
        {
            var users2images = new Dictionary<string, List<string>>();

            var uploadDirectory = new DirectoryInfo(this.uploadDirectoryPath);
            var userDirectories = uploadDirectory.GetDirectories();
            foreach(var userDir in userDirectories)
            {
                var userName = userDir.Name;
                users2images.Add(userName, new List<string>());
                var userImages = userDir.GetFiles();
                foreach(var image in userImages)
                {
                    users2images[userName].Add(image.Name);
                }
            }

            return users2images;
        }
    }

}