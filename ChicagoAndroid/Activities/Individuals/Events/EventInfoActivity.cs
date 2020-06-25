using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Android.Views.InputMethods;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using System.Threading.Tasks;
using Android.Support.V4.Widget;
using V4Fragment = Android.Support.V4.App.Fragment;
using V4FragmentManager = Android.Support.V4.App.FragmentManager;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Newtonsoft.Json;
using Tabs.Mobile.ChicagoAndroid.Adapters.Individuals.Events;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.Events;
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Models.Businesses;

namespace Tabs.Mobile.ChicagoAndroid.Activities.Individuals.Events
{
    [Activity(Label = "Event Information", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class EventInfoActivity : BaseActivity
    {

        #region Constants, Enums, Variables

        private ListView eventInfoList;
        private View headerView;
        private bool showToolbar;
        private bool isBusiness;
        TextView yes;
        TextView no;
        TextView areYouGoing;
        ImageView eventLogo;
        FloatingActionButton fabBtn;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        private EventInfoAdapter EventInfoAdapter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BusinessEvents BusinessEvents { get; set; } = new BusinessEvents();

        /// <summary>
        /// 
        /// </summary>
        private Android.Support.V7.Widget.PopupMenu ActionsMenu { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private Rsvp ExisitingRsvp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Business BusinessInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static Android.Graphics.Bitmap ImageBitmap { get; set; }

        public static BusinessEvents UpdateBusinessEvent { get; set; }

        public static bool RequiresRefresh { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toolbar"></param>
        public void IndividualHomeButtomToolbar(V7Toolbar toolbar)
        {
            try
            {
                yes = toolbar.FindViewById<TextView>(Resource.Id.yes);
                no = toolbar.FindViewById<TextView>(Resource.Id.no);
                areYouGoing = toolbar.FindViewById<TextView>(Resource.Id.areYouGoing);

                no.Click += async delegate
                {
                    await AddRSVP(false);
                    UpdateItemText(false);
                };

                yes.Click += async delegate
                {
                    if (yes.Text.ToLower() == "yes")
                    {
                        await AddRSVP(true);
                        UpdateItemText(true);
                    }
                    else
                    {
                        Change();
                    }
                };
            }
            catch (Exception)
            {
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                this.SupportActionBar.SetDisplayShowHomeEnabled(true);
                SetContentView(Resource.Layout.EventInfo);
                eventInfoList = FindViewById<ListView>(Resource.Id.eventInfoList);
                var toolbar = FindViewById<V7Toolbar>(Resource.Id.rsvpToolbar);
                fabBtn = FindViewById<FloatingActionButton>(Resource.Id.editEvent);

                showToolbar = Intent.GetBooleanExtra("ShowToolbar", false);
                isBusiness = Intent.GetBooleanExtra("IsBusiness", false);

                fabBtn.Visibility = isBusiness ? ViewStates.Visible : ViewStates.Gone;

                fabBtn.Click += delegate
                {
                    Intent intent = new Intent(this, typeof(Businesses.CreateEventsActivity));
                    intent.PutExtra("BusinessEventInfo", JsonConvert.SerializeObject(this.BusinessEvents));
                    intent.PutExtra("ActionMode", (int)BusinessEvents.ActionMode.Edit);
                    Businesses.CreateEventsActivity.ImageBitmap = ImageBitmap;
                    this.StartActivity(intent);
                };

                toolbar.Visibility = showToolbar ? ViewStates.Visible : ViewStates.Gone;
                toolbar.InflateMenu(Resource.Menu.event_info_toolbar);
                toolbar.Elevation = 20.0f;
                IndividualHomeButtomToolbar(toolbar);

                this.BusinessEvents = JsonConvert.DeserializeObject<BusinessEvents>(Intent.GetStringExtra("BusinessEventInfo"));

                LoadData();
            }
            catch (Exception)
            {
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menuAction:
                    Intent intent = new Intent(this, typeof(Businesses.BusinessRsvpsActivity));
                    intent.PutExtra("BusinessEventInfo", JsonConvert.SerializeObject(this.BusinessEvents));
                    this.StartActivity(intent);
                    //DisplayActionsMenu();
                    return true;
                case Android.Resource.Id.Home:
                    this.Finish();
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            isBusiness = Intent.GetBooleanExtra("IsBusiness", false);
            if (isBusiness)
            {
                this.MenuInflater.Inflate(Resource.Menu.menu_with_text, menu);

                menu.FindItem(Resource.Id.menuAction).SetTitle(AppText.ViewRsvps);
            }

            return base.OnCreateOptionsMenu(menu);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task GetEventLogo()
        {
            try
            {
                ImageViewImage logo = new ImageViewImage();
                logo.Id = this.BusinessEvents.EventId;
                Uri imageUri = new Uri(await BlobStorageHelper.GetEventLogoUri(this.BusinessEvents.EventId));
                logo.ImageUrl = imageUri;

                if (logo.ImageBitmap == null)
                {
                    this.BeginDownloadingImage(logo, eventLogo);
                }

                eventLogo.SetImageBitmap(logo.ImageBitmap);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Display category menu
        /// </summary>
        private void DisplayActionsMenu()
        {
            if (this.ActionsMenu != null)
            {
                this.ActionsMenu.Dismiss();
                this.ActionsMenu.Dispose();
            }
            this.ActionsMenu = new Android.Support.V7.Widget.PopupMenu(this, fabBtn, (int)GravityFlags.Start);
            try
            {
                this.ActionsMenu.Inflate(Resource.Menu.event_info_menu);
                IMenu menu = this.ActionsMenu.Menu;

                this.ActionsMenu.MenuItemClick += (sender, e) => {
                    switch (e.Item.ItemId)
                    {
                        case Resource.Id.viewRsvp:
                            Intent intent = new Intent(this, typeof(Businesses.BusinessRsvpsActivity));
                            intent.PutExtra("BusinessEventInfo", JsonConvert.SerializeObject(this.BusinessEvents));
                            this.StartActivity(intent);
                            break;
                        case Resource.Id.liveCheckIns:
                            Intent intent1 = new Intent(this, typeof(Businesses.BusinessCheckInsActivity));
                            intent1.PutExtra("EventId", JsonConvert.SerializeObject(this.BusinessEvents.EventId));
                            intent1.PutExtra("ScreenCheckInType", (int)Shared.Models.CheckIns.CheckIn.CheckInTypes.Event);
                            this.StartActivity(intent1);
                            break;
                    }
                };
                this.ActionsMenu.Show();
            }
            catch (Exception) { }
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (RequiresRefresh)
            {
                RequiresRefresh = false;
                if (UpdateBusinessEvent != null)
                {
                    this.BusinessEvents = UpdateBusinessEvent;
                }
                LoadData();
            }
        }

        /// <summary>
        /// Load data
        /// </summary>
        private async void LoadData()
        {
            try
            {

                if (this.BusinessEvents != null)
                {

                    if (headerView != null)
                    {
                        eventInfoList.RemoveHeaderView(headerView);
                    }
                    headerView = LayoutInflater.FromContext(this).Inflate(Resource.Layout.EventInfoHeader, null);

                    eventLogo = headerView.FindViewById<ImageView>(Resource.Id.eventLogo);
                    var eventTitle = headerView.FindViewById<TextView>(Resource.Id.eventTitle);
                    var eventOwner = headerView.FindViewById<TextView>(Resource.Id.eventOwner);

                    eventTitle.Text = string.IsNullOrEmpty(this.BusinessEvents.Title) ? "" : this.BusinessEvents.Title;
                    eventLogo.SetImageBitmap(ImageBitmap);
                
                    if (this.CheckNetworkConnectivity() != null)
                    {
                        if (ImageBitmap != null)
                        {
                            eventLogo.SetImageBitmap(ImageBitmap);
                            //ImageBitmap.Recycle();
                            //ImageBitmap = null;
                        }
                        else
                        {
                            await GetEventLogo();
                        }
                        this.BusinessInfo = await App.BusinessFactory.Get(this.BusinessEvents.BusinessId);

                        if (this.BusinessInfo != null)
                        {
                            eventOwner.Text = string.IsNullOrEmpty(this.BusinessInfo.BusinessName) ? "" : "By " + this.BusinessInfo.BusinessName;
                        }

                        if (showToolbar)
                        {
                            var eventId = this.BusinessEvents == null ? 0 : this.BusinessEvents.EventId;
                            this.ExisitingRsvp = await App.RsvpFactory.GetToasterRsvp(this.CurrentUser.UserId, eventId);

                            if (this.ExisitingRsvp != null)
                            {
                                UpdateItemText(this.ExisitingRsvp.Going);
                            }
                        }
                    }

                    eventInfoList.AddHeaderView(headerView);
                    this.EventInfoAdapter = new EventInfoAdapter(this, this.BusinessEvents);
                    eventInfoList.Adapter = this.EventInfoAdapter;
                    eventInfoList.ItemClick += this.EventInfoAdapter.OnListItemClick;
                    eventInfoList.DividerHeight = 2;


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
        /// <param name="yes"></param>
        private void UpdateItemText(bool showYes)
        {
            if (showYes)
            {
                areYouGoing.Text = AppText.YouAreGoing;
                yes.Text = AppText.Change;
                no.Text = "";
                no.Visibility = ViewStates.Gone;
            }
            else
            {
                areYouGoing.Text = AppText.YouAreNotGoing;
                yes.Text = AppText.Change;
                no.Text = "";
                no.Visibility = ViewStates.Gone;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Change()
        {
            yes.Text = AppText.Yes;
            no.Text = AppText.No;
            no.Visibility = ViewStates.Visible;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="going"></param>
        /// <returns></returns>
        private async Task AddRSVP(bool going)
        {
            try
            {
                if (CheckNetworkConnectivity() == null)
                {
                    Toast.MakeText(this, ToastMessage.NoInternet, ToastLength.Short).Show();
                    return;
                }
                else
                {
                    if (this.BusinessEvents != null)
                    {
                        this.ShowProgressbar(true, "", ToastMessage.Saving);
                        Rsvp rsvp = new Rsvp();
                        rsvp.Going = going;
                        rsvp.EventId = this.BusinessEvents.EventId;
                        rsvp.BusinessId = this.BusinessInfo != null ? this.BusinessInfo.BusinessId : 0;
                        rsvp.UserId = this.CurrentUser.UserId;

                        this.ExisitingRsvp = await App.RsvpFactory.GetToasterRsvp(this.CurrentUser.UserId, rsvp.EventId);

                        if (this.ExisitingRsvp == null)
                        {
                            await App.RsvpFactory.Add(rsvp);
                        }
                        else
                        {
                            await App.RsvpFactory.Change(going, rsvp.UserId, rsvp.EventId);
                        }
                        this.ShowProgressbar(false, "", ToastMessage.Saving);
                    }
                }
            }
            catch (Exception)
            {
                this.ShowProgressbar(false, "", ToastMessage.Saving);
            }
        }

        #endregion

    }
}