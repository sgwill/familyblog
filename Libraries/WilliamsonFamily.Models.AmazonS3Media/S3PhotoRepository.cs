using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WilliamsonFamily.Models.Photo;
using System.IO;
using Amazon.S3.Transfer;
using System.Configuration;

namespace WilliamsonFamily.Models.AmazonS3Media
{
    public class S3PhotoRepository : IPhotoRepository
    {
        private string photoBucket = "WilliamsonFamilyBlog";

        public IPhoto UploadPhoto(Stream stream, string filename, string title, string descriptioSn, string tags)
        {
            TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();
            request.InputStream = stream;
            request.BucketName = photoBucket;
            request.Key = filename;
            request.CannedACL = Amazon.S3.Model.S3CannedACL.PublicRead;

            TransferUtility transferUtility = new TransferUtility(ConfigurationManager.AppSettings["AWSAccessKey"], ConfigurationManager.AppSettings["AWSSecretKey"]);            
            transferUtility.Upload(request);

            S3Photo photo = new S3Photo();
            photo.WebUrl = string.Format("http://s3.amazonaws.com/{0}/{1}", photoBucket, filename);
            photo.Title = filename;

            return photo;
        }
    }
}
