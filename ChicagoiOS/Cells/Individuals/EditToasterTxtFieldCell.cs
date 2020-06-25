using System;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class EditToasterTxtFieldCell : UITableViewCell
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
                return TxtField;
            }
        }

        #endregion

        #region Constructors

        public EditToasterTxtFieldCell (IntPtr handle) : base (handle)
        {
        }

        #endregion

    }
}