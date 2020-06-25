using Foundation;
using System;
using System.Linq;
using System.Collections.Generic;
using UIKit;
using BigTed;
using Tabs.Mobile.Shared.Models.Businesses;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.ChicagoiOS.DataSource.Business;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class BusinessListCell : UITableViewCell
    {

        /// <summary>
        /// 
        /// </summary>
        public UIButton _CheckInBtn
        {
            get
            {
                return ChkInBtn;
            }
        }

        public UILabel _BusinessType
        {
            get
            {
                return BusinessType;
            }
        }

        public UILabel _BusinessName
        {
            get
            {
                return BusinessName;
            }
        }

        public UIImageView _BusinessLogo
        {
            get
            {
                return BusinessLogo;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public BusinessListDataSource DataSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public NSIndexPath IndexPath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BusinessSearch Item { get; set; }

        public BusinessListCell (IntPtr handle) : base (handle)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        private void OpenCheckIn()
        {
            var controller = this.DataSource.Controller.Storyboard.InstantiateViewController("CheckInController") as CheckInController;
            controller.BusinessInfo = Item;
            controller.CheckInType = Shared.Models.CheckIns.CheckIn.CheckInTypes.Business;
            this.DataSource.Controller.NavigationController.PushViewController(controller, true);
        }

        partial void ChkInBtn_TouchUpInside(UIButton sender)
        {
            OpenCheckIn();
        }
    }
}