using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using WilliamsonFamily.Models.Web;
using WilliamsonFamily.Library.Web;
using WilliamsonFamily.Models.Content;

namespace WilliamsonFamily.Web.Web
{
    public static class Extensions
    {
		public static IContentRepository ContentRepository { get; set; }

        static ITitleCleaner titleCleaner;
        public static ITitleCleaner TitleCleaner
        {
            get
            {
                // I did this to make testing easier. It sucks
                if (titleCleaner == null) titleCleaner = new TitleCleaner();
                return titleCleaner;
            }
        }

        public static string BlogUrl(this string title)
        {
            return TitleCleaner.CleanTitle(title);
        }

        public static string EntrySummary(this string entry)
        {
            if (entry.Length <= 1000)
                return entry;
            else
            {
                return entry
                    .Substring(0, entry.IndexOf(" ", 1000));
            }
        }

        public static string Action(this RouteData data)
        {
            return (data.Values["action"] ?? "").ToString();
        }

        public static string Controller(this RouteData data)
        {
            return (data.Values["controller"] ?? "").ToString();
        }

        public static string FriendlyDate(this DateTime dateTime)
        {
            if (dateTime.Date == DateTime.Today.Date)
                return "Today";
            else if (dateTime.Date == DateTime.Today.AddDays(-1).Date)
                return "Yesterday";
            else if (DateTime.Today.AddDays(-3).Date >= dateTime.Date && DateTime.Today.AddDays(-7) <= dateTime.Date)
                return string.Format("{0:dddd}, {0:MMMM} {0:dd}", dateTime);
            else if (dateTime.Year == DateTime.Today.Year)
                return string.Format("{0:M}", dateTime);
            return string.Format("{0:MMMM} {0:dd}, {0:yyyy}", dateTime.Date);
        }

        public static string RenderPager(this HtmlHelper html, int pageCount, int pageIndex, string url)
        {
            StringBuilder sb = new StringBuilder();
            string urlFormat = "<a href=\"{0}?page={1}\">{2}</a>";
            string nonFormat = "<span class='currentPage'>{0}</span>";

            sb.Append("Page: ");
            if (pageIndex > 1)
                sb.Append(string.Format(urlFormat, url, pageIndex - 1, "Newer"));

            for (int i = 1; i <= pageCount; i++)
            {
                if (i == pageIndex)
                    sb.Append(string.Format(nonFormat, i));
                else
                    sb.Append(string.Format(urlFormat, url, i, i));
            }

            if (pageIndex < pageCount )
                sb.Append(string.Format(urlFormat, url, pageIndex + 1, "Older"));

            return sb.ToString();
        }

		public static string ResolveToken(this HtmlHelper html, string token)
		{
			var content = ContentRepository.Lookup(token);
			if (content == null)
				return "";

			return content.Value;
		}
    }
}
