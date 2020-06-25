using Foundation;
using System;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class UserTypeCell : UITableViewCell
    {

        #region Properties

        public UILabel _Title
        {
            get
            {
                return Title;
            }
        }

        public UISwitch _TitleSwitch
        {
            get
            {
                return TitleSwitch;
            }
        }

        #endregion

        #region Constructors

        public UserTypeCell (IntPtr handle) : base (handle)
        {
        }

        #endregion

    }
}