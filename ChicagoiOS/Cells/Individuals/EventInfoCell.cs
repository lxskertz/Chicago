using Foundation;
using System;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class EventInfoCell : UITableViewCell
    {

        /// <summary>
        /// 
        /// </summary>
        public UILabel _Title
        {
            get
            {
                return Title;
            }
        }

        public UILabel _SubTitle
        {
            get
            {
                return Subtitle;
            }
        }

        public UIImageView _ImageIcon
        {
            get
            {
                return ImageIcon;
            }
        }

        public EventInfoCell (IntPtr handle) : base (handle)
        {
        }
    }
}