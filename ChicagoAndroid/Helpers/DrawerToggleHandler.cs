using System;
using Android.App;
using Android.Views;
using Android.Support.V7.App;
using Android.Support.V4.Widget;


namespace Tabs.Mobile.ChicagoAndroid.Helpers
{
    public class ActionBarDrawerEventArgs : EventArgs
    {

        #region Properties

        /// <summary>
        /// Gets or sets the drawer view
        /// </summary>
        public View DrawerView { get; set; }

        /// <summary>
        /// Gets or set new state
        /// </summary>
        public int NewState { get; set; }

        /// <summary>
        /// Gets or sets the slide offset
        /// </summary>
        public float SlideOffset { get; set; }

        #endregion

    }
    
    #region Delegates

    public delegate void ActionBarDrawerChangedEventHandler(object s, ActionBarDrawerEventArgs e);

    #endregion

    #region InnerClass

    public class DrawerToggleHandler : ActionBarDrawerToggle
    {

        #region Constructors

        public DrawerToggleHandler(Activity activity, DrawerLayout drawerLayout, int drawerImageRes, int openDrawerContentDescRes, int closeDrawerContentDescRes)
            : base(activity, drawerLayout, openDrawerContentDescRes, closeDrawerContentDescRes) { }

        #endregion

        #region Events

        public event ActionBarDrawerChangedEventHandler DrawerClosed;
        public event ActionBarDrawerChangedEventHandler DrawerOpened;
        public event ActionBarDrawerChangedEventHandler DrawerSlide;
        public event ActionBarDrawerChangedEventHandler DrawerStateChanged;

        #endregion

        #region Methods

        /// <summary>
        /// Called on navigation drawer close
        /// </summary>
        /// <param name="drawerView"></param>
        public override void OnDrawerClosed(View drawerView)
        {
            if (null != DrawerClosed)
                DrawerClosed(this, new ActionBarDrawerEventArgs { DrawerView = drawerView });
            base.OnDrawerClosed(drawerView);
        }

        /// <summary>
        /// Called on navigation drawer open
        /// </summary>
        /// <param name="drawerView"></param>
        public override void OnDrawerOpened(View drawerView)
        {
            if (null != DrawerOpened)
                DrawerOpened(this, new ActionBarDrawerEventArgs { DrawerView = drawerView });
            base.OnDrawerOpened(drawerView);
        }

        /// <summary>
        /// Called when drawer slide
        /// </summary>
        /// <param name="drawerView"></param>
        /// <param name="slideOffset"></param>
        public override void OnDrawerSlide(View drawerView, float slideOffset)
        {
            if (null != DrawerSlide)
                DrawerSlide(this, new ActionBarDrawerEventArgs
                {
                    DrawerView = drawerView,
                    SlideOffset = slideOffset
                });
            base.OnDrawerSlide(drawerView, slideOffset);
        }

        /// <summary>
        /// Called when drawer change state
        /// </summary>
        /// <param name="newState"></param>
        public override void OnDrawerStateChanged(int newState)
        {
            if (null != DrawerStateChanged)
                DrawerStateChanged(this, new ActionBarDrawerEventArgs
                {
                    NewState = newState
                });
            base.OnDrawerStateChanged(newState);
        }

        #endregion

    }

    #endregion

}