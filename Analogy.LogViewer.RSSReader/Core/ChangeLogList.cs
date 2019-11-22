using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analogy.Interfaces;

namespace Analogy.LogViewer.RSSReader.Core
{
    public static class ChangeLogList
    {
        public static IEnumerable<AnalogyChangeLog> GetChangeLog()
        {
            yield return new AnalogyChangeLog("Initial commit",AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2019, 11, 22));
        }
    }
}
