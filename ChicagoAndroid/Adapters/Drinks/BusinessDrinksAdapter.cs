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
using Newtonsoft.Json;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Models.Drinks;
using Tabs.Mobile.ChicagoAndroid.Fragments.Drinks;

namespace Tabs.Mobile.ChicagoAndroid.Adapters.Drinks
{
    public class BusinessDrinksAdapter : BaseAdapter
    {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<BusinessDrink> Drinks { get; set; }

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        private BusinessDrinksFragment BusinessDrinksFragment { get; set; }

        #endregion

        #region Constructors

        public BusinessDrinksAdapter(BusinessDrinksFragment businessDrinksFragment,
            List<BusinessDrink> drinks)
        {
            this.BusinessDrinksFragment = businessDrinksFragment;
            this.Drinks = drinks;
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
                return this.Drinks.Count;
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

        /// <summary>
        /// Gets view... list cells
        /// </summary>
        /// <param name="position"></param>
        /// <param name="convertView"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public override View GetView(int position, View convertView, ViewGroup container)
        {
            if (convertView == null)
            {
                convertView = LayoutInflater.FromContext(this.BusinessDrinksFragment.HomeContext).Inflate(Resource.Layout.BusinessDrinksListItem, container, false);
            }

            var title = convertView.FindViewById<TextView>(Resource.Id.title);
            var subTitle = convertView.FindViewById<TextView>(Resource.Id.subTitle);

            var item = this.Drinks.ElementAt(position);

            if (item != null)
            {
                title.Text = string.IsNullOrEmpty(item.DrinkName) ? "" : item.DrinkName;
                subTitle.Text = "$" + item.Price.ToString();
            }

            return convertView;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var item = this.Drinks.ElementAt(e.Position);
            Intent activity = new Intent(this.BusinessDrinksFragment.HomeContext, typeof(Activities.Drinks.AddEditDrinkActivity));
            activity.PutExtra("ScreenActionType", (int)Activities.Drinks.AddEditDrinkActivity.ActionType.Edit);
            activity.PutExtra("BusinessDrink", JsonConvert.SerializeObject(item));
            this.BusinessDrinksFragment.HomeContext.StartActivity(activity);
        }

        #endregion

    }
}