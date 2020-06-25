using Foundation;
using System;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class SignUpItemCell : UITableViewCell
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

        #endregion

        #region Constructors

        public SignUpItemCell (IntPtr handle) : base (handle)
        {
        }

        #endregion

    }
}