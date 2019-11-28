using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Analogy.LogViewer.RSSReader.Core
{
    [Serializable]
    public class StandardRSSPost : AbstractRSSPost
    {
        private enum RSStags
        {
            title,
            pubDate,
            link,
            guid,
            description,
            creator,
            content,

        }
        public StandardRSSPost(XContainer post, IRSSFeed feed)
        {
            BelongsToFeed = feed;
            // Get the string properties from the post's element values
            Title = GetElementValue(post, "title");
            Link = GetElementValue(post, "link");
            Url = GetElementValue(post, "guid");
            Description = GetElementValue(post, "description");
            Creator = GetElementValue(post,
                "{http://purl.org/rss/elements/1.1/}");
            Content = GetElementValue(post,
                "{http://purl.org/rss/1.0/modules/content/}");


            // The Date property is a nullable DateTime? -- if the pubDate element
            // can't be parsed into a valid date, the Date property is set to null
            DateTime result;
            if (DateTime.TryParse(GetElementValue(post, "pubDate"), out result))
                Date = (DateTime?)result;

            Read = false;
            AddedDate = DateTime.Now;
            IgnorePostContentInComparison = feed.DontKeepHistory;
        }

        public StandardRSSPost(string title, string url, string link, string description, string creator, string content,
                         DateTime? pubDate, IRSSFeed feed)
        {
            Title = title ?? string.Empty;
            Url = url ?? string.Empty;
            Link = link ?? string.Empty;
            Description = description ?? string.Empty;
            Creator = creator ?? string.Empty;
            Content = content ?? string.Empty;
            Date = pubDate;
            Read = false;
            BelongsToFeed = feed;
            AddedDate = DateTime.Now;
            IgnorePostContentInComparison = feed.DontKeepHistory;
        }

        private static string GetElementValue(XContainer element, string name)
        {
            //if (element == null) return string.Empty;
            //var result = (element.Elements().FirstOrDefault(e => e.Name.Contains(name)));
            //return result != null ? result.Value : string.Empty;

            if (element?.Element(name) == null)
                return string.Empty;
            return element.Element(name)?.Value;
        }



    }
}
