using Foundation;
using System;
using UIKit;
using Tabs.Mobile.ChicagoiOS.DataSource.Individuals;
using Tabs.Mobile.Shared.Models.Individuals;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class BlockedUserCell : UITableViewCell
    {

        #region Properties

        public UILabel _Name
        {
            get {
                return Name;
            }
        }

        public UIButton _UnblockBtn
        {
            get
            {
                return UnblockBtn;
            }
        }

        public BlockedUserDataSource DataSource { get; set; }

        public NSIndexPath IndexPath { get; set; }

        public Toasters Item { get; set; }

        #endregion

        #region Constructors

        public BlockedUserCell(IntPtr handle) : base(handle) { }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        partial void UnblockBtn_TouchUpInside(UIButton sender)
        {
            this.DataSource.Controller.UnBlockedUser(Item);
        }

        #endregion

    }
}