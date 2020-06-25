using System;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class EditToasterSwitchCell : UITableViewCell
    {

        #region Properties

        public UILabel Title
        {
            get
            {
                return _Title;
            }
        }

        public UISwitch TitleSwitch
        {
            get
            {
                return _TitleSwitch;
            }
        }

        #endregion

        #region Constructors

        public EditToasterSwitchCell (IntPtr handle) : base (handle)
        {
        }

        #endregion

    }
}