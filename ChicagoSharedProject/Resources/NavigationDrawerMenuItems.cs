using System;
#if __IOS__
using Foundation;
using UIKit;
#endif

namespace Tabs.Mobile.Shared.Resources
{
    public class NavigationDrawerMenuItems
    {

        #region Constants, Enums, and Variables

        public static string[] Titles = { "Home", "Logout" };

        #endregion

        #region Properties

        /// <summary>
        /// Gets or Sets the Menu title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or Sets the selected title Image
        /// </summary>
        public string UnSelectedTitleImage { get; set; }

        /// <summary>
        /// Gets or Sets the unslected title Image
        /// </summary>
        public string SelectedTitleImage { get; set; }

#if __IOS__

        public UITableViewCellStyle CellStyle
        {
            get { return cellStyle; }
            set { cellStyle = value; }
        }
        protected UITableViewCellStyle cellStyle = UITableViewCellStyle.Default;

        public UITableViewCellAccessory CellAccessory
        {
            get { return cellAccessory; }
            set { cellAccessory = value; }
        }
        protected UITableViewCellAccessory cellAccessory = UITableViewCellAccessory.None;

#endif

        #endregion

        #region Constructor

        public NavigationDrawerMenuItems() { }

        public NavigationDrawerMenuItems(string title)
        {
            this.Title = title;
        }

        #endregion

    }
}
