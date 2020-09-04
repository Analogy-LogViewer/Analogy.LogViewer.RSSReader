using Analogy.LogViewer.RSSReader.Core;
using Analogy.LogViewer.RSSReader.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Analogy.Interfaces;

namespace Analogy.LogViewer.RSSReader
{
    public class RSSComponentImages : IAnalogyComponentImages
    {
        public Image GetLargeImage(Guid analogyComponentId)
        {
            if (analogyComponentId == RSSFactory.rssFactoryId)
                return Resources.rss_icon_32x32;
            return null;
        }

        public Image GetSmallImage(Guid analogyComponentId)
        {
            if (analogyComponentId == RSSFactory.rssFactoryId)
                return Resources.rss_icon_16x16;
            return null;
        }

        public Image GetOnlineConnectedLargeImage(Guid analogyComponentId) => null;

        public Image GetOnlineConnectedSmallImage(Guid analogyComponentId) => null;

        public Image GetOnlineDisconnectedLargeImage(Guid analogyComponentId) => null;

        public Image GetOnlineDisconnectedSmallImage(Guid analogyComponentId) => null;
    }
}
