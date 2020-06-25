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
using Tabs.Mobile.ChicagoAndroid.Models;

namespace Tabs.Mobile.ChicagoAndroid.Adapters
{
    public class CustomSpinnerAdapter : BaseAdapter
    {

        #region Properties

        /// <summary>
        /// Gets or Sets Current Context
        /// </summary>
        private Activity MyContext { get; set; }

        /// <summary>
        /// Gets or Sets List of Spinner Items
        /// </summary>
        private List<SpinnerItems> SpinnerItem { get; set; }

        #endregion

        #region Constructors

        public CustomSpinnerAdapter(Activity context, List<SpinnerItems> spinnerItem)
        {
            MyContext = context;
            SpinnerItem = spinnerItem;
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
                return SpinnerItem.Count;
            }
        }

        /// <summary>
        /// Gets list item
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override Java.Lang.Object GetItem(int position)
        {
            return position;
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
        /// Gets view... spinner cells
        /// </summary>
        /// <param name="position"></param>
        /// <param name="convertView"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = SpinnerItem[position];
            var view = (convertView ?? MyContext.LayoutInflater.Inflate(Android.Resource.Layout.SimpleSpinnerDropDownItem, parent, false));
            var name = view.FindViewById<TextView>(global::Android.Resource.Id.Text1);
            name.Tag = item.ID;
            name.Text = item.Name;
            return view;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public SpinnerItems GetItemAtPosition(int position)
        {
            return SpinnerItem[position];
        }

        #endregion
    }

    }