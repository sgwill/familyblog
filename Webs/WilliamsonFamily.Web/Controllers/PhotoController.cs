using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WilliamsonFamily.Models.Photo;
using WilliamsonFamily.Library.Exceptions;
using WilliamsonFamily.Library.Web;
using MvcMiniProfiler;
using WilliamsonFamily.Library.Web.Routing;

namespace WilliamsonFamily.Web.Controllers
{
    public class PhotoController : BaseController
    {
        #region Injectables
        public IPhotoRepository PhotoRepository { get; set; }

        private void EnsureInjectables()
        {
            if (PhotoRepository == null) throw new InjectablePropertyNullException("PhotoRepository");
        }
        #endregion

		[Route("{user}/photo/upload.aspx")]
        public ActionResult Upload()
        {
            string view = "UploadPhoto";
            if (IsAjaxRequest)
                view += "Partial";

            return View(view);
        }

        [ValidateInput(false)]
        [AcceptVerbs(HttpVerbs.Post)]
		[Route("{user}/photo/upload.aspx", HttpVerbs.Post)]
        public ActionResult Upload(HttpPostedFileBase theFile)
        {
            EnsureInjectables();

            IPhoto photo = null;
            using (profiler.Step("PhotoController.Upload.Post"))
            {
                photo = PhotoRepository.UploadPhoto(theFile.InputStream, theFile.FileName, "", "", "");
            }

            if (IsAjaxRequest)
                return new FileUploadJsonResult()
                {
                    Data = new { Url = photo.WebUrl, success = true, message = "success" }
                };

            return RedirectToAction("Upload");
        }
    }
}