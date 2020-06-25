using Foundation;
using System;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class OtherEventInfoCellTwo : UITableViewCell
    {
        #region Properties

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

        /// <summary>
        /// 
        /// </summary>
        public UISwitch _TitleSwitch
        {
            get
            {
                return TitleSwitch;
            }
        }

        #endregion

        public OtherEventInfoCellTwo (IntPtr handle) : base (handle)
        {
        }
    }
}