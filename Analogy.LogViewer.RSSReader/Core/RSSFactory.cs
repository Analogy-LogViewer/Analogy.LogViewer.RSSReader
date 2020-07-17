using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using System;
using System.Collections.Generic;

namespace Analogy.LogViewer.RSSReader.Core
{
    public class RSSFactory : IAnalogyFactory
    {
        internal static Guid rssFactoryId = new Guid("5BCE8AE3-D46D-4782-B4EE-A12B15E59648");
        public Guid FactoryId { get; } = rssFactoryId;
        public string Title { get; } = "RSS Reader";
        public IEnumerable<IAnalogyChangeLog> ChangeLog { get; } = ChangeLogList.GetChangeLog();
        public IEnumerable<string> Contributors { get; } = new List<string> { "Lior Banai" };
        public string About { get; } = "RSS Reader";
    }
}
