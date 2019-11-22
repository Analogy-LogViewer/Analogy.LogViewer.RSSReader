using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analogy.LogViewer.RSSReader.Core
{
    public interface IDisplayedInSystemNotification
    {
        bool DisplayedInSystemNotification { get; set; }
        byte ShowedInPopupCount { get; set; }
    }
}
