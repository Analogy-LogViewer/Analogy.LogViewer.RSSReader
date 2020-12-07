using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Analogy.LogViewer.RSSReader.Core;
using Analogy.LogViewer.RSSReader.Properties;

namespace Analogy.LogViewer.RSSReader
{
    public class WebFetcher
    {
        private enum DisplayFeedsType
        {
            All,
            Active,
            NonActive,
            Privates,
            Disabled,
            NotInCategory,
            InSomeCategory
        }
        public static List<RSSFeedsContainer> FeedsGroup { get; private set; }
        private bool BusyRefreshingFromWeb { get; set; }
        private int RSSRefreshCount { get; set; }
        private string DisplayFeedsName { get; set; }
        public string Status { get; set; }
        private List<IRSSFeed> DisplayFeeds { get; set; }
        private ulong TotalDownloadedKB { get; set; }
        private AppSettings Settings { get; } = ComponentsContainer.Instance.AppSettings;
        private List<IRSSPost> DisplayPosts { get; set; }
        public event EventHandler<LogArgs> OnLogOperation = delegate { };
        public event EventHandler<LogArgs> OnCurrentFeedsChanged = delegate { };
        public event EventHandler<RSSArgs> OnPostChanged = delegate { };
        public event EventHandler<LogArgs> OnWebRefresh = delegate { };
        public event EventHandler<LogArgs> OnWebEndRefresh = delegate { };

        public WebFetcher()
        {
            FeedsGroup = new List<RSSFeedsContainer> { ComponentsContainer.Instance.RSSFeedsContainer };
        }
        private void RefreshRSS(bool activeFeeds)
        {

            if (!BusyRefreshingFromWeb)
            {

                if (activeFeeds)
                {
                    RSSRefreshCount++;
                    SetDispalyFeeds(DisplayFeedsType.Active, null);
                }
                else
                {
                    SetDispalyFeeds(DisplayFeedsType.NonActive, null);
                }

                GetRSSItemsFromFeeds(DisplayFeeds, true, true);
            }
            else
            {
                Status = "In progress";
            }
            Status = "Number of RSS refreshes: " + RSSRefreshCount;
        }
        private void SetDispalyFeeds(DisplayFeedsType type, IRSSCategory category)
        {
            RSSFeedsContainer feeds = FeedsGroup.FirstOrDefault();
            if (feeds != null)
            {
                switch (type)
                {
                    case DisplayFeedsType.All:
                        DisplayFeeds = feeds.GetFeeds().Where(feed => !feed.Disabled).ToList();
                        DisplayFeedsName = "All Feeds";
                        break;
                    case DisplayFeedsType.Active:
                        DisplayFeeds = (from feed in feeds.GetFeeds()
                                        where (feed.Active && !feed.Disabled)
                                        select feed).ToList();
                        DisplayFeedsName = "All Active Feeds";
                        break;
                    case DisplayFeedsType.NonActive:
                        DisplayFeeds = (from feed in feeds.GetFeeds()
                                        where (!feed.Active && !feed.Disabled)
                                        select feed).ToList();
                        DisplayFeedsName = "All Non Active Feeds";
                        break;
                    case DisplayFeedsType.Privates:
                        DisplayFeeds = feeds.GetFeeds().Where(feed => !feed.Disabled && feed.IsPersonalFeed).ToList();
                        DisplayFeedsName = "All Private Feeds";
                        break;
                    case DisplayFeedsType.Disabled:
                        DisplayFeeds = feeds.GetFeeds().Where(feed => feed.Disabled).ToList();
                        DisplayFeedsName = "All Disabled Feeds";
                        break;
                    case DisplayFeedsType.NotInCategory:
                        DisplayFeeds =
                            feeds.GetFeeds().Where(feed => !feed.Disabled && feed.BelongsToCategories.Count == 0).ToList
                                ();
                        DisplayFeedsName = "All Feeds not in any Category";
                        break;
                    case DisplayFeedsType.InSomeCategory:
                        if (category != null)
                        {
                            DisplayFeeds = (from feed in feeds.GetFeeds()
                                            where feed.BelongsToCategories.Contains(category)
                                            select feed).ToList();
                            DisplayFeedsName = "All Feeds in Category: " + category.CategoryName;
                        }
                        break;

                }
                // tpFeeds.Text = string.Format("Feeds Viewer (Selected Feeds: {0})", DisplayFeedsName);
            }
        }

        public async Task<List<IRSSPost>> GetRSSItemsFromFeeds(List<IRSSFeed> feedsToRefresh, bool unreadOnly, bool fromweb)
        {
            if (BusyRefreshingFromWeb)
            {

                return null;
            }

            string oldDispalyName = DisplayFeedsName;
            //chkbUnreadposts.Checked = unreadOnly;
            //tpFeeds.Text = string.Format("Feeds Viewer (Selected Feeds: {0})", DisplayFeedsName);
            //tstxtbSearchFeed.Text = "Search in feeds: " + DisplayFeedsName;
            // LogInThisForm(string.Empty, true); //clear log
            OnLogOperation(this, new LogArgs(DateTime.Now + ": Start refreshing feed(s): " + DisplayFeedsName));
            string refreshMsg = string.Format("Start refreshing: {0} at {1}", DisplayFeedsName, DateTime.Now);
            OnWebRefresh(this, new LogArgs(refreshMsg));



            var posts = new List<IRSSPost>();
            int i = 0;
            int finished = 0;
            foreach (var feed in feedsToRefresh)
            {
                IRSSFeed feed1 = feed;
                var items = await feed1.GetAllItemsFromWeb(false, !Settings.AppRSSSetings.NotifyOnRSSErrors);
                posts.AddRange(items);
                OnLogOperation(this,
                               new LogArgs(string.Format("{0}: creating task {1} for feed {2}",
                                                         DateTime.Now, i, feed1.RSSName)));
                var op = string.Format(
                                    "{0}: Task  finished for feed {1}. Time: {2} Seconds, Download size: {3} KB. {4} new posts",
                                    DateTime.Now, feed1.RSSName,
                                    feed1.LastDownloadTime.TotalSeconds,
                                    feed1.LastDownloadSizeKb,
                                    feed1.LastNewPostsCount);
                OnLogOperation(this, new LogArgs(string.Format(op)));
                //string msg = string.Format("{0} Finished feeds: {1}/{2}",
                //                           Resources.UnReadRSSPostsReading,
                //                           ++finished, numOfTasks);
                //Util.Utils.UpdateControl(chkbUnreadposts, msg);
                //if (tasks[i1].Status == TaskStatus.RanToCompletion)
                //{
                //    var results = tasks[i1].Result.ToList();
                //    var unreadposts =
                //        feed1.GetAllItemsFromCache(unreadOnly, true, true).
                //            ToList();
                //    posts.AddRange(unreadposts);
                //    DisplayPosts = posts;
                //    if (unreadposts.Count > 0)
                //    {
                //       // DisplayRSSItems(DisplayPosts, dgvRSSItems);
                //      //  UpdateFeedsCountsAfterChange();
                //    }
                //}

            }

            OnWebEndRefresh(this, new LogArgs(refreshMsg));

            // Util.Utils.UpdateControl(chkbUnreadposts, oldDispalyName);


            DisplayPosts = posts;
            DisplayRSSItems(unreadOnly);
            //UpdateFeedsCountsAfterChange(true);
            ulong totalFeedsKB = 0;
            foreach (RSSFeedsContainer rssGrouping in FeedsGroup)
                foreach (var feed in rssGrouping.GetFeeds())
                {
                    totalFeedsKB += feed.TotalDownloadedKB;
                }

            TotalDownloadedKB = totalFeedsKB;

            //save feeds to disk?
            if (Settings.AppRSSSetings.SaveRSSFeedsOnEveryRefresh && fromweb && posts.Count > 0)
            {
                SaveAllFeedsToDisk();
            }

            // var postsarg = new RefreshArgs(posts);
            //  OnWebRefresh(this, postsarg);
            //Util.Utils.UpdateControl(tpFeeds,string.Format("Feeds Viewer (Selected Feeds: {0})", oldDispalyName));

            return posts;
        }

        private void DisplayRSSItems(bool unreadonly, List<IRSSPost> alternativePost = null)
        {
            List<IRSSPost> postsToDisplay = alternativePost;
            if (postsToDisplay == null)
            {
                postsToDisplay = DisplayPosts;
            }

            postsToDisplay = postsToDisplay.Where(itm => !itm.Read || itm.Read != unreadonly).ToList();
            postsToDisplay = postsToDisplay.OrderByDescending(itm => itm.AddedDate).ToList();
            // olvPosts.SetObjects(postsToDisplay);
            // olvPosts.ShowGroups = chkbGroupResults.Checked;

            //if (postsToDisplay.Count > 0)
            //    olvPosts.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            int countnewitems = (postsToDisplay).Count(itm => itm != null && itm.Read == false);

            //chkbUnreadposts.Text = Resources.UnReadRSSPostsAlreadyTotalItems + postsToDisplay.Count;
            //tsslTotalDownloaded.Text = Utils.FormatKBytes(TotalDownloadedKB) + " Total Downloaded";

            //if (countnewitems > 0 && UserActive)
            //    notifyIconStatus.ShowBalloonTip(1000, "RSS Desktop Aggregator",string.Format("{0} new RSS Items", countnewitems), ToolTipIcon.None);

        }
        private void SaveAllFeedsToDisk()
        {
            foreach (RSSFeedsContainer rssGrouping in FeedsGroup)
            {
                rssGrouping.SerializeToBinaryFile(Settings.AppRSSSetings.FileName);
            }
        }
        private void LoadFeedsPosts()
        {
            if (DisplayFeeds != null)
            {
                // chkbUnreadposts.CheckedChanged -= chkbUnreadposts_CheckedChanged;
                DisplayPosts = (from feed in DisplayFeeds
                                from post in feed.GetAllItemsFromCache(false)
                                select post).ToList();


                // chkbUnreadposts.CheckedChanged += chkbUnreadposts_CheckedChanged;
            }
        }
        
    }
}
