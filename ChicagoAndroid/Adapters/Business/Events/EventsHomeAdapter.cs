using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Tabs.Mobile.Shared.Models.Events;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.ChicagoAndroid.Fragments.Business.Events;
using Tabs.Mobile.ChicagoAndroid.ViewHolders.Business;

namespace Tabs.Mobile.ChicagoAndroid.Adapters.Business.Events
{
    public class EventsHomeAdapter : RecyclerView.Adapter, Helpers.ItemTouchHelperAdapter
    {
        #region Constants, Enums, and Variables

        public event EventHandler<int> ItemClick;
        private View itemView;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<BusinessEvents> Rows { get; set; }

        /// <summary>
        /// Gets or sets the adapter context
        /// </summary>
        public Activities.Businesses.BusinessHomeActivity MyContext { get; set; }

        /// <summary>
        /// Gets or sets view holders
        /// </summary>
        public EventsHomeViewHolder EventsHomeViewHolder { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BusinessEventsFragment BusinessEventsFragment { get; set; }

        #endregion

        #region Constructors

        public EventsHomeAdapter(Activities.Businesses.BusinessHomeActivity context, List<BusinessEvents> rows,
            BusinessEventsFragment businessEventsFragment)
        {
            this.Rows = rows;
            this.MyContext = context;
            this.BusinessEventsFragment = businessEventsFragment;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Return the view type of the item at position for the purposes of view recycling.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override int GetItemViewType(int position)
        {
            return base.GetItemViewType(position);
        }

        /// <summary>
        /// Returns the total number of items in the data set hold by the adapter.
        /// </summary>
        public override int ItemCount
        {
            get
            {
                return Rows.Count;
            }
        }

        /// <summary>
        /// Create a new container/product CardView invoked by the layout manager
        /// Called when RecyclerView needs a new RecyclerView.ViewHolder of the given type to represent an item.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="viewType"></param>
        /// <returns></returns>
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.EventsHomeListItem, parent, false);

            return new EventsHomeViewHolder(this, itemView, OnListItemClick);
        }

        /// <summary>
        /// Fill in the contents of the container/product card invoked by the layout manager
        /// Called by RecyclerView to display the data at the specified position.
        /// This method should update the contents of the ItemView to reflect the item at the given position.
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="position"></param>
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            this.EventsHomeViewHolder = holder as EventsHomeViewHolder;
            BindCardData(this.EventsHomeViewHolder, Rows.ElementAt(position));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewHolder"></param>
        /// <param name="data"></param>
        private void BindCardData(EventsHomeViewHolder viewHolder, BusinessEvents bEvent)
        {
            viewHolder.Title.Text = string.IsNullOrEmpty(bEvent.Title) ? "" : bEvent.Title;
            viewHolder.Description.Text = string.IsNullOrEmpty(bEvent.EventDescription) ? "" : bEvent.EventDescription;
        }

        /// <summary>
        /// Invoke the OnItemClick event with positional data..
        /// </summary>
        /// <param name="position"></param>
        private void OnItemClick(int position)
        {
            ItemClick?.Invoke(this, position);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromPosition"></param>
        /// <param name="toPosition"></param>
        public void OnItemMove(int fromPosition, int toPosition)
        {
            //if (fromPosition < toPosition) {
            //    for (int i = fromPosition; i < toPosition; i++) {
            //        //Collections.swap(mItems, i, i + 1);
            //    }
            //} else {
            //    for (int i = fromPosition; i > toPosition; i--) {
            //        //Collections.swap(mItems, i, i - 1);
            //    }
            //}
            //NotifyItemMoved(fromPosition, toPosition);

            //var prev = this.Rows.Remove(this.Rows.ElementAtOrDefault(fromPosition));
            //ScanResult.Add(prev);
            //NotifyItemMoved(fromPosition, toPosition);

            //return true;
        }

        /// <summary>
        /// Delete event
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public async Task DeleteEvent(BusinessEvents bEvent)
        {
            try
            {
                if (this.MyContext.CheckNetworkConnectivity() == null)
                {
                    this.MyContext.ShowSnack(itemView, ToastMessage.NoInternet, "OK");
                    return;
                }
                else
                {
                    this.MyContext.ShowProgressbar(true, "", ToastMessage.Deleting);
                    await App.BusinessEventsFactory.Delete(bEvent.EventId);
                    await BlobStorageHelper.DeleteEventLogoBlob(bEvent.EventId);
                    this.MyContext.ShowProgressbar(false, "", ToastMessage.Saving);
                }
            }
            catch (Exception)
            {
                this.MyContext.ShowProgressbar(false, "", ToastMessage.Saving);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        public async void OnItemDismiss(int position)
        {
            var item = this.Rows.ElementAtOrDefault(position);
            await DeleteEvent(item);
            this.Rows.Remove(this.Rows.ElementAtOrDefault(position));
            NotifyItemRemoved(position);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnListItemClick(int position)
        {
            try
            {
                var item = this.Rows.ElementAt(position);
                Intent activity = new Intent(this.MyContext, typeof(Activities.Individuals.Events.EventInfoActivity));
                //Activities.Individuals.Events.EventInfoActivity.ImageBitmap = itemLogo.ImageBitmap;
                activity.PutExtra("BusinessEventInfo", JsonConvert.SerializeObject(item));
                activity.PutExtra("ShowToolbar", false);
                activity.PutExtra("IsBusiness", true);
                this.MyContext.StartActivity(activity);
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        #endregion

    }
}