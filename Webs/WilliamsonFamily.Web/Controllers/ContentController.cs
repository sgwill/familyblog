using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WilliamsonFamily.Library.Web.Routing;
using WilliamsonFamily.Models.Content;
using WilliamsonFamily.Models.Family;
using WilliamsonFamily.Models.User;
using WilliamsonFamily.Web.Models.Content;

namespace WilliamsonFamily.Web.Controllers
{
	[Authorize]
    public class ContentController : BaseController
    {
		public IContentRepository ContentRepository { get; set; }
		public IUserRepository UserRepository { get; set; }
		public IFamilyRepository FamilyRepository { get; set; }

		[AcceptVerbs(HttpVerbs.Get)]
		[Route("{user}/content")]
		public ActionResult Index(string user)
		{
			if (user.ToLower() != HttpContext.User.Identity.Name.ToLower())
				return RedirectToAction("Index", "Content", new { user = HttpContext.User.Identity.Name });

			var dbUser = UserRepository.Load(user);
			var family = FamilyRepository.LoadFromUsername(dbUser.Username);

			var model = new ContentIndexModel();
			string sidebarblurb = "{0}.Blog.SidebarBlurb";
			model.Username = dbUser.Username;
			model.BlogSidebarBlurb = ContentRepository.Lookup(string.Format(sidebarblurb, model.Username));
			if (model.BlogSidebarBlurb == null)
			{
				model.BlogSidebarBlurb = ContentRepository.New();
				model.BlogSidebarBlurb.Token = string.Format(sidebarblurb, model.Username);
			}

			if (family != null)
			{
				model.FamilyName = family.FamilyName;
				model.BlogFamilySidebarBlurb = ContentRepository.Lookup(string.Format("{0}.Blog.SidebarBlurb", model.FamilyName));
				if (model.BlogFamilySidebarBlurb == null)
				{
					model.BlogFamilySidebarBlurb = ContentRepository.New();
					model.BlogFamilySidebarBlurb.Token = string.Format(sidebarblurb, model.FamilyName);
				}
			}

			return View(model);
		}

		[ValidateInput(false)]
		[AcceptVerbs(HttpVerbs.Post)]
		[Route("{user}/content/edit")]
		public ActionResult Edit(string user, ContentEditModel model)
		{
			if (model != null && !string.IsNullOrEmpty(model.Token))
			{
				// todo: automapper
				var content = ContentRepository.New();
				content.ContentID = model.ContentID;
				content.Token = model.Token;
				content.Value = model.Value;
			
				ContentRepository.Set(content);
			}
				
			return RedirectToAction("Index", new { user = user });
		}
    }
}