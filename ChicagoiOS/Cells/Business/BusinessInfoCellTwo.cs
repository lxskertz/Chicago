using Foundation;
using System;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class BusinessInfoCellTwo : UITableViewCell
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

        public BusinessInfoCellTwo (IntPtr handle) : base (handle)
        {
        }
    }
}