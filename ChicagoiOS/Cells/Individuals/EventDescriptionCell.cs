using Foundation;
using System;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class EventDescriptionCell : UITableViewCell
    {


        /// <summary>
        /// 
        /// </summary>
        public UILabel _EventDescription
        {
            get
            {
                return EventDescription;
            }
        }


        public EventDescriptionCell (IntPtr handle) : base (handle)
        {
        }
    }
}