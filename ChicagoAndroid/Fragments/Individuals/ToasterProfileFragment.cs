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
using Tabs.Mobile.Shared.Models.Individuals;
using Tabs.Mobile.Shared.Models;

namespace Tabs.Mobile.ChicagoAndroid.Fragments.Individuals
{
    public class ToasterProfileFragment : BaseIndividualsFragment
    {

        #region Constants, Enums, and Variables

        // Create a new instance field for this activity.
        private static ToasterProfileFragment instance;
        private TextView name;
        private TextView location;
        private TextView headline;
        private FrameLayout parentLayout;
        private Button toasterRequest;
        private ImageView profilePicture;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public static bool RequireRefresh { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static string UpdatedHeadline { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static string UpdatedHometown { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static bool ProfilePicUpdated { get; set; }

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

        #region Constructors

        public ToasterProfileFragment(Activities.Individuals.IndividualHomeActivity context)
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
            instance = new ToasterProfileFragment(context);

            return instance;
        }

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
            var view = inflater.Inflate(Resource.Layout.ToasterProfile, container, false);
            try
            {
                name = view.FindViewById<TextView>(Resource.Id.name);
                location = view.FindViewById<TextView>(Resource.Id.location);
                headline = view.FindViewById<TextView>(Resource.Id.headline);
                parentLayout = view.FindViewById<FrameLayout>(Resource.Id.toasterProfileLayout);
                toasterRequest = view.FindViewById<Button>(Resource.Id.toasterRequest);
                var fabBtn = view.FindViewById<FloatingActionButton>(Resource.Id.editProfile);
                profilePicture = view.FindViewById<ImageView>(Resource.Id.profilePic);
                this.PhotosRecycler = view.FindViewById<RecyclerView>(Resource.Id.toastersProfileRecycler);

                toasterRequest.Click += delegate
                {
                    this.HomeContext.StartActivity(typeof(Activities.Individuals.ToastersActivity));
                };

                fabBtn.Click += delegate
                {
                    this.HomeContext.StartActivity(typeof(Activities.Individuals.EditToasterProfileActivity));
                };

                GetToastersCount();

            } catch (Exception)
            {

            }

            return view;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task GetToastersCount()
        {
            try
            {
                if (this.HomeContext.CheckNetworkConnectivity() == null)
                {
                    toasterRequest.Text = "Toasters";
                }
                else
                {
                    var count = await App.ToastersFactory.GetTotalToastersCount(this.HomeContext.CurrentUser.UserId);
                    toasterRequest.Text = count + " Toasters";
                }
            }
            catch (Exception)
            {
                this.HomeContext.ShowProgressbar(false, "", ToastMessage.Searching);
            }
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
                Uri imageUri = new Uri(await Shared.Helpers.BlobStorageHelper.GetToasterPhotosUri(b.UserId, b.ToasterPhotoId));
                logo.ImageUrl = imageUri;
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
                if (this.HomeContext.CheckNetworkConnectivity() == null)
                {
                    this.HomeContext.ShowSnack(this.parentLayout, ToastMessage.NoInternet, "OK");
                    return;
                }
                else
                {
                    //BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);

                    //var userId = FromSearchedUser ? SearchedUser.UserId : this.CurrentUser.UserId;
                    var photos = await App.ToasterPhotoFactory.Get(this.HomeContext.CurrentUser.UserId);
                    if (photos != null)
                    {
                        this.ToasterPhotos = photos.ToList();
                        await GetPhotoUris();
                        this.ToasterPhotoAdaper = new ToasterPhotoAdaper(this, ToasterPhotos, ImageViewImages);
                        this.ToasterLayoutManager = new LinearLayoutManager(this.HomeContext);
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
        private async Task GetProfilePicture()
        {
            try
            {
                if (this.HomeContext.CheckNetworkConnectivity() == null)
                {
                    this.HomeContext.ShowSnack(this.parentLayout, ToastMessage.NoInternet, "OK");
                    return;
                }

                ImageViewImage itemLogo = new ImageViewImage();
                itemLogo.Id = this.HomeContext.CurrentUser.UserId;
                itemLogo.ImageUrl = await this.HomeContext.GetUserAvatarUri(this.HomeContext.CurrentUser.UserId);

                if (itemLogo.ImageUrl != null)
                {
                    if (itemLogo.ImageBitmap == null)
                    {
                        this.HomeContext.BeginDownloadingImage(itemLogo, profilePicture);
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
        public async override void OnStart()
        {
            try
            {
                base.OnStart();

                if (RequireRefresh)
                {
                    RequireRefresh = false;
                    this.HomeContext.CurrentUser = this.HomeContext.GetCurrentUser();
                    var fname = string.IsNullOrEmpty(this.HomeContext.CurrentUser.FirstName) ? string.Empty : this.HomeContext.CurrentUser.FirstName;
                    var lname = string.IsNullOrEmpty(this.HomeContext.CurrentUser.LastName) ? string.Empty : this.HomeContext.CurrentUser.LastName;
                    name.Text = fname + " " + lname;
                    location.Text = UpdatedHometown;
                    headline.Text = UpdatedHeadline;
                }

                if (ProfilePicUpdated)
                {
                    ProfilePicUpdated = false;
                    await GetProfilePicture();
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public async override void OnActivityCreated(Bundle savedInstanceState)
        {
            try
            {
                base.OnActivityCreated(savedInstanceState);


                var fname = string.IsNullOrEmpty(this.HomeContext.CurrentUser.FirstName) ? string.Empty : this.HomeContext.CurrentUser.FirstName;
                var lname = string.IsNullOrEmpty(this.HomeContext.CurrentUser.LastName) ? string.Empty : this.HomeContext.CurrentUser.LastName;
                name.Text = fname + " " + lname;
                this.HomeContext.SupportActionBar.Title = string.IsNullOrEmpty(this.HomeContext.CurrentUser.Username) ? "Profile" : this.HomeContext.CurrentUser.Username;


                await GetProfileInfo();
                await GetProfilePicture();
                await GetPhotos();
            }
            catch (Exception) { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task GetProfileInfo()
        {
            try
            {
                if (this.HomeContext.CheckNetworkConnectivity() == null)
                {
                    this.HomeContext.ShowSnack(this.parentLayout, ToastMessage.NoInternet, "OK");
                    return;
                }
                else
                {
                    this.HomeContext.ShowProgressbar(true, "", ToastMessage.Loading);
                    var toasterInfo = await App.IndividualFactory.GetToasterByUserId(this.HomeContext.CurrentUser.UserId);
                    if (toasterInfo != null)
                    {
                        location.Text = string.IsNullOrEmpty(toasterInfo.HomeTown) ? string.Empty : toasterInfo.HomeTown;
                        headline.Text = string.IsNullOrEmpty(toasterInfo.Headline) ? string.Empty : toasterInfo.Headline;
                    }
                    else
                    {
                        location.Text = string.Empty;
                        headline.Text = string.Empty;
                    }
                    this.HomeContext.ShowProgressbar(false, "", ToastMessage.Loading);
                }
            }
            catch (Exception)
            {
                this.HomeContext.ShowProgressbar(false, "", ToastMessage.Searching);
            }
        }

        #endregion

    }
}