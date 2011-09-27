using System;
using System.IO;

namespace WilliamsonFamily.Models.Photo
{
    public interface IPhotoRepository
    {
        IPhoto UploadPhoto(Stream stream, string filename, string title, string descriptioSn, string tags);
    }
}