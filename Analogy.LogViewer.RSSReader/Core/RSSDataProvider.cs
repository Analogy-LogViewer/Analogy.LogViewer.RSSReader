﻿using Analogy.Interfaces;
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
        public Guid FactoryId { get; set; } = RSSFactory.rssFactoryId;
        public string Title { get; set; } = "RSS Reader";
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
        public bool UseCustomColors { get; set; } = false;
        public IEnumerable<(string originalHeader, string replacementHeader)> GetReplacementHeaders()
            => Array.Empty<(string, string)>();

        public (Color backgroundColor, Color foregroundColor) GetColorForMessage(IAnalogyLogMessage logMessage)
            => (Color.Empty, Color.Empty);
        public Guid Id { get; set; } = new Guid("01A17FA2-94F2-46A2-A80A-89AE4893C037");

        public Image ConnectedLargeImage { get; set; } = null;
        public Image ConnectedSmallImage { get; set; } = null;
        public Image DisconnectedLargeImage { get; set; } = null;
        public Image DisconnectedSmallImage { get; set; } = null;
        public string OptionalTitle { get; set; } = "RSS Reader";
        public IAnalogyOfflineDataProvider FileOperationsHandler { get; }
        public event EventHandler<AnalogyDataSourceDisconnectedArgs> OnDisconnected;
        public event EventHandler<AnalogyLogMessageArgs> OnMessageReady;
        public event EventHandler<AnalogyLogMessagesArgs> OnManyMessagesReady;

        private WebFetcher Featcher { get; set; }
        public Task<bool> CanStartReceiving() => Task.FromResult(true);
        private RSSFeedsContainer RSSContainer = ComponentsContainer.Instance.RSSFeedsContainer;
        private AppSettings Settings = ComponentsContainer.Instance.AppSettings;

        public async Task StartReceiving()
        {

            var feeds = RSSContainer.GetNonDisabledFeeds().ToList();
            if (feeds.Any())

            {
                var posts = await Featcher.GetRSSItemsFromFeeds(feeds, false, true);
                foreach (IRSSPost post in posts)
                {
                    AnalogyLogMessage m = CreateAnalogyMessageFromPost(post);
                    OnMessageReady?.Invoke(this, new AnalogyLogMessageArgs(m, post.Url, post.Url, Id));
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

        public Task StopReceiving() => Task.CompletedTask;



    }

    public class RSSUserSetting : IAnalogyDataProviderSettings
    {
        public Task SaveSettingsAsync()
        {
            AppSettings.SaveSettings(ComponentsContainer.Instance.AppSettings, false);
            return Task.CompletedTask;
        }

        public string Title { get; set; } = "RSS Feed Settings";
        public UserControl DataProviderSettings => new SettingsDialogUC(ComponentsContainer.Instance.RSSFeedsContainer, ComponentsContainer.Instance.AppSettings);
        public Guid FactoryId { get; set; } = RSSFactory.rssFactoryId;
        public Guid Id { get; set; } = new Guid("5543D343-26B1-42FC-889D-A573202A2D35");
        public Image SmallImage { get; set; } = Resources.AnalogyRSS16x16;
        public Image LargeImage { get; set; } = Resources.AnalogyRSS32x32Transparent;
    }
}
