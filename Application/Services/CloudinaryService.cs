using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Application.InterfacesServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(account);
        }



        public async Task<string> UploadImageAsync(IFormFile file, string folderName = "centermanagement/images")
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File không hợp lệ");
            }

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation()
                    .Quality("auto")    // Tự động điều chỉnh chất lượng ảnh
                    .FetchFormat("auto") // Chuyển đổi định dạng ảnh sang WebP hoặc JPEG tối ưu
                    .Width(800)         // Giới hạn ảnh tối đa 800px
                    .Crop("limit"),      // Giữ nguyên tỉ lệ, chỉ giảm kích thước
                Folder = folderName // Chỉ định thư mục
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl.ToString(); // Trả về URL của ảnh
        }


        public async Task<bool> DeleteImageAsync(string publicId)
        {

            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);
            return result.Result == "ok";
        }

        public string GetPublicIdFromUrl(string imageUrl)
        {
            var uri = new Uri(imageUrl);
            var segments = uri.Segments;

            // Tìm vị trí "upload/" và lấy phần sau nó
            var publicIdParts = segments.SkipWhile(s => !s.Contains("upload/")).Skip(1).ToList();

            // Kiểm tra nếu phần đầu tiên sau "upload/" có dạng "v1234567890/"
            if (publicIdParts.Count > 0 && publicIdParts[0].StartsWith("v") && char.IsDigit(publicIdParts[0][1]))
            {
                publicIdParts.RemoveAt(0); // Xóa version (v1741893640)
            }
            // Ghép lại để tạo public_id
            var publicIdPath = string.Join("", publicIdParts);

            // Loại bỏ phần mở rộng ảnh (nếu có)
            var publicId = System.IO.Path.ChangeExtension(publicIdPath, null);

            return publicId;
        }

    }
}
