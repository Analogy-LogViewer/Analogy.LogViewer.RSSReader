using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using System;
using System.Collections.Generic;
using System.Drawing;
using Analogy.LogViewer.RSSReader.Properties;

namespace Analogy.LogViewer.RSSReader.Core
{
    public class RSSFactory : Template.PrimaryFactory
    {
        internal static Guid Id = new Guid("5BCE8AE3-D46D-4782-B4EE-A12B15E59648");
        public override Guid FactoryId { get; set; } = Id;
        public override string Title { get; set; } = "RSS Reader";
        public override IEnumerable<IAnalogyChangeLog> ChangeLog { get; set; } = ChangeLogList.GetChangeLog();
        public override Image LargeImage { get; set; } = Resources.AnalogyRSS32x32;
        public override Image SmallImage { get; set; } = Resources.AnalogyRSS16x16;
        public override IEnumerable<string> Contributors { get; set; } = new List<string> { "Lior Banai" };
        public override string About { get; set; } = "RSS Reader";
    }
}
