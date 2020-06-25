using Foundation;
using System;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class BlockedToastersCell : UITableViewCell
    {


        public UIButton _UnBlockBtn
        {
            get
            {
                return UnBlockBtn;
            }
        }

        public UILabel ToasterName
        {
            get
            {
                return _ToasterName;
            }
        }

        public BlockedToastersCell (IntPtr handle) : base (handle)
        {
        }
    }
}