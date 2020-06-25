using Android.Support.Design.Widget;
using Android.OS;
using Android.Views;
using Android.Widget;
using Tabs.Mobile.Shared.Resources;

namespace Tabs.Mobile.ChicagoAndroid.Fragments.Individuals
{
    public class ReportBottomSheetFragment : BottomSheetDialogFragment
    {

        #region Constants, Enums, and Variables

        private TextView spamPost;
        private TextView inappropriatePost;
        private TextView block;
        private TextView unfollow;

        public enum Caller
        {
            LiveToasters = 1,
            ToasterProfile = 2
        }

        #endregion

        #region Properties

        private Caller DialogCaller { get; set; }

        private Activities.BaseActivity MyContext { get; set; }

        #endregion

        #region Constructors

        public ReportBottomSheetFragment() { }

        public ReportBottomSheetFragment(Activities.BaseActivity context, Caller dialogCaller)
        {
            this.MyContext = context;
            this.DialogCaller = dialogCaller;
        }

        #endregion


        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
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
            var view = inflater.Inflate(Resource.Layout.ToastersBottomSheet, container, false);
            spamPost = view.FindViewById<TextView>(Resource.Id.reportPost);
            inappropriatePost = view.FindViewById<TextView>(Resource.Id.reportUser);
            block = view.FindViewById<TextView>(Resource.Id.blockUser);
            unfollow = view.FindViewById<TextView>(Resource.Id.unfollowUser);

            block.Visibility = ViewStates.Gone;
            unfollow.Visibility = ViewStates.Gone;

            //if (this.DialogCaller == Caller.LiveToasters)
            //{
            //    reportUser.Visibility = ViewStates.Gone;
            //}
            //else
            //{
            //    reportPost.Visibility = ViewStates.Gone;
            //    HideShowOptions();
            //}

            spamPost.Text = AppText.ItsSpam;
            inappropriatePost.Text = AppText.ItsInappropriate;
            spamPost.Click += delegate { ReportSpam(); };
            inappropriatePost.Click += delegate { ReportInappropriate(); };

            return view;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public static ReportBottomSheetFragment NewInstance(Activities.BaseActivity context, Caller caller)
        {
            ReportBottomSheetFragment fragment = new ReportBottomSheetFragment(context, caller);
            return fragment;
        }

        /// <summary>
        /// 
        /// </summary>
        private async void ReportSpam()
        {
            if (this.MyContext != null && this.MyContext is Activities.Individuals.SearchToasterProfileActivity)
            {
                var intent = (Activities.Individuals.SearchToasterProfileActivity)this.MyContext;
                if (intent != null)
                {
                    //intent.repor();
                }
            }
            else
            {
                Android.Support.V4.App.Fragment fragment = this.MyContext.SupportFragmentManager.FindFragmentById(Resource.Id.fragmentContainer);
                if (fragment != null && fragment is IndividualHomeFragment)
                { 
                    var indFrag = (IndividualHomeFragment)fragment;
                    if (indFrag != null)
                    {
                        var toasterFrag = (CheckIns.LiveToastersFragment)indFrag.LiveToastersFragment;
                        if (toasterFrag != null)
                        {
                            await toasterFrag.ReportSpam();
                        }
                    }
                }
            }
            this.Dismiss();
        }

        private void ReportInappropriate()
        {
            if (this.MyContext != null && this.MyContext is Activities.Individuals.SearchToasterProfileActivity)
            {
                var intent = (Activities.Individuals.SearchToasterProfileActivity)this.MyContext;
                if (intent != null)
                {
                    //await intent.UnfollowToaster();
                }
            }
            else
            {
                Android.Support.V4.App.Fragment fragment = this.MyContext.SupportFragmentManager.FindFragmentById(Resource.Id.fragmentContainer);
                if (fragment != null && fragment is IndividualHomeFragment)
                {
                    var indFrag = (IndividualHomeFragment)fragment;
                    if (indFrag != null)
                    {
                        var toasterFrag = (CheckIns.LiveToastersFragment)indFrag.LiveToastersFragment;
                        if (toasterFrag != null)
                        {
                            toasterFrag.ReportInappropriate();
                        }
                    }
                }
            }
            this.Dismiss();
        }

        #endregion


    }
}