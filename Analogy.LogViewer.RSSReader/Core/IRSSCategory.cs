using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analogy.LogViewer.RSSReader.Core
{
    public interface IRSSCategory
    {
        string CategoryName { get; set; }
        int MinutesToUpdate { get; set; }
        int FeedsCount { get; }
        int ImageIndex { get; set; }
        bool Undeletable { get; set; }
        List<IRSSFeed> FeedsInCategory { get; }

        void AddFeed(IRSSFeed feed);
        void RemoveFeed(IRSSFeed feed);

    }
}
