using Foundation;
using System;
using UIKit;
using Tabs.Mobile.ChicagoiOS.DataSource.Individuals;
using Tabs.Mobile.Shared.Models.Individuals;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class ToastersCell : UITableViewCell
    {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public UILabel Name
        {
            get
            {
                return _Name;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UILabel Username
        {
            get
            {
                return _Username;
            }
        }

        public UIImageView ProfilePic
        {
            get
            {
                return _ProfilePic;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ToastersDataSource DataSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public NSIndexPath IndexPath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Toasters Item { get; set; }

        #endregion

        #region Constructors

        public ToastersCell (IntPtr handle) : base(handle) { }

        #endregion

    }
}