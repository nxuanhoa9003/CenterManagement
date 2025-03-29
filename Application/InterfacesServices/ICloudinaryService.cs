using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.InterfacesServices
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile file, string folderName = "centermanagement/images");
        Task<bool> DeleteImageAsync(string publicId);
        string GetPublicIdFromUrl(string imageUrl);
    }
}
