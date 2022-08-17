using UserCollectionBlaz.Data;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Components.Forms;

namespace UserCollectionBlaz.Service
{
    public class CloudinaryService
    {
        public readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> options)
        {
            var acc = new Account(
                options.Value.CloudName,
                options.Value.ApiKey,
                options.Value.ApiSecret);
            _cloudinary = new Cloudinary(acc);
        }

            public async Task<ImageUploadResult> AddPhotoAsync(IBrowserFile file)
            {
                var uploadResult = new ImageUploadResult();
                if (file.Size > 0)
                {
                    using var stream = file.OpenReadStream();
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.Name, stream),
                    };
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }
                return uploadResult;
            }

            public async Task<DeletionResult> DeletePhotoAsync(string publicUrl)
            {
                var publicId = publicUrl.Split('/').Last().Split('.')[0];
                var deleteParams = new DeletionParams(publicId);
                return await _cloudinary.DestroyAsync(deleteParams);
            }
        }
    }

