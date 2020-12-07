using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using Analogy.LogViewer.RSSReader.Properties;
using Analogy.LogViewer.RSSReader.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Analogy.LogViewer.RSSReader.Core
{
    public class RSSDataProvider : Template.DataProvidersFactory
    {
        public override Guid FactoryId { get; set; } = RSSFactory.Id;
        public override string Title { get; set; } = "RSS Reader";
        public override IEnumerable<IAnalogyDataProvider> DataProviders { get; set; } = new List<IAnalogyDataProvider> { new OnlineRSSReader() };
    }

    public class OnlineRSSReader : Template.OnlineDataProvider
    {
        public override Task InitializeDataProviderAsync(IAnalogyLogger logger)
        {
            Featcher = new WebFetcher();
            return base.InitializeDataProviderAsync(logger);
        }


        public override Guid Id { get; set; } = new Guid("01A17FA2-94F2-46A2-A80A-89AE4893C037");

        public override string OptionalTitle { get; set; } = "RSS Reader";
        public override IAnalogyOfflineDataProvider FileOperationsHandler { get; set; }

        private WebFetcher Featcher { get; set; }
        public override Task<bool> CanStartReceiving() => Task.FromResult(true);
        private RSSFeedsContainer RSSContainer = ComponentsContainer.Instance.RSSFeedsContainer;
        private AppSettings Settings = ComponentsContainer.Instance.AppSettings;

        public override async Task StartReceiving()
        {

            var feeds = RSSContainer.GetNonDisabledFeeds().ToList();
            if (feeds.Any())

            {
                var posts = await Featcher.GetRSSItemsFromFeeds(feeds, false, true);
                foreach (IRSSPost post in posts)
                {
                    AnalogyLogMessage m = CreateAnalogyMessageFromPost(post);
                    MessageReady(this, new AnalogyLogMessageArgs(m, post.Url, post.Url, Id));
                }
            }
            else
            {
                await Task.Delay(Settings.AppRSSSetings.IntervalMinutes * 60 * 1000);
            }
        }

        private AnalogyLogMessage CreateAnalogyMessageFromPost(IRSSPost post)
        {
            return new AnalogyLogMessage
            {
                Category = post.FeedName,
                Class = AnalogyLogClass.General,
                Date = post.Date ?? DateTime.MinValue,
                FileName = post.FeedName,
                Level = AnalogyLogLevel.Information,
                Source = post.Url,
                Text = post.Title + Environment.NewLine + post.Description
            };
        }

        public override Task StopReceiving() => Task.CompletedTask;

    }

    public class RSSUserSetting : Template.UserSettingsFactory
    {
        public override Task SaveSettingsAsync()
        {
            AppSettings.SaveSettings(ComponentsContainer.Instance.AppSettings, false);
            return Task.CompletedTask;
        }

        public override string Title { get; set; } = "RSS Feed Settings";
        public override UserControl DataProviderSettings { get; set; }= new SettingsDialogUC(ComponentsContainer.Instance.RSSFeedsContainer, ComponentsContainer.Instance.AppSettings);
        public override Guid FactoryId { get; set; } = RSSFactory.Id;
        public override Guid Id { get; set; } = new Guid("5543D343-26B1-42FC-889D-A573202A2D35");
        public override Image SmallImage { get; set; } = Resources.AnalogyRSS16x16;
        public override Image LargeImage { get; set; } = Resources.AnalogyRSS32x32Transparent;
    }
}
