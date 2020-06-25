using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using V4Fragment = Android.Support.V4.App.Fragment;
using V4FragmentManager = Android.Support.V4.App.FragmentManager;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Tabs.Mobile.ChicagoAndroid.Adapters.Individuals.CheckIns;
using Tabs.Mobile.ChicagoAndroid.Fragments.Individuals;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.CheckIns;
using Tabs.Mobile.Shared.Models.Events;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Models.Reports.Spams;
using Newtonsoft.Json;

namespace Tabs.Mobile.ChicagoAndroid.Fragments.Individuals.CheckIns
{
    public class LiveToastersFragment : BaseIndividualsFragment
    {

        #region Constants, Enums, and Variables

        // Create a new instance field for this activity.
        private static LiveToastersFragment instance;
        public SwipeRefreshLayout refresher;
        public FrameLayout pageLayout;
        private TextView noResultMsg;
        private Android.Support.V7.App.AlertDialog alertDialog;
        private Android.Support.V7.App.AlertDialog.Builder builder;

        #endregion

        #region Properties

        /// <summary>
        /// Layout manager that lays out each card in the RecyclerView:
        /// </summary>
        private RecyclerView.LayoutManager CheckInsListLayoutManager { get; set; }

        /// <summary>
        /// Gets or sets the recycler view
        /// </summary>
        private RecyclerView CheckInsRecycler { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<CheckIn> CheckIns { get; set; } = new List<CheckIn>();

        /// <summary>
        /// 
        /// </summary>
        public List<ImageViewImage> ImageViewImage { get; set; } = new List<ImageViewImage>();

        /// <summary>
        /// Gets or sets the adapater
        /// </summary>
        private LiveToastersAdapter LiveToastersAdapter { get; set; }

        public Dictionary<int, bool> LikeList = new Dictionary<int, bool>();

        public static bool RequiresRefresh { get; set; }

        #endregion

        #region Constructors

        public LiveToastersFragment(Activities.Individuals.IndividualHomeActivity context)
        {
            this.HomeContext = context;
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
            HasOptionsMenu = true;
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
            var view = inflater.Inflate(Resource.Layout.IndividualLiveToasters, container, false);

            try
            {
                this.CheckInsRecycler = view.FindViewById<RecyclerView>(Resource.Id.liveToastersCardRecycler);
                pageLayout = view.FindViewById<FrameLayout>(Resource.Id.liveToastersLayout);
                noResultMsg = view.FindViewById<TextView>(Resource.Id.noResultMessage);

                refresher = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefresh);
                refresher.Refresh += HandleRefresh;

                LoadData();
                this.HomeContext.SupportActionBar.Title = "Live";
                builder = new Android.Support.V7.App.AlertDialog.Builder(this.HomeContext);

            }
            catch (Exception)
            {
            }

            return view;
        }

        public override void OnResume()
        {
            try
            {
                base.OnResume();
                if (RequiresRefresh)
                {
                    RequiresRefresh = false;
                    if (this.HomeContext.CheckNetworkConnectivity() == null)
                    {
                        Toast.MakeText(this.HomeContext, ToastMessage.NoInternet, ToastLength.Short).Show();
                        return;
                    }
                    BindAdapter();
                }
            }
            catch (Exception)
            {

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="inflater"></param>
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            //try
            //{
            //inflater.Inflate(Resource.Menu.check_in_menu, menu);
            inflater.Inflate(Resource.Menu.home_action, menu);

            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_checkIn:
                    Intent activity = new Intent(this.HomeContext, typeof(Activities.Businesses.BusinessesActivity));
                    this.StartActivity(activity);
                    break;
                case Resource.Id.action_receivedDrinks:
                    Intent intent = new Intent(this.HomeContext, typeof(Activities.Orders.OrdersActivity));
                    intent.PutExtra("ToasterOrderEnum", (int)Shared.Models.Orders.ToasterOrder.ToasterOrderEnum.Receiver);
                    this.HomeContext.StartActivity(intent);
                    break;
                case Resource.Id.action_sentDrinks:
                    Intent intentt = new Intent(this.HomeContext, typeof(Activities.Orders.OrdersActivity));
                    intentt.PutExtra("ToasterOrderEnum", (int)Shared.Models.Orders.ToasterOrder.ToasterOrderEnum.Sender);
                    this.HomeContext.StartActivity(intentt);
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void HideShowNoResult(bool show, bool checkIn)
        {
            noResultMsg.Visibility = show ? ViewStates.Visible : ViewStates.Gone;
            noResultMsg.Text = checkIn ? ToastMessage.NoLiveCheckin : ToastMessage.NoLiveEvent;
            this.CheckInsRecycler.Visibility = show ? ViewStates.Gone : ViewStates.Visible; //!NoResultMessage.Hidden;
        }

        /// <summary>
        /// Load data
        /// </summary>
        private async Task LoadData()
        {
            try
            {
                if (this.HomeContext.CheckNetworkConnectivity() == null)
                {
                    this.HomeContext.ShowSnack(pageLayout, ToastMessage.NoInternet, "OK");
                }
                else
                {
                    this.HomeContext.ShowProgressbar(true, "", ToastMessage.Loading);

                    var checkIns = await App.CheckInFactory.GetCheckIns(this.HomeContext.CurrentUser.UserId);
                    if (checkIns != null && checkIns.Count > 0)
                    {
                        HideShowNoResult(false, true);
                        this.CheckIns = checkIns.ToList();
                        await GetCheckInPicUris();
                        await GetLikeCheckInList();

                        this.LiveToastersAdapter = new LiveToastersAdapter(this.HomeContext, this.CheckIns, this, this.ImageViewImage);
                        this.CheckInsListLayoutManager = new LinearLayoutManager(this.HomeContext);
                        this.CheckInsRecycler.SetItemAnimator(new DefaultItemAnimator());
                        this.CheckInsRecycler.HasFixedSize = true;
                        this.CheckInsRecycler.SetLayoutManager(this.CheckInsListLayoutManager);
                        this.CheckInsRecycler.SetAdapter(this.LiveToastersAdapter);
                    }
                    else
                    {
                        HideShowNoResult(true, true);
                    }

                    this.HomeContext.ShowProgressbar(false, "", ToastMessage.Loading);
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                this.HomeContext.ShowProgressbar(false, "", ToastMessage.Loading);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task GetCheckInPicUris()
        {
            try
            {
                foreach (var b in this.CheckIns)
                {

                    ImageViewImage logo = new ImageViewImage();
                    logo.Id = b.CheckInId;
                    //Uri imageUri = new Uri(await Shared.Helpers.BlobStorageHelper.GetCheckIntLogoUri(b.CheckInId));

                    var uriString = await Shared.Helpers.BlobStorageHelper.GetCheckIntLogoUri(b.CheckInId);
                    if (!string.IsNullOrEmpty(uriString))
                    {
                        Uri imageUri = new Uri(uriString);
                        logo.ImageUrl = imageUri;
                    }
                    else
                    {

                        var userUriString = await Shared.Helpers.BlobStorageHelper.GetToasterBlobUri(b.UserId);
                        if (!string.IsNullOrEmpty(userUriString))
                        {
                            Uri imageUri = new Uri(userUriString);
                            logo.ImageUrl = imageUri;
                            //this.CheckInsImageViewImage.Add(logo);
                        }
                    }
                    this.ImageViewImage.Add(logo);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task GetLikeCheckInList()
        {
            try
            {
                foreach (var b in this.CheckIns)
                {
                    var liked = await App.CheckInLikesFactory.GetCheckInLike(this.HomeContext.CurrentUser.UserId, b.CheckInId);

                    if (liked != null)
                    {
                        AddRemoveLike(liked.Liked, b.CheckInId);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="like"></param>
        /// <param name="eventId"></param>
        public void AddRemoveLike(bool like, int checkinId)
        {
            if (LikeList.ContainsKey(checkinId))
            {
                LikeList[checkinId] = like;
            }
            else
            {
                LikeList.Add(checkinId, like);
            }
        }

        private async void BindAdapter()
        {
            var checkIns = await App.CheckInFactory.GetCheckIns(this.HomeContext.CurrentUser.UserId);
            if (checkIns != null && checkIns.Count > 0)
            {
                HideShowNoResult(false, true);
                this.CheckIns = checkIns.ToList();
                await GetCheckInPicUris();
                await GetLikeCheckInList();

                if (LiveToastersAdapter == null)
                {
                    this.LiveToastersAdapter = new LiveToastersAdapter(this.HomeContext, this.CheckIns, this, this.ImageViewImage);
                    this.CheckInsListLayoutManager = new LinearLayoutManager(this.HomeContext);
                    this.CheckInsRecycler.SetItemAnimator(new DefaultItemAnimator());
                    this.CheckInsRecycler.HasFixedSize = true;
                    this.CheckInsRecycler.SetLayoutManager(this.CheckInsListLayoutManager);
                    this.CheckInsRecycler.SetAdapter(this.LiveToastersAdapter);
                }
                else
                {
                    this.LiveToastersAdapter.Rows = this.CheckIns;
                    this.LiveToastersAdapter.ImageViewImage = this.ImageViewImage;
                    this.HomeContext.RunOnUiThread(() =>
                    {
                        this.LiveToastersAdapter.NotifyDataSetChanged();
                    });
                }
            }
            else
            {
                HideShowNoResult(true, true);
            }
        }


        public async Task BlockToaster()
        {
            try
            {
                if (this.HomeContext.CheckNetworkConnectivity() == null)
                {
                    this.HomeContext.ShowSnack(pageLayout, ToastMessage.NoInternet, "OK");
                    return;
                }
                else
                {
                    if (this.LiveToastersAdapter.SelectedItem == null)
                    {
                        return;
                    }

                    await App.ToastersFactory.BlockToaster(this.HomeContext.CurrentUser.UserId, this.LiveToastersAdapter.SelectedItem.UserId, this.HomeContext.CurrentUser.UserId);
                    HandleRefresh(null, null);
                }
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        public async Task UnfollowToaster()
        {
            try
            {
                if (this.HomeContext.CheckNetworkConnectivity() == null)
                {
                    this.HomeContext.ShowSnack(pageLayout, ToastMessage.NoInternet, "OK");
                    return;
                }
                else
                {
                    if (this.LiveToastersAdapter.SelectedItem == null)
                    {
                        return;
                    }

                    await App.ToastersFactory.UnfollowToaster(this.HomeContext.CurrentUser.UserId, this.LiveToastersAdapter.SelectedItem.UserId, this.HomeContext.CurrentUser.UserId);
                    HandleRefresh(null, null);
                }
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        public void ReportInappropriate()
        {
            Intent intent = new Intent(this.HomeContext, typeof(Activities.Reports.InappropraiteOptionsActivity));
            intent.PutExtra("CheckInItem", JsonConvert.SerializeObject(this.LiveToastersAdapter.SelectedItem));
            this.HomeContext.StartActivity(intent);
        }

        private void CancelClicked(object sender, DialogClickEventArgs arg)
        {
            if (alertDialog != null)
            {
                alertDialog.Dismiss();
                alertDialog.Dispose();
                alertDialog = null;
            }
        }

        public async Task ReportSpam()
        {
            try
            {
                if (this.HomeContext.CheckNetworkConnectivity() == null)
                {
                    this.HomeContext.ShowSnack(pageLayout, ToastMessage.NoInternet, "OK");
                    return;
                }
                else
                {
                    ReportedSpamCheckIn spamCheckIn = new ReportedSpamCheckIn();
                    spamCheckIn.BlockedByAdmin = false;
                    spamCheckIn.BlockedByAdminUserId = 0;
                    spamCheckIn.BusinessId = this.LiveToastersAdapter.SelectedItem.BusinessId;
                    spamCheckIn.BusinessName = this.LiveToastersAdapter.SelectedItem.BusinessName;
                    spamCheckIn.CheckInDate = this.LiveToastersAdapter.SelectedItem.CheckInDate;
                    spamCheckIn.CheckInDateString = this.LiveToastersAdapter.SelectedItem.CheckInDate.ToString();
                    spamCheckIn.CheckInId = this.LiveToastersAdapter.SelectedItem.CheckInId;
                    spamCheckIn.CheckInType = this.LiveToastersAdapter.SelectedItem.CheckInType;
                    spamCheckIn.CheckInUserId = this.LiveToastersAdapter.SelectedItem.UserId;
                    spamCheckIn.EventId = this.LiveToastersAdapter.SelectedItem.EventId;
                    spamCheckIn.ReporterUserId = this.HomeContext.CurrentUser.UserId;
                    spamCheckIn.ReporterFirstName = this.HomeContext.CurrentUser.FirstName;
                    spamCheckIn.ReporterLastName = this.HomeContext.CurrentUser.LastName;

                    var checkinuser = await App.UsersFactory.GetUser(this.LiveToastersAdapter.SelectedItem.UserId);

                    if(checkinuser != null)
                    {
                        spamCheckIn.SenderFirstName = checkinuser.FirstName;
                        spamCheckIn.SenderLastName = checkinuser.LastName;
                    }

                    await App.ReportedSpamCheckInFactory.ReportSpam(spamCheckIn);

                    if (alertDialog != null && alertDialog.IsShowing)
                    {
                        alertDialog.Dismiss();
                        alertDialog.Dispose();
                    }
                    builder.SetMessage(ToastMessage.SpamReportMessage);
                    builder.SetCancelable(false);
                    builder.SetPositiveButton(AppText.Ok, CancelClicked);
                    alertDialog = builder.Create();
                    alertDialog.Show();
                }
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleRefresh(object sender, EventArgs e)
        {
            try
            {
                if (this.HomeContext.CheckNetworkConnectivity() == null)
                {
                    this.HomeContext.ShowSnack(pageLayout, ToastMessage.NoInternet, "OK");
                }
                else
                {
                    BindAdapter();
                }
            }
            catch (Exception ex)
            {
                refresher.Refreshing = false;
            }
            refresher.Refreshing = false;
        }

        #endregion

    }
}