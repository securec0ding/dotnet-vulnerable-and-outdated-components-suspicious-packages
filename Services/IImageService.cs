using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace OldDependency.Services
{
    public interface IImageService
    {
        void ExtractAndStoreImages(string userName, IFormFile fileFromUser);
        Dictionary<string, List<string>> GetAll();
    }
}