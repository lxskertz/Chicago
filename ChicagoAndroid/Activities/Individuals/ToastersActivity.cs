using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Content.PM;
using Android.Views.InputMethods;
using Android.Support.V7.Widget;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using V4Fragment = Android.Support.V4.App.Fragment;
using V4FragmentManager = Android.Support.V4.App.FragmentManager;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Tabs.Mobile.ChicagoAndroid.Adapters.Individuals;
using Tabs.Mobile.ChicagoAndroid.Fragments.Individuals;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Models.Individuals;

namespace Tabs.Mobile.ChicagoAndroid.Activities.Individuals
{
    [Activity(Label = "Toasters", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class ToastersActivity : BaseActivity
    {

        #region Constants, Enums, and Variables

        public SearchParameters param = new SearchParameters();
        private IMenuItem requestMenu;
        public bool pendingRequestShown;

        #endregion

        #region Properties

        /// Gets or sets the search view
        /// </summary>
        public Android.Support.V7.Widget.SearchView SearchView { get; set; }

        /// <summary>
        /// Layout manager that lays out each card in the RecyclerView:
        /// </summary>
        private RecyclerView.LayoutManager ListLayoutManager { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private Android.Support.V7.Widget.PopupMenu ActionsMenu { get; set; }

        /// <summary>
        /// Gets or sets the recycler view
        /// </summary>
        private RecyclerView ToastersSearchRecycler { get; set; }

        private ICollection<Toasters> Toasters { get; set; }

        /// <summary>
        /// Gets or sets the adapter
        /// </summary>
        private ToastersAdapter ToastersAdapter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalResultCount { get; set; }

        public List<ImageViewImage> ImageViewImages { get; set; } = new List<ImageViewImage>();

        public Individual IndividualInfo { get; set; }

        public static bool RequiresRefresh { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.Toasters);

                //add the back arrow
                this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                this.SupportActionBar.SetDisplayShowHomeEnabled(true);

                this.ToastersSearchRecycler = FindViewById<RecyclerView>(Resource.Id.toastersRecycler);
                var inviteContactbtn = FindViewById<Button>(Resource.Id.inviteContactBtn);

                inviteContactbtn.Click += delegate
                {
                    Intent intent = new Intent(this, typeof(InviteContactActivity));
                    this.StartActivity(intent);
                };

                await RetriveToasters();
            }
            catch (Exception)
            {
            }
        }

        protected async override void OnResume()
        {
            try
            {
                base.OnResume();

                if (RequiresRefresh)
                {
                    RequiresRefresh = false;
                    await RetriveToasters();
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Initialize recycler view
        /// </summary>
        private void InitRecyclerView(List<Toasters> data)
        {
            this.ToastersAdapter = new ToastersAdapter(this, data, this.ImageViewImages);
            this.ListLayoutManager = new LinearLayoutManager(this);
            this.ToastersSearchRecycler.SetItemAnimator(new DefaultItemAnimator());
            this.ToastersSearchRecycler.HasFixedSize = true;
            this.ToastersSearchRecycler.SetLayoutManager(this.ListLayoutManager);
            this.ToastersSearchRecycler.AddOnScrollListener(new Listeners.Individuals.ToastersSearchScrollListener(this, this.ToastersAdapter, Listeners.Individuals.ToastersSearchScrollListener.Caller.Toasters));
            this.ToastersSearchRecycler.SetAdapter(this.ToastersAdapter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        private void InitSearchParameters(string searchTerm)
        {
            param = new SearchParameters();
            param.PageSize = 15;
            param.SearchTerm = searchTerm;
            param.PageNumber = 0;
            param.Id = this.CurrentUser.UserId; //this.IndividualInfo != null ? this.IndividualInfo.IndividualId : 0;
        }

        private async void ResetTable(View view)
        {
            if (!pendingRequestShown || (requestMenu !=null && requestMenu.TitleFormatted.ToString() == AppText.More))
            {
                //await GetPendingRequest();
                DisplayActionsMenu(view);
            }
            else
            {
                this.Title = "Toasters";
                await RetriveToasters();
            }
        }

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
                case Resource.Id.menuAction:
                    var menuItemView = FindViewById(Resource.Id.menuAction);
                    ResetTable(menuItemView);
                    //DisplayActionsMenu();
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        /// <summary>
        /// Display category menu
        /// </summary>
        private void DisplayActionsMenu(View view)
        {
            if (this.ActionsMenu != null)
            {
                this.ActionsMenu.Dismiss();
                this.ActionsMenu.Dispose();
            }
            this.ActionsMenu = new Android.Support.V7.Widget.PopupMenu(this, view, (int)GravityFlags.Start);
            try
            {
                this.ActionsMenu.Inflate(Resource.Menu.toasters_menu);
                IMenu menu = this.ActionsMenu.Menu;

                this.ActionsMenu.MenuItemClick += async (sender, e) => {
                    switch (e.Item.ItemId)
                    {
                        case Resource.Id.pendingRequests:
                            await GetPendingRequest();
                            //ResetTable();
                            break;
                        case Resource.Id.blockedToasters:
                            Intent intent = new Intent(this, typeof(BlockedToastersActivity));
                            this.StartActivity(intent);
                            break;
                    }
                };
                this.ActionsMenu.Show();
            }
            catch (Exception) { }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.menu_with_text, menu);
            requestMenu = menu.FindItem(Resource.Id.menuAction);
            requestMenu.SetTitle(AppText.More);

            return base.OnCreateOptionsMenu(menu);
        }

        public async Task GetPicUris()
        {
            try
            {
                foreach (var b in this.Toasters)
                {
                    ImageViewImage itemLogo = new ImageViewImage();
                    //var userId = b.UserOneId == this.CurrentUser.UserId ? b.UserTwoId : b.UserOneId;
                    itemLogo.Id = b.UserId;

                    var uriString = await Shared.Helpers.BlobStorageHelper.GetToasterBlobUri(b.UserId);
                    if (!string.IsNullOrEmpty(uriString))
                    {
                        Uri imageUri = new Uri(uriString);
                        itemLogo.ImageUrl = imageUri;
                        this.ImageViewImages.Add(itemLogo);
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
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public async Task GetPendingRequest()
        {
            try
            {
                if (this.CheckNetworkConnectivity() == null)
                {
                    this.ShowSnack(this.ToastersSearchRecycler, ToastMessage.NoInternet, "OK");
                    return;
                }
                else
                {
                    //this.SearchPerformed = true;
                    InitSearchParameters("");
                    this.ShowProgressbar(true, "", ToastMessage.Loading);
                    Toasters = await App.ToastersFactory.GetPendingToasters(param);

                    if (Toasters != null && Toasters.Count > 0)
                    {
                        this.Title = "Requests";
                        pendingRequestShown = true;
                        requestMenu.SetTitle(AppText.AcceptedRequest);
                        await GetPicUris();
                        if (ToastersAdapter == null)
                        {
                            InitRecyclerView(Toasters.ToList());
                        }
                        else
                        {
                            this.RunOnUiThread(() =>
                            {
                                this.ToastersAdapter.Rows = Toasters.ToList();
                                this.ToastersAdapter.ImageViewImages = this.ImageViewImages;
                                this.ToastersAdapter.NotifyDataSetChanged();
                            });
                        }
                        this.ToastersAdapter.LoadMore = true;
                    }
                    else
                    {
                        this.ShowSnack(this.ToastersSearchRecycler, ToastMessage.NoPendingRequests, "OK");
                    }
                }
                this.ShowProgressbar(false, "", ToastMessage.Searching);
            }
            catch (Exception)
            {
                this.ShowProgressbar(false, "", ToastMessage.Searching);
                this.ShowSnack(this.ToastersSearchRecycler, ToastMessage.ServerError, "OK");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task RetriveToasters()
        {
            try
            {
                if (this.CheckNetworkConnectivity() == null)
                {
                    this.ShowSnack(this.ToastersSearchRecycler, ToastMessage.NoInternet, "OK");
                    return;
                }
                else
                {
                    this.ShowProgressbar(true, "", ToastMessage.Loading);
                    this.IndividualInfo = await App.IndividualFactory.GetToasterByUserId(this.CurrentUser.UserId);
                    InitSearchParameters("");
                    Toasters = await App.ToastersFactory.GetToasters(param);

                    if (Toasters != null && Toasters.Count > 0)
                    {
                        pendingRequestShown = false;
                        requestMenu.SetTitle(AppText.More);
                        await GetPicUris();
                        if (ToastersAdapter == null)
                        {
                            InitRecyclerView(Toasters.ToList());
                        }
                        else
                        {
                            this.RunOnUiThread(() =>
                            {
                                this.ToastersAdapter.Rows = Toasters.ToList();
                                this.ToastersAdapter.ImageViewImages = this.ImageViewImages;
                                this.ToastersAdapter.NotifyDataSetChanged();
                            });
                        }
                        this.ToastersAdapter.LoadMore = true;
                    }
                    else
                    {
                        this.ShowSnack(this.ToastersSearchRecycler, ToastMessage.NullResult, "OK");
                    }
                }
                this.ShowProgressbar(false, "", ToastMessage.Searching);
            }
            catch (Exception ex)
            {
                var a = ex;
                this.ShowProgressbar(false, "", ToastMessage.Searching);
                this.ShowSnack(this.ToastersSearchRecycler, ToastMessage.ServerError, "OK");
            }
        }

        #endregion

    }
}