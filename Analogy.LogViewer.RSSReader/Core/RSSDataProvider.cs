using Analogy.DataProviders.Extensions;
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
    public class RSSDataProvider : IAnalogyDataProvidersFactory
    {
        public Guid FactoryId { get; } = RSSFactory.rssFactoryId;
        public string Title { get; } = "Analogy RSS Reader";
        public IEnumerable<IAnalogyDataProvider> DataProviders { get; } = new List<IAnalogyDataProvider> { new OnlineRSSReader() };

    }

    public class OnlineRSSReader : IAnalogyRealTimeDataProvider
    {
        public Task InitializeDataProviderAsync(IAnalogyLogger logger)
        {
            Featcher = new WebFetcher();
            return Task.CompletedTask;
        }

        public void MessageOpened(AnalogyLogMessage message)
        {
            //nop
        }

        public Guid ID { get; } = new Guid("01A17FA2-94F2-46A2-A80A-89AE4893C037");
        public string OptionalTitle { get; } = "Analogy RSS Reader";
        public IAnalogyOfflineDataProvider FileOperationsHandler { get; }
        public bool IsConnected { get; }
        public event EventHandler<AnalogyDataSourceDisconnectedArgs> OnDisconnected;
        public event EventHandler<AnalogyLogMessageArgs> OnMessageReady;
        public event EventHandler<AnalogyLogMessagesArgs> OnManyMessagesReady;

        private WebFetcher Featcher { get; set; }
        public Task<bool> CanStartReceiving() => Task.FromResult(true);
        private Task FeatcherTask;
        private RSSFeedsContainer RSSContainer = ComponentsContainer.Instance.RSSFeedsContainer;
        private AppSettings Settings = ComponentsContainer.Instance.AppSettings;

        public void StartReceiving()
        {
            FeatcherTask = Task.Factory.StartNew(async () =>
            {
                var feeds = RSSContainer.GetNonDisabledFeeds().ToList();
                if (feeds.Any())

                {
                    var posts = await Featcher.GetRSSItemsFromFeeds(feeds, false, true);
                    foreach (IRSSPost post in posts)
                    {
                        AnalogyLogMessage m = CreateAnalogyMessageFromPost(post);
                        OnMessageReady?.Invoke(this, new AnalogyLogMessageArgs(m, post.Url, post.Url, ID));
                    }
                }
                else
                {
                    await Task.Delay(Settings.AppRSSSetings.IntervalMinutes * 60 * 1000);
                }
            });
        }

        private AnalogyLogMessage CreateAnalogyMessageFromPost(IRSSPost post)
        {
            return new AnalogyLogMessage
            {
                Category = post.FeedName,
                Class = AnalogyLogClass.General,
                Date = post.Date ?? DateTime.MinValue,
                FileName = post.FeedName,
                Level = AnalogyLogLevel.Event,
                Source = post.Url,
                Text = post.Title + Environment.NewLine + post.Description
            };
        }

        public void StopReceiving()
        {
            //
        }


    }

    public class RSSUserSetting : IAnalogyDataProviderSettings
    {
        public Task SaveSettingsAsync()
        {
            AppSettings.SaveSettings(ComponentsContainer.Instance.AppSettings, false);
            return Task.CompletedTask;
        }

        public string Title { get; } = "Analogy RSS Feed Settings";
        public UserControl DataProviderSettings => new SettingsDialogUC(ComponentsContainer.Instance.RSSFeedsContainer, ComponentsContainer.Instance.AppSettings);
        public Guid FactoryId { get; set; } = RSSFactory.rssFactoryId;
        public Image SmallImage { get; } = Resources.AnalogyRSS16x16;
        public Image LargeImage { get; } = Resources.AnalogyRSS32x32Transparent;
    }
}
