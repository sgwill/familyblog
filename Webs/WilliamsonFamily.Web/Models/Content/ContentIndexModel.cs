using System;
using WilliamsonFamily.Models.Content;

namespace WilliamsonFamily.Web.Models.Content
{
	public class ContentIndexModel
	{
		public string Username { get; set; }
		public string FamilyName { get; set; }
		public IContent BlogSidebarBlurb { get; set; }
		public IContent BlogFamilySidebarBlurb { get; set; }
	}
}