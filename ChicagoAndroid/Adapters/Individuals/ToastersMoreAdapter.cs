using System;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;
using Tabs.Mobile.Shared.Helpers;

namespace Tabs.Mobile.ChicagoAndroid.Adapters.Individuals
{
    public class ToastersMoreAdapter : BaseAdapter
    {

        #region Contants, Enums, and Variables

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the adapter context
        /// </summary>
        private Fragments.Individuals.ToastersMoreFragment MyContext { get; set; }

        public string[] Titles { get; set; }

        #endregion

        #region Constructors

        public ToastersMoreAdapter(Fragments.Individuals.ToastersMoreFragment context, string[] titles)
        {
            this.MyContext = context;
            this.Titles = titles;
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
                return this.Titles.Length;
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
                convertView = LayoutInflater.FromContext(this.MyContext.HomeContext).Inflate(Resource.Layout.ToastersMoreListView, container, false);
            }

            var item = this.Titles.ElementAt(position);
            var title = convertView.FindViewById<TextView>(Resource.Id.title);
            title.Text = this.Titles.ElementAt(position);

            return convertView;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var item = this.Titles.ElementAt(e.Position);
            switch (item)
            {
                case MoreScreenHelper.Points:
                    Intent intent = new Intent(this.MyContext.HomeContext, typeof(Activities.Points.ToasterPointsActivity));
                    this.MyContext.HomeContext.StartActivity(intent);
                    break;
                case MoreScreenHelper.Settings:
                    break;
                case MoreScreenHelper.Orders:
                    break;
                case MoreScreenHelper.Payment:
                    Intent activity = new Intent(this.MyContext.HomeContext, typeof(Activities.Payments.PaymentMethodsActivity));                  
                    this.MyContext.HomeContext.StartActivity(activity);
                    break;
                case MoreScreenHelper.Logout:
                    this.MyContext.HomeContext.Logout();
                    break;
            }
        }

        ///// <summary>
        ///// Logout
        ///// </summary>
        ///// <returns></returns>
        //private async void Logout()
        //{
        //    try
        //    {
        //        this.MyContext.HomeContext.DeleteSavedPreferences();
        //        //DeleteCredentials();

        //        if (this.MyContext.HomeContext.CheckNetworkConnectivity() != null)
        //        {
        //            await App.UsersFactory.Logout(this.MyContext.HomeContext.CurrentUser.Email);
        //        }

        //        this.MyContext.HomeContext.StartActivity(typeof(Activities.HomeActivity));
        //        this.MyContext.HomeContext.Finish();
        //    }
        //    catch (Exception)
        //    {
        //        this.MyContext.HomeContext.StartActivity(typeof(Activities.HomeActivity));
        //        this.MyContext.HomeContext.Finish();
        //    }
        //}



        #endregion

    }
}