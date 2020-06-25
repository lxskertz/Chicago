using Foundation;
using System;
using UIKit;
using Tabs.Mobile.ChicagoiOS.DataSource.Individuals;
using Tabs.Mobile.Shared.Models.Individuals;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class ToastersSearchCell : UITableViewCell
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

        /// <summary>
        /// 
        /// </summary>
        public UIButton RequestBtn
        {
            get
            {
                return _FollowBtn;
            }
        }

        public UIImageView _ProfilePic
        {
            get
            {
                return ProfilePic;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ToastersSearchDataSource DataSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public NSIndexPath IndexPath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ToastersSearchItem Item { get; set; }

        #endregion

        #region Constructors

        public ToastersSearchCell (IntPtr handle) : base(handle) { }

        #endregion

        #region Methods

        partial void _FollowBtn_TouchUpInside(UIButton sender)
        {
        }

        #endregion

    }
}