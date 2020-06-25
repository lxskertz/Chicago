using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Tabs.Mobile.Shared.Resources;

namespace Tabs.Mobile.ChicagoAndroid.Adapters.Individuals
{
    public class NavigationDrawerMenuAdapter : BaseAdapter<string>
    {

        #region Properties 

        /// <summary>
        /// Gets or sets the adaptor owner
        /// </summary>
        private AppCompatActivity Owner { get; set; }

        /// <summary>
        /// Gets or the navigation menu list
        /// </summary>
        public List<NavigationDrawerMenuItems> SlideMenu = new List<NavigationDrawerMenuItems>();


        #endregion

        #region Constructors

        public NavigationDrawerMenuAdapter(AppCompatActivity activity, List<NavigationDrawerMenuItems> menuItems) : base()
        {
            this.Owner = activity;
            this.SlideMenu = menuItems;
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
                return SlideMenu.Count;
            }
        }

        /// <summary>
        /// Gets list item
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override Java.Lang.Object GetItem(int position)
        {
            return SlideMenu[position].Title;
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
        /// Get Item
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override string this[int position]
        {
            get
            {
                return SlideMenu[position].Title;
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
            if (convertView == null)
            {
                convertView = LayoutInflater.FromContext(Owner).Inflate(Resource.Layout.NavigationDrawerListItem, container, false);
                convertView.SetBackgroundColor(Android.Graphics.Color.White);
            }
            ListView navBarList = (ListView)container;
            TextView title = convertView.FindViewById<TextView>(Resource.Id.menuItem);
            ImageView titleImage = convertView.FindViewById<ImageView>(Resource.Id.navBarImage);

            var titleImageSrc = position == navBarList.CheckedItemPosition ? SlideMenu[position].SelectedTitleImage : SlideMenu[position].UnSelectedTitleImage;
            Android.Graphics.Color titleColor = position == navBarList.CheckedItemPosition ? Color.NavBarSelectedText : Color.NavBarUnSelectedText;

            title.Text = SlideMenu[position].Title;
            title.SetTextColor(titleColor);
            int resID = Owner.Resources.GetIdentifier(titleImageSrc, "drawable", Owner.PackageName);
            titleImage.SetImageResource(resID);

            return convertView;
        }

        #endregion

    }
}