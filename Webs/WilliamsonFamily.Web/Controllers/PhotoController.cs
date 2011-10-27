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

		[Route("{user}/photo/upload")]
		public ActionResult Upload()
		{
			string view = "UploadPhotoPartial";
			return View(view);
		}

		[ValidateInput(false)]
		[AcceptVerbs(HttpVerbs.Post)]
		[Route("{user}/photo/uploadphoto", HttpVerbs.Post)]
		public ActionResult UploadPhoto(HttpPostedFileBase theFile)
		{
			EnsureInjectables();

			IPhoto photo = null;
			photo = PhotoRepository.UploadPhoto(theFile.InputStream, theFile.FileName, "", "", "");

			return new FileUploadJsonResult()
			{
				Data = new { Url = photo.WebUrl, success = true, message = "success" }
			};

		}
	}
}