using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Analogy.Interfaces.Factories;

namespace Analogy.LogViewer.RSSReader.Core
{
   public class RSSDataProvider : IAnalogyDataProvidersFactory
    {
        public string Title { get; }= "Analogy RSS Reader";
        public IEnumerable<IAnalogyDataProvider> Items { get; }

        public RSSDataProvider()
        {
            Items=new List<IAnalogyDataProvider>{};
        }
    }

   public class OnlineRSSREader: IAnalogyRealTimeDataProvider
    {
       public Guid ID { get; } = new Guid("01A17FA2-94F2-46A2-A80A-89AE4893C037");
        public string OptionalTitle { get; } = "Analogy RSS Reader";
        public IAnalogyOfflineDataProvider FileOperationsHandler { get; }
        public bool IsConnected { get; }
        public event EventHandler<AnalogyDataSourceDisconnectedArgs> OnDisconnected;
        public event EventHandler<AnalogyLogMessageArgs> OnMessageReady;
        public event EventHandler<AnalogyLogMessagesArgs> OnManyMessagesReady;

        public Task<bool> CanStartReceiving()
        {
            throw new NotImplementedException();
        }

        public void StartReceiving()
        {
            throw new NotImplementedException();
        }

        public void StopReceiving()
        {
            throw new NotImplementedException();
        }
        
        public void InitDataProvider()
        {

        }
    }
}
