using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analogy.LogViewer.RSSReader.Core
{
    public class ComponentsContainer
    {
        private static Lazy<ComponentsContainer> _instance = new Lazy<ComponentsContainer>(() => new ComponentsContainer());
        public static ComponentsContainer Instance { get; } = _instance.Value;

        public AppSettings AppSettings { get; }
        public RSSFeedsContainer RSSFeedsContainer { get; }
        public ComponentsContainer()
        {
            bool aggregatorPerUser = Properties.Settings.Default.StorePerUser;
            AppSettings = AppSettings.LoadSettings(aggregatorPerUser, false);
            RSSFeedsContainer = RSSFeedsContainer.DeSerializeBinaryFile(AppSettings.AppRSSSetings.FileName);
        }
    }
}
