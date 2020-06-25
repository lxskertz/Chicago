using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Tabs.Mobile.ChicagoAndroid.Fragments.Business
{
    public class BaseBusinessFragment : Android.Support.V4.App.Fragment
    {

        #region Properties

        /// <summary>
        /// Gets or sets the home context
        /// </summary>
        public Activities.Businesses.BusinessHomeActivity HomeContext { get; set; }

        #endregion
    }
}