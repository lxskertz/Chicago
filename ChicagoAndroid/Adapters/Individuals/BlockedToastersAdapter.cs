using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Tabs.Mobile.Shared.Models.Individuals;
using Tabs.Mobile.ChicagoAndroid.Activities.Individuals;
using Tabs.Mobile.ChicagoAndroid.ViewHolders.Individuals;

namespace Tabs.Mobile.ChicagoAndroid.Adapters.Individuals
{
    public class BlockedToastersAdapter : BaseAdapter
    {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<Toasters> Rows { get; set; } = new List<Toasters>();

        private BlockedToastersActivity BlockedToastersActivity { get; set; }

        #endregion

        #region Constructors

        public BlockedToastersAdapter(BlockedToastersActivity context, List<Toasters> rows)
        {
            this.BlockedToastersActivity = context;
            this.Rows = rows;
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
                return this.Rows.Count;
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
            return position;
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
            BlockedToastersViewholder holder = null;
            var view = convertView;

            if (view != null)
            {
                holder = view.Tag as BlockedToastersViewholder;
            }

            if (holder == null)
            {
                view = LayoutInflater.FromContext(this.BlockedToastersActivity).Inflate(Resource.Layout.BlockedToastersListItem, container, false);
                holder = new BlockedToastersViewholder(this, view, OnActionBtnListener);
                view.Tag = holder;
            }

            var item = this.Rows.ElementAt(position);
            holder.ActionButon.SetTag(Resource.Id.unblockBtn, position);
            var firstName = !string.IsNullOrEmpty(item.FirstName) ? item.FirstName : string.Empty;
            var lastName = !string.IsNullOrEmpty(item.LastName) ? item.LastName : string.Empty;
            holder.Name.Text = firstName + " " + lastName;

            return view;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="viewHolder"></param>
        public void OnActionBtnListener(int position, BlockedToastersViewholder viewHolder)
        {
            var item = this.Rows.ElementAt(position);
            this.BlockedToastersActivity.UnblockUser(item);
        }


        #endregion

    }
}