using System;
using System.Buffers.Text;
using System.IO;
using System.Linq;
using Flexify.Exceptions;
using Flexify.Models;
using Microsoft.Extensions.Hosting;

namespace Flexify.Services
{
    public class UploadImageService
    {
        private readonly string directory;
        private readonly string[] allowedExtension;

        public UploadImageService(IHostEnvironment environment)
        {
            directory = environment.ContentRootPath + "/Uploads/";
            allowedExtension = new string[] {"jpg", "png"};
        }

        public bool upload(Image image)
        {
            if (image is null)
                throw new UserException("Image cannot be null!");
            
            //check the image extension
            string[] fileInfo = image.FileName.Split(".");
            if (!allowedExtension.Contains(fileInfo[1]))
            {
                throw new UserException("Invalid file extension!");
            }

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            byte[] img = Convert.FromBase64String(image.File);
            
            // MemoryStream stream = new MemoryStream(img, 0, img.Length);
            // stream.Write(img, 0, img.Length);

            string imgPath = directory + image.FileName;
            
            using (var fs = new FileStream(imgPath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(img, 0, img.Length);
            }

            return File.Exists(imgPath);

        }
    }
}