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
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using V4Fragment = Android.Support.V4.App.Fragment;
using V4FragmentManager = Android.Support.V4.App.FragmentManager;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Newtonsoft.Json;
using Tabs.Mobile.ChicagoAndroid.Adapters;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.ChicagoAndroid.Fragments.Individuals;

namespace Tabs.Mobile.ChicagoAndroid.Fragments.Individuals
{
    public class IndividualHomeFragment : BaseIndividualsFragment
    {

        #region Constants, Enums, and Variables

        // Create a new instance field for this activity.
        private static IndividualHomeFragment instance;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or Sets the adaptor
        /// </summary>
        private MyPagerAdapter Adaptor { get; set; }

        /// <summary>
        /// Gets or sets the TutorialPager
        /// </summary>
        private ViewPager ViewPager { get; set; }

        public CheckIns.LiveToastersFragment LiveToastersFragment { get; private set; }

        public LiveEventsFragment LiveEventsFragment { get; private set; }

        /// <summary>
        /// Gets or sets the toolbar
        /// </summary>
        //private V7Toolbar Toolbar { get; set; }

        #endregion

        #region Constructors

        public IndividualHomeFragment(Activities.Individuals.IndividualHomeActivity context)
        {
            this.HomeContext = context;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create instance of this fragment
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static V4Fragment NewInstance(Activities.Individuals.IndividualHomeActivity context)
        {
            instance = new IndividualHomeFragment(context);

            return instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.ToastersLive, container, false);
            try
            {
                this.ViewPager = view.FindViewById<ViewPager>(Resource.Id.viewpager);
                //this.Toolbar = view.FindViewById<V7Toolbar>(Resource.Id.toolbar);
                //this.Toolbar.InflateMenu(Resource.Menu.individual_home_menu);

                var tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabs);
                tabLayout.SetupWithViewPager(this.ViewPager);

                this.Adaptor = new MyPagerAdapter(this.ChildFragmentManager);
                this.LiveToastersFragment = new CheckIns.LiveToastersFragment(this.HomeContext);
                this.LiveEventsFragment = new LiveEventsFragment(this.HomeContext);
                this.Adaptor.AddFragment(this.LiveToastersFragment, "Toasters");
                this.Adaptor.AddFragment(this.LiveEventsFragment, "Events");
                //ViewPager.OffscreenPageLimit = 2;
                this.ViewPager.Adapter = this.Adaptor;
                this.ViewPager.Adapter.NotifyDataSetChanged();
            }
            catch (Exception)
            {
            }

            return view;
        }

        #endregion

    }
}