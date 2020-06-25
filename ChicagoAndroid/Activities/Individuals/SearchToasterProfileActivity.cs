using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Content.PM;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Android.Views.InputMethods;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using V4Fragment = Android.Support.V4.App.Fragment;
using V4FragmentManager = Android.Support.V4.App.FragmentManager;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Tabs.Mobile.ChicagoAndroid.Adapters.Individuals;
using Tabs.Mobile.ChicagoAndroid.Fragments.Individuals;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models;
using Newtonsoft.Json;
using Tabs.Mobile.Shared.Models.Individuals;

namespace Tabs.Mobile.ChicagoAndroid.Activities.Individuals
{
    [Activity(Label = "Profile", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class SearchToasterProfileActivity : BaseActivity
    {

        #region Constants, Enums, and Variables

        private TextView name;
        private TextView location;
        private TextView headline;
        private FrameLayout parentLayout;
        private Button toasterRequest;
        private ImageView profilePicture;
        private Toasters ToasterInfo;
        private Android.Support.V7.App.AlertDialog alertDialog;
        private Android.Support.V7.App.AlertDialog.Builder builder;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public bool FromSearchedUser { get; set; }

        public bool FromToasters { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        public bool FromRequestPending { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static bool ProfilePicUpdated { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Shared.Models.Users.Users SearchedUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Toasters Toaster { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Individual IndividualInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Individual SearchedIndividualInfo { get; set; }

        /// <summary>
        /// Layout manager that lays out each card in the RecyclerView:
        /// </summary>
        private RecyclerView.LayoutManager ToasterLayoutManager { get; set; }

        /// <summary>
        /// Gets or sets the recycler view
        /// </summary>
        private RecyclerView PhotosRecycler { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ImageViewImage> ImageViewImages { get; set; } = new List<ImageViewImage>();

        public List<ToasterPhoto> ToasterPhotos { get; set; } = new List<ToasterPhoto>();

        /// <summary>
        /// Gets or sets the adapater
        /// </summary>
        private ToasterPhotoAdaper ToasterPhotoAdaper { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// This hook is called whenever an item in your options menu is selected.
        /// </summary>
        /// <Param name="item"></Param>
        /// <returns></returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    this.Finish();
                    break;
                case Resource.Id.done:
                    OpenActions();
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        /// <summary>
        /// 
        /// </summary>
        private void OpenActions()
        {
            var fragment = ToastersMoreBottomSheetFragment.NewInstance(this, ToastersMoreBottomSheetFragment.Caller.ToasterProfile);
            fragment.Show(this.SupportFragmentManager, "1");
        }

        public async Task BlockToaster()
        {
            try
            {
                if (this.CheckNetworkConnectivity() == null)
                {
                    this.ShowSnack(parentLayout, ToastMessage.NoInternet, "OK");
                    return;
                }
                else
                {
                    if (Toaster == null)
                    {
                        return;
                    }

                    await App.ToastersFactory.BlockToaster(this.CurrentUser.UserId, SearchedUser.UserId, this.CurrentUser.UserId);
                    toasterRequest.Visibility = ViewStates.Invisible;
                    ToastersActivity.RequiresRefresh = true;
                    this.Finish();
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
                if (this.CheckNetworkConnectivity() == null)
                {
                    this.ShowSnack(parentLayout, ToastMessage.NoInternet, "OK");
                    return;
                }
                else
                {
                    if (Toaster == null)
                    {
                        return;
                    }

                    await App.ToastersFactory.UnfollowToaster(this.CurrentUser.UserId, SearchedUser.UserId, this.CurrentUser.UserId);
                    toasterRequest.Visibility = ViewStates.Invisible;
                    ToastersActivity.RequiresRefresh = true;
                    this.Finish();
                }
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        private void CancelClicked(object sender, DialogClickEventArgs arg)
        {
            if (alertDialog != null)
            {
                alertDialog.Dismiss();
                alertDialog.Dispose();
                alertDialog = null;
            }
            this.Finish();
        }

        private async void ReportUserToTABS(object sender, DialogClickEventArgs arg)
        {
            if (alertDialog != null)
            {
                alertDialog.Dismiss();
                alertDialog.Dispose();
                alertDialog = null;
            }

            try
            {
                if (this.CheckNetworkConnectivity() == null)
                {
                    Toast.MakeText(this, ToastMessage.NoInternet, ToastLength.Short).Show();
                    return;
                }
                else
                {
                    Shared.Models.Reports.Users.ReportedUser reportedUser = new Shared.Models.Reports.Users.ReportedUser();
                    reportedUser.BlockedByAdmin = false;
                    reportedUser.BlockedByAdminUserId = 0;
                    reportedUser.ReportDate = DateTime.Now;
                    reportedUser.ReporterUserId = this.CurrentUser.UserId;
                    reportedUser.ReporterFirstName = this.CurrentUser.FirstName;
                    reportedUser.ReporterLastName = this.CurrentUser.LastName;

                    //var checkinuser = await App.UsersFactory.GetUser(SearchedUser.UserId);

                    if (SearchedUser != null)
                    {
                        reportedUser.SenderUserId = SearchedUser.UserId;
                        reportedUser.SenderFirstName = SearchedUser.FirstName;
                        reportedUser.SenderLastName = SearchedUser.LastName;
                    }

                    await App.ReportedUserFactory.ReportUser(reportedUser);

                    Toast.MakeText(this, ToastMessage.GenericAccountReportMessage, ToastLength.Long).Show();
                }
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        public void ReportUser()
        {

            if (alertDialog != null && alertDialog.IsShowing)
            {
                alertDialog.Dismiss();
                alertDialog.Dispose();
            }
            builder.SetTitle(ToastMessage.ReportUserTitle);
            builder.SetMessage(ToastMessage.ReportUserMessage);
            builder.SetCancelable(false);
            builder.SetPositiveButton(AppText.Report, ReportUserToTABS);
            builder.SetNegativeButton(AppText.Cancel, CancelClicked);
            alertDialog = builder.Create();
            alertDialog.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.more_menu, menu);

            return base.OnCreateOptionsMenu(menu);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.ToasterProfile);

                this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                this.SupportActionBar.SetDisplayShowHomeEnabled(true);

                name = FindViewById<TextView>(Resource.Id.name);
                location = FindViewById<TextView>(Resource.Id.location);
                headline = FindViewById<TextView>(Resource.Id.headline);
                parentLayout = FindViewById<FrameLayout>(Resource.Id.toasterProfileLayout);
                toasterRequest = FindViewById<Button>(Resource.Id.toasterRequest);
                var fabBtn = FindViewById<FloatingActionButton>(Resource.Id.editProfile);
                profilePicture = FindViewById<ImageView>(Resource.Id.profilePic);
                this.PhotosRecycler = FindViewById<RecyclerView>(Resource.Id.toastersProfileRecycler);

                this.FromSearchedUser = Intent.GetBooleanExtra("FromSearchedUser", false);
                this.FromRequestPending = Intent.GetBooleanExtra("FromRequestPending", false);
                this.SearchedUser = JsonConvert.DeserializeObject<Shared.Models.Users.Users>(Intent.GetStringExtra("SearchedUser"));
                if (FromSearchedUser)
                {
                    fabBtn.Visibility = ViewStates.Gone;
                }

                this.FromToasters = Intent.GetBooleanExtra("FromToasters", false);
                if (FromToasters)
                {
                    this.Toaster = JsonConvert.DeserializeObject<Toasters>(Intent.GetStringExtra("Toaster"));
                }

                name.Text = SearchedUser != null ? SearchedUser.FirstName : string.Empty;
                this.SupportActionBar.Title = string.IsNullOrEmpty(SearchedUser.Username) ? "Profile" : SearchedUser.Username;

                await GetProfileInfo();
                await GetProfilePicture();
                await GetPhotos();

                toasterRequest.Click += async delegate
                {
                    if (toasterRequest.Text == AppText.AcceptRequest)
                    {
                        await AcceptRequest();
                    }
                    else
                    {
                        await SendAddToaster();
                    }
                };

                builder = new Android.Support.V7.App.AlertDialog.Builder(this);

                if (this.IndividualInfo != null)
                {

                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task GetPhotoUris()
        {
            foreach (var b in this.ToasterPhotos)
            {
                ImageViewImage logo = new ImageViewImage();
                logo.Id = b.ToasterPhotoId;
                var uriString = await Shared.Helpers.BlobStorageHelper.GetToasterPhotosUri(b.UserId, b.ToasterPhotoId);
                if (!string.IsNullOrEmpty(uriString))
                {
                    Uri imageUri = new Uri(uriString);
                    logo.ImageUrl = imageUri;
                }
                this.ImageViewImages.Add(logo);
            }
        }

        /// <summary> 
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task GetPhotos()
        {
            try
            {
                if (this.CheckNetworkConnectivity() == null)
                {
                    this.ShowSnack(this.parentLayout, ToastMessage.NoInternet, "OK");
                    return;
                }
                else
                {
                    //BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);

                    var userId = FromSearchedUser ? SearchedUser.UserId : this.CurrentUser.UserId;
                    var photos = await App.ToasterPhotoFactory.Get(userId);
                    if (photos != null)
                    {
                        this.ToasterPhotos = photos.ToList();
                        await GetPhotoUris();
                        this.ToasterPhotoAdaper = new ToasterPhotoAdaper(this, ToasterPhotos, ImageViewImages);
                        this.ToasterLayoutManager = new LinearLayoutManager(this);
                        this.PhotosRecycler.SetItemAnimator(new DefaultItemAnimator());
                        this.PhotosRecycler.AddItemDecoration(new Helpers.PhotoItemDecorator(10));
                        this.PhotosRecycler.HasFixedSize = true;
                        this.PhotosRecycler.SetLayoutManager(this.ToasterLayoutManager);
                        this.PhotosRecycler.SetAdapter(this.ToasterPhotoAdaper);
                    }

                    //BTProgressHUD.Dismiss();
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                //BTProgressHUD.Dismiss();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task AcceptRequest()
        {
            try
            {
                if (this.CheckNetworkConnectivity() == null)
                {
                    this.ShowSnack(parentLayout, ToastMessage.NoInternet, "OK");
                    return;
                }
                else
                {
                    if (SearchedUser == null)
                    {
                        return;
                    }

                    await App.ToastersFactory.AcceptRequest(SearchedUser.UserId, this.CurrentUser.UserId, SearchedUser.UserId, Toasters.ToasterRequestStatus.Accepted);
                    toasterRequest.Visibility = ViewStates.Invisible;
                    ToastersActivity.RequiresRefresh = true;
                    await new Shared.Helpers.PushNotificationHelper(App.NotificationRegisterFactory, Shared.Helpers.PushNotificationHelper.PushPlatform.Android).ToasterAcceptedPush(this.CurrentUser.FirstName, SearchedUser.UserId);
                    this.Finish();
                }
            }
            catch (Exception){}
        }

        private async Task SendAddToaster()
        {
            try
            {
                if(toasterRequest.Text == AppText.Requested)
                {
                    return;
                }
                if (this.CheckNetworkConnectivity() == null)
                {
                    this.ShowSnack(parentLayout, ToastMessage.NoInternet, "OK");
                    return;
                }
                else
                {
                    if (SearchedUser == null)
                    {
                        return;
                    }

                    Toasters toaster = new Toasters();
                  
                    toaster.RequestStatus = this.SearchedIndividualInfo != null && !this.SearchedIndividualInfo.PrivateAccount ? Toasters.ToasterRequestStatus.Accepted : Toasters.ToasterRequestStatus.Pending;
                    toaster.UserOneId = this.CurrentUser.UserId;
                    toaster.UserTwoId = SearchedUser.UserId;
                    toaster.ActionUserId = this.CurrentUser.UserId;

                    await App.ToastersFactory.AddToaster(toaster);

                    if(this.IndividualInfo != null)
                    {
                        if (this.IndividualInfo.PrivateAccount)
                        {
                            toasterRequest.Text = AppText.Requested;
                            await new Shared.Helpers.PushNotificationHelper(App.NotificationRegisterFactory, Shared.Helpers.PushNotificationHelper.PushPlatform.Android).ToasterRequestPush(this.CurrentUser.FirstName, SearchedUser.UserId);
                        }
                        else
                        {
                            toasterRequest.Visibility = ViewStates.Invisible;
                            await new Shared.Helpers.PushNotificationHelper(App.NotificationRegisterFactory, Shared.Helpers.PushNotificationHelper.PushPlatform.Android).ToasterAddedPush(this.CurrentUser.FirstName, SearchedUser.UserId);
                        }
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
        /// <returns></returns>
        private async Task GetProfilePicture()
        {
            try
            {
                if (this.CheckNetworkConnectivity() == null)
                {
                    this.ShowSnack(this.parentLayout, ToastMessage.NoInternet, "OK");
                    return;
                }

                ImageViewImage itemLogo = new ImageViewImage();
                var userId = SearchedUser != null ? SearchedUser.UserId : 0;
                itemLogo.Id = this.CurrentUser.UserId;
                itemLogo.ImageUrl = await GetUserAvatarUri(userId);

                if (itemLogo.ImageUrl != null)
                {
                    if (itemLogo.ImageBitmap == null)
                    {
                        this.BeginDownloadingImage(itemLogo, profilePicture);
                    }

                    profilePicture.SetImageBitmap(itemLogo.ImageBitmap);
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
        private async Task GetProfileInfo()
        {
            try
            {
                if (this.CheckNetworkConnectivity() == null)
                {
                    this.ShowSnack(this.parentLayout, ToastMessage.NoInternet, "OK");
                    return;
                }
                else
                {
                    this.ShowProgressbar(true, "", ToastMessage.Loading);
                    this.IndividualInfo = await App.IndividualFactory.GetToasterByUserId(this.CurrentUser.UserId);
                    //var userId = SearchedUser != null ? SearchedUser.UserId : 0;
                    this.SearchedIndividualInfo = await App.IndividualFactory.GetToasterByUserId(SearchedUser.UserId);
                    if (this.SearchedIndividualInfo != null)
                    {
                        location.Text = string.IsNullOrEmpty(this.SearchedIndividualInfo.HomeTown) ? string.Empty : this.SearchedIndividualInfo.HomeTown;
                        headline.Text = string.IsNullOrEmpty(this.SearchedIndividualInfo.Headline) ? string.Empty : this.SearchedIndividualInfo.Headline;

                        var individualId = this.IndividualInfo != null ? this.IndividualInfo.IndividualId : 0;

                        ToasterInfo = await App.ToastersFactory.Connected(this.CurrentUser.UserId, SearchedUser.UserId);

                        if (FromRequestPending)
                        {
                            toasterRequest.Text = AppText.AcceptRequest;
                        }
                        else if (ToasterInfo != null)
                        {
                            if(ToasterInfo.ToastersId > 0)
                            {
                                if (ToasterInfo.RequestStatus == Toasters.ToasterRequestStatus.Accepted)
                                {
                                    toasterRequest.Visibility = ViewStates.Invisible;
                                }                       
                                else
                                {
                                    toasterRequest.Text = AppText.Requested;
                                }
                            }
                        }
                    }
                    else
                    {
                        location.Text = string.Empty;
                        headline.Text = string.Empty;
                    }
                    this.ShowProgressbar(false, "", ToastMessage.Loading);
                }
            }
            catch (Exception)
            {
                this.ShowProgressbar(false, "", ToastMessage.Searching);
            }
        }


        #endregion

    }
}