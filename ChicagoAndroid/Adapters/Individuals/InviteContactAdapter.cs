using System;
using System.Collections.Generic;
using System.Linq;
using Android.Views;
using Android.Widget;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.ChicagoAndroid.ViewHolders.Individuals;

namespace Tabs.Mobile.ChicagoAndroid.Adapters.Individuals
{
    public class InviteContactAdapter : BaseAdapter<string>
    {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<PhoneContact> PhoneContacts { get; set; }

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        private Activities.Individuals.InviteContactActivity InviteContactActivity { get; set; }

        #endregion

        #region Constructors

        public InviteContactAdapter(Activities.Individuals.InviteContactActivity inviteContactActivity,
            List<PhoneContact> phoneContacts)
        {
            this.InviteContactActivity = inviteContactActivity;
            this.PhoneContacts = phoneContacts;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get number of item to be displayed
        /// </summary>
        public override int Count
        {
            get
            {
                return this.PhoneContacts.Count;
            }
        }

        /// <summary>
        /// Gets item ID
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
		public override long GetItemId(int position)
        {
            return position;
        }

        /// <summary>
        /// Get the type of View that will be created for the specified item.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override int GetItemViewType(int position)
        {
            return base.GetItemViewType(position);
        }

        /// <summary>
        /// Get item at specified position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override Java.Lang.Object GetItem(int position)
        {
            return "";
        }

        public override string this[int position]
        {
            get
            {
                return this.PhoneContacts.ElementAt(position).FirstName;
            }
           
        }

        /// <summary>
        /// Gets view... list cells
        /// </summary>
        /// <param name="position"></param>
        /// <param name="convertView"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public override View GetView(int position, View convertView, ViewGroup container)
        {
            InviteContactsViewHolder holder = null;
            var view = convertView;

            if (view != null)
            {
                holder = view.Tag as InviteContactsViewHolder;
            }
            if (holder == null)
            {
                view = LayoutInflater.FromContext(this.InviteContactActivity).Inflate(Resource.Layout.InviteContactListItem, container, false);
                holder = new InviteContactsViewHolder(this, view, OnActionBtnListener);
                view.Tag = holder;
            }

            var item = this.PhoneContacts.ElementAt(position);

            if (item != null)
            {
                holder.Invitebutton.SetTag(Resource.Id.inviteBtn, position);
                holder.Title.Text = string.IsNullOrEmpty(item.Name) ? "" : item.Name;
                holder.Subtitle.Text = string.IsNullOrEmpty(item.PhoneNumber) ? "" : item.PhoneNumber;
            }

            return view;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="viewHolder"></param>
        public async void OnActionBtnListener(int position, InviteContactsViewHolder viewHolder)
        {
            try
            {
                if (this.InviteContactActivity.CheckNetworkConnectivity() == null)
                {
                    Toast.MakeText(this.InviteContactActivity, ToastMessage.NoInternet, ToastLength.Short).Show();
                }
                else
                {
                    var item = this.PhoneContacts.ElementAt(position);
                    this.InviteContactActivity.ShowProgressbar(true, "", ToastMessage.PleaseWait);

                    await App.SMSMessageFactory.SendInvitation(item.PhoneNumber,
                        this.InviteContactActivity.CurrentUser.FirstName, item.FirstName);

                    this.InviteContactActivity.ShowProgressbar(false, "", ToastMessage.PleaseWait);

                    Shared.Models.Points.Point point = new Shared.Models.Points.Point();
                    point.UserId = this.InviteContactActivity.CurrentUser.UserId;
                    point.PointStatus = Shared.Models.Points.Point.ToasterPointStatus.Earned;
                    point.EarnedDate = DateTime.Now;
                    point.RedeemedDate = null;
                    point.PointAmount = (int)Shared.Models.Points.Point.PointAmountScale.Invite;
                    Toast.MakeText(this.InviteContactActivity, ToastMessage.InviteSent, ToastLength.Short).Show();
                    await App.ToasterPointsFactory.NewDailyPoint(point);
                    await new Shared.Helpers.PushNotificationHelper(App.NotificationRegisterFactory, Shared.Helpers.PushNotificationHelper.PushPlatform.Android).NewPointsPush(point.UserId);
                }
            }
            catch (Exception)
            {
                this.InviteContactActivity.ShowProgressbar(false, "", ToastMessage.PleaseWait);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
        }

        #endregion

    }
}