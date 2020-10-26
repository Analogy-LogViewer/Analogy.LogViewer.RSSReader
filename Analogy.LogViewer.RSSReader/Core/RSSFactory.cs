using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using System;
using System.Collections.Generic;
using System.Drawing;
using Analogy.LogViewer.RSSReader.Properties;

namespace Analogy.LogViewer.RSSReader.Core
{
    public class RSSFactory : IAnalogyFactory
    {
        internal static Guid Id = new Guid("5BCE8AE3-D46D-4782-B4EE-A12B15E59648");
        public void RegisterNotificationCallback(INotificationReporter notificationReporter)
        {
            
        }

        public Guid FactoryId { get; set; } = Id;
        public string Title { get; set; } = "RSS Reader";
        public IEnumerable<IAnalogyChangeLog> ChangeLog { get; set; } = ChangeLogList.GetChangeLog();
        public Image LargeImage { get; set; } = Resources.AnalogyRSS32x32;
        public Image SmallImage { get; set; } = Resources.AnalogyRSS16x16;
        public IEnumerable<string> Contributors { get; set; } = new List<string> { "Lior Banai" };
        public string About { get; set; } = "RSS Reader";
    }
}
