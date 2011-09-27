using System;
using System.Web.Mvc;
using System.ServiceModel.Syndication;
using System.Xml;

namespace WilliamsonFamily.Library.Web
{
    public class RssResult : ActionResult
    {
        private SyndicationFeed _feed;

        public RssResult(SyndicationFeed feed)
        {
            _feed = feed;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "application/rss+xml";

            Rss20FeedFormatter rssFormatter = new Rss20FeedFormatter(_feed);
            using (XmlWriter writer = XmlWriter.Create(context.HttpContext.Response.Output))
            {
                rssFormatter.WriteTo(writer);
            }
        }
    }
}