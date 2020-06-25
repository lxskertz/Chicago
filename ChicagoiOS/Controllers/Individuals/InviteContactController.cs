using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIKit;
using Tabs.Mobile.ChicagoiOS.DataSource.Individuals;
using BigTed;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class InviteContactController : BaseViewController
    {
        #region Properties

        private InviteContactDataSource InviteContactDataSource { get; set; }

        #endregion


        #region Constructors

        public InviteContactController (IntPtr handle) : base (handle)
        {
        }

        #endregion

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
                LoadContacts();
            }
            catch (Exception)
            {
            }
        }

        /// <summary> 
        /// 
        /// </summary>
        /// <returns></returns>
        private async void LoadContacts()
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else
                {
                    BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);

                    var contacts = await new Helpers.ContactService().GetAllContacts();

                    if (contacts != null)
                    {
                        contacts = contacts.OrderBy(x => x.FirstName).ToList();
                        ContactTable.EstimatedRowHeight = 44f;
                        ContactTable.RowHeight = UITableView.AutomaticDimension;
                        InviteContactDataSource = new InviteContactDataSource(this, contacts.ToList());
                        ContactTable.Source = InviteContactDataSource;
                        ContactTable.TableFooterView = new UIView();
                    }
                    BTProgressHUD.Dismiss();
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                BTProgressHUD.Dismiss();
            }
        }
    }
}