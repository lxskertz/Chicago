using Foundation;
using System;
using UIKit;
using Tabs.Mobile.Shared.Models.CheckIns;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.ChicagoiOS.DataSource.Business;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class BusinessCheckInsCell : UITableViewCell
    {

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
        public UILabel _SubTitle
        {
            get
            {
                return SubTitle;
            }
        }


        public UIButton _SendDrinkBtn
        {
            get
            {
                return SendDrinkBtn;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public BusinessCheckInsDataSource DataSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public NSIndexPath IndexPath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CheckIn Item { get; set; }

        public BusinessCheckInsCell (IntPtr handle) : base (handle)
        {
        }

        partial void SendDrinkBtn_TouchUpInside(UIButton sender)
        {
            var controller = this.DataSource.Controller.Storyboard.InstantiateViewController("SendDrinkController") as SendDrinkController;
            controller.CheckInItem = Item;
            controller.FromBusiness = true;
            this.DataSource.Controller.NavigationController.PushViewController(controller, true);
        }
    }
}