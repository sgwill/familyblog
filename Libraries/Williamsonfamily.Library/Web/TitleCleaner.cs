using System;
using WilliamsonFamily.Models.Web;

namespace WilliamsonFamily.Library.Web
{
    public class TitleCleaner : ITitleCleaner
    {
        #region ITitleCleaner Members

        public string CleanTitle(string title)
        {
            return title
                .Replace(":", "")
                .Replace("/", "")
                .Replace("?", "")
                .Replace("#", "")
                .Replace("[", "")
                .Replace("]", "")
                .Replace("@", "")
                .Replace("*", "")
                .Replace(".", "")
                .Replace(",", "")
                .Replace("\\", "")
                .Replace("&", "")
                .Replace("'", "")
                .Replace(" ", "-")
                .Replace("--", "-")
                .Replace("\"", "")
                .ToLower();
        }

        #endregion
    }
}
