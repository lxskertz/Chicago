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
using Android.Support.V4.App;

namespace Tabs.Mobile.ChicagoAndroid.Adapters
{
    public class MyPagerAdapter : FragmentPagerAdapter
    { 

        #region Properties

        /// <summary>
        /// Gets or sets the fragment list
        /// </summary>
        public List<Android.Support.V4.App.Fragment> FragmentList = new List<Android.Support.V4.App.Fragment>();

        /// <summary>
        /// 
        /// </summary>
        public List<string> FragmentTitles = new List<string>();

        #endregion

        #region Constructors

        public MyPagerAdapter(Android.Support.V4.App.FragmentManager fm) : base(fm) { }

        #endregion

        #region Methods 


        /// <summary>
        /// Add fragment to the list
        /// </summary>
        /// <param name="fragment"></param>
        public void AddFragment(Android.Support.V4.App.Fragment fragment, string title)
        {
            FragmentList.Add(fragment);
            FragmentTitles.Add(title);
        }

        /// <summary>
        /// Add view fragment to the list
        /// </summary>
        /// <param name="view"></param>
        //public void AddFragmentView(Func<LayoutInflater, ViewGroup, Bundle, View> view)
        //{
        //    FragmentList.Add(new Android.Support.V4.App.Fragment(view));
        //}

        /// <summary>
        /// Get the tutorial screen count
        /// </summary>
        public override int Count
        {
            get
            {
                return FragmentList.Count;
            }
        }

        /// <summary>
        /// Get selected item
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return FragmentList[position];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(FragmentTitles[position]);
        }

        #endregion

    }
}