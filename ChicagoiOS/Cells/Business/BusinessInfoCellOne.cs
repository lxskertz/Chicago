using Foundation;
using System;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class BusinessInfoCellOne : UITableViewCell
    {

        #region Properties

        /// <summary>
        /// Gets or sets title
        /// </summary>
        public UILabel _Title
        {
            get
            {
                return Title;
            }
        }

        /// <summary>
        /// Gets or sets title field
        /// </summary>
        public UITextField _TitleField
        {
            get
            {
                return TitleField;
            }
        }

        /// <summary>
        /// Gets or sets the index path
        /// </summary>
        public NSIndexPath IndexPath { get; set; }

        #endregion

        public BusinessInfoCellOne (IntPtr handle) : base (handle)
        {
        }
    }
}