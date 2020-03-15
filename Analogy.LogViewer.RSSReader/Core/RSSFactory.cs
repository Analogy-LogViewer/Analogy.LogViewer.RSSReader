using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Analogy.Interfaces.Factories;

namespace Analogy.LogViewer.RSSReader.Core
{
    public class RSSFactory : IAnalogyFactory
    {
        internal static Guid rssFactoryId= new Guid("5BCE8AE3-D46D-4782-B4EE-A12B15E59648");
        public Guid FactoryID { get; } = rssFactoryId;
        public string Title { get; } = "Analogy RSS Reader";
        public IAnalogyDataProvidersFactory DataProviders { get; } = new RSSDataProvider();
        public IAnalogyCustomActionsFactory Actions { get; } = new EmptyActionsFactory();
        public IEnumerable<IAnalogyChangeLog> ChangeLog { get; } = ChangeLogList.GetChangeLog();
        public IEnumerable<string> Contributors { get; } = new List<string> { "Lior Banai" };
        public string About { get; } = "Analogy RSS Reader";
    }
}
