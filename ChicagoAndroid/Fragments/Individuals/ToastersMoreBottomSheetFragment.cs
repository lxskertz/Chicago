using Android.Support.Design.Widget;
using Android.OS;
using Android.Views;
using Android.Widget;
using Tabs.Mobile.Shared.Resources;

namespace Tabs.Mobile.ChicagoAndroid.Fragments.Individuals
{
    public class ToastersMoreBottomSheetFragment : BottomSheetDialogFragment
    {

        #region Constants, Enums, and Variables

        private TextView reportPost;
        private TextView reportUser;
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

        public ToastersMoreBottomSheetFragment() { }

        public ToastersMoreBottomSheetFragment(Activities.BaseActivity context, Caller dialogCaller)
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
            var view =  inflater.Inflate(Resource.Layout.ToastersBottomSheet, container, false);
            reportPost = view.FindViewById<TextView>(Resource.Id.reportPost);
            reportUser = view.FindViewById<TextView>(Resource.Id.reportUser);
            block = view.FindViewById<TextView>(Resource.Id.blockUser);
            unfollow = view.FindViewById<TextView>(Resource.Id.unfollowUser);

            if(this.DialogCaller == Caller.LiveToasters)
            {
                reportUser.Visibility = ViewStates.Gone;
            }
            else
            {
                reportPost.Visibility = ViewStates.Gone;
                HideShowOptions();
            }

            block.Click += delegate { BlockUser(); };
            unfollow.Click += delegate { Unfollow(); };
            reportPost.Click += delegate { ReportPost(); };
            reportUser.Click += delegate { ReportUser(); };

            return view;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public static ToastersMoreBottomSheetFragment NewInstance(Activities.BaseActivity context, Caller caller)
        {
            ToastersMoreBottomSheetFragment fragment = new ToastersMoreBottomSheetFragment(context, caller);
            return fragment;
        }

        /// <summary>
        /// 
        /// </summary>
        private async void BlockUser()
        {
            if (this.MyContext != null && this.MyContext is Activities.Individuals.SearchToasterProfileActivity)
            {
                var intent = (Activities.Individuals.SearchToasterProfileActivity)this.MyContext;
                if (intent != null)
                {
                    await intent.BlockToaster();
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
                            await toasterFrag.BlockToaster();
                        }
                    }
                }
            }
            this.Dismiss();
        }

        private async void Unfollow()
        {
            if (this.MyContext != null && this.MyContext is Activities.Individuals.SearchToasterProfileActivity)
            {
                var intent = (Activities.Individuals.SearchToasterProfileActivity)this.MyContext;
                if (intent != null)
                {
                    await intent.UnfollowToaster();
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
                            await toasterFrag.UnfollowToaster();
                        }
                    }
                }
            }
            this.Dismiss();
        }

        private void ReportUser() 
        {
            if (this.MyContext != null && this.MyContext is Activities.Individuals.SearchToasterProfileActivity)
            {
                var intent = (Activities.Individuals.SearchToasterProfileActivity)this.MyContext;
                if (intent != null)
                {
                    intent.ReportUser();
                }
            }
            this.Dismiss();
        }

        private void ReportPost()
        {
            this.Dismiss();
            this.Dispose();
            var fragment = ReportBottomSheetFragment.NewInstance(this.MyContext, (ReportBottomSheetFragment.Caller)this.DialogCaller); //.Caller.LiveToasters);
            fragment.Show(this.MyContext.SupportFragmentManager, "1");
        }

        /// <summary>
        /// 
        /// </summary>
        private void HideShowOptions()
        {
            if (this.MyContext != null && this.MyContext is Activities.Individuals.SearchToasterProfileActivity)
            {
                var intent = (Activities.Individuals.SearchToasterProfileActivity)this.MyContext;
                if(intent != null && intent.Toaster != null)
                {
                    var blockText = intent.Toaster.RequestStatus == Shared.Models.Individuals.Toasters.ToasterRequestStatus.Blocked ? AppText.Unblock : AppText.Block;
                    block.Text = blockText;

                    unfollow.Visibility = intent.Toaster.RequestStatus == Shared.Models.Individuals.Toasters.ToasterRequestStatus.Accepted ? ViewStates.Visible : ViewStates.Gone;
                } else
                {
                    block.Visibility = ViewStates.Gone;
                    unfollow.Visibility = ViewStates.Gone;
                }
                ///call refresh require method that calls 
            }
        }

        #endregion

    }
}