using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WilliamsonFamily.Models.Photo;
using FlickrNet;
using System.IO;
using System.Configuration;
using MvcMiniProfiler;

namespace WilliamsonFamily.Models.FlickrPhoto
{
    public class FlickrPhotoRepository : IPhotoRepository
    {
        #region IPhotoRepository Members

        public IPhoto UploadPhoto(Stream stream, string filename, string title, string description, string tags)
        {
            using (MiniProfiler.Current.Step("FlickrPhotoRepository.UploadPhoto"))
            {
                Flickr fl = new Flickr();

                string authToken = (ConfigurationManager.AppSettings["FlickrAuth"] ?? "").ToString();
                if (string.IsNullOrEmpty(authToken))
                    throw new ApplicationException("Missing Flickr Authorization");

                fl.AuthToken = authToken;
                string photoID = fl.UploadPicture(stream, filename, title, description, tags, true, true, false,
                    ContentType.Photo, SafetyLevel.Safe, HiddenFromSearch.Visible);
                var photo = fl.PhotosGetInfo(photoID);
                var allSets = fl.PhotosetsGetList();
                var blogSet = allSets
                                .FirstOrDefault(s => s.Description == "Blog Uploads");
                if (blogSet != null)
                    fl.PhotosetsAddPhoto(blogSet.PhotosetId, photo.PhotoId);

                FlickrPhoto fphoto = new FlickrPhoto();
                fphoto.Description = photo.Description;
                fphoto.WebUrl = photo.MediumUrl;
                fphoto.Title = photo.Title;
                fphoto.Description = photo.Description;

                return fphoto;
            }
        }

        #endregion
    }
}