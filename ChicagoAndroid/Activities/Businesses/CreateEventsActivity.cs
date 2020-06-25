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
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Helpers;
using Android.Support.V7.Widget;
using Tabs.Mobile.Shared.Models.Events;
using V4Fragment = Android.Support.V4.App.Fragment;
using V4FragmentManager = Android.Support.V4.App.FragmentManager;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Tabs.Mobile.ChicagoAndroid.Adapters;
using Tabs.Mobile.ChicagoAndroid.Fragments.Business.Events;
using Newtonsoft.Json;

namespace Tabs.Mobile.ChicagoAndroid.Activities.Businesses
{
    [Activity(Label = "Create Event", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class CreateEventsActivity : BaseActivity
    {

        #region Constants, Enums, and Variables

        private CoordinatorLayout coordinatorLayout;
        public Spinner selectTypeSpinner;
        public Spinner selectCategorySpinner;
        public TextView startDateText;
        public TextView startTimeText;
        public TextView endDateText;
        public TextView endTimeText;
        public AppCompatEditText venue;
        public AppCompatEditText address;
        public AppCompatEditText city;
        public AppCompatEditText state;
        public AppCompatEditText zipCode;
        public TextInputLayout venueLayout;
        public TextInputLayout addressLayout;
        public TextInputLayout cityLayout;
        public TextInputLayout stateLayout;
        public TextInputLayout zipCodeLayout;
        //public RadioButton freeRadio;
        //public RadioButton paidRadio;
        //public RadioButton publicRadio;
        //public RadioButton privateRadio;

        public AppCompatEditText title;
        public AppCompatEditText description;
        public TextInputLayout titleLayout;
        public TextInputLayout descriptionLayout;
        public ImageView eventLogo;

        public Plugin.Media.Abstractions.MediaFile SelectedLogoFile;

        #endregion

        #region Properties

        public BusinessEvents.ActionMode Mode { get; set; }

        public BusinessEvents BusinessEvent { get; set; }

        public static Android.Graphics.Bitmap ImageBitmap { get; set; }

        /// <summary>
        /// Gets or Sets the adaptor
        /// </summary>
        private MyPagerAdapter Adaptor { get; set; }

        /// <summary>
        /// Gets or sets the TutorialPager
        /// </summary>
        private ViewPager ViewPager { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<EventCategory> EventCategories { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<EventType> EventTypes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public BusinessEvents BusinessEvents { get; set; }

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
                case Resource.Id.done:
                    CreateEvent();
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
            this.MenuInflater.Inflate(Resource.Menu.done_icon_menu, menu);

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
                this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                this.SupportActionBar.SetDisplayShowHomeEnabled(true);
                this.ShowProgressbar(true, "", ToastMessage.Loading);

                SetContentView(Resource.Layout.CreateEventHome);
                coordinatorLayout = FindViewById<CoordinatorLayout>(Resource.Id.main_content);
                this.ViewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
                var toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);
                toolbar.InflateMenu(Resource.Menu.create_events_menu);
                toolbar.Elevation = 10.0f;

                var tabLayout = FindViewById<TabLayout>(Resource.Id.tabs);
                tabLayout.SetupWithViewPager(this.ViewPager);
                tabLayout.Visibility = ViewStates.Gone;

                this.Mode = (BusinessEvents.ActionMode)Intent.GetIntExtra("ActionMode", (int)BusinessEvents.ActionMode.Add);

                if (Mode == BusinessEvents.ActionMode.Edit)
                {
                    this.Title = AppText.EditEvent;
                    this.BusinessEvent = JsonConvert.DeserializeObject<BusinessEvents>(Intent.GetStringExtra("BusinessEventInfo"));
                }


                await GetEventTypes();
                await GetEventCategories();

                this.Adaptor = new MyPagerAdapter(this.SupportFragmentManager);
                this.Adaptor.AddFragment(new EventNameDescFragment(this), "name");
                this.Adaptor.AddFragment(new OtherEventInfoFragment(this), "other");
                this.ViewPager.Adapter = this.Adaptor;
                this.ViewPager.Adapter.NotifyDataSetChanged();

                this.ShowProgressbar(false, "", ToastMessage.PleaseWait);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task GetEventTypes()
        {
            if (this.CheckNetworkConnectivity() == null)
            {
                Toast.MakeText(this, ToastMessage.NoInternet, ToastLength.Short).Show();
            } else
            {
                this.EventTypes = await App.BusinessEventsFactory.GetEventTypes();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task GetEventCategories()
        {
            if (this.CheckNetworkConnectivity() == null)
            {
                Toast.MakeText(this, ToastMessage.NoInternet, ToastLength.Short).Show();
            }
            else
            {
                this.EventCategories = await App.BusinessEventsFactory.GetEventCategories();
            }
        }

        /// <summary>
        /// Add event logo
        /// </summary>
        private async Task AddEventLogo(int eventId)
        {
            if (this.SelectedLogoFile != null)
            {
                //this.ShowProgressbar(true, "", ToastMessage.Updating);
                await BlobStorageHelper.SaveEventLogoBlob(this.SelectedLogoFile.Path, eventId);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async void CreateEvent()
        {
            try
            {
                if (this.CheckNetworkConnectivity() == null)
                {
                    this.ShowSnack(coordinatorLayout, ToastMessage.NoInternet, "OK");
                    return;
                }
                if (!this.ValidateInput(titleLayout, title, ToastMessage.RequiredField))
                {
                    ShowSnack(coordinatorLayout, ToastMessage.RequiredEventTitle, "OK");
                    return;
                }
                if (!this.ValidateInput(descriptionLayout, description, ToastMessage.RequiredField))
                {
                    ShowSnack(coordinatorLayout, ToastMessage.RequiredEventDescription, "OK");
                    return;
                }
                if (!this.ValidateInput(venueLayout, venue, ToastMessage.RequiredField))
                {
                    ShowSnack(coordinatorLayout, ToastMessage.RequiredEventVenue, "OK");
                    return;
                }
                if (!this.ValidateInput(addressLayout, address, ToastMessage.RequiredField))
                {
                    ShowSnack(coordinatorLayout, ToastMessage.RequiredEventStreet, "OK");
                    return;
                }
                if (!this.ValidateInput(cityLayout, city, ToastMessage.RequiredField))
                {
                    ShowSnack(coordinatorLayout, ToastMessage.RequiredEventCity, "OK");
                    return;
                }
                if (!this.ValidateInput(stateLayout, state, ToastMessage.RequiredField))
                {
                    ShowSnack(coordinatorLayout, ToastMessage.RequiredEventState, "OK");
                    return;
                }
                if (!this.ValidateInput(zipCodeLayout, zipCode, ToastMessage.RequiredField))
                {
                    ShowSnack(coordinatorLayout, ToastMessage.RequiredEventZipcode, "OK");
                    return;
                }
                if (string.IsNullOrEmpty(startDateText.Text) || startDateText.Text.ToLower() == "select start date")
                {
                    ShowSnack(coordinatorLayout, ToastMessage.RequiredStartDate, "OK");
                    return;
                }
                if (string.IsNullOrEmpty(endDateText.Text) || endDateText.Text.ToLower() == "select end date")
                {
                    ShowSnack(coordinatorLayout, ToastMessage.RequiredEndDate, "OK");
                    return;
                }
                if (string.IsNullOrEmpty(startTimeText.Text) || startTimeText.Text.ToLower() == "select start time")
                {
                    ShowSnack(coordinatorLayout, ToastMessage.RequiredStartTime, "OK");
                    return;
                }
                if (string.IsNullOrEmpty(endTimeText.Text) || endTimeText.Text.ToLower() == "select end time")
                {
                    ShowSnack(coordinatorLayout, ToastMessage.RequiredEndTime, "OK");
                    return;
                }
                if (!TimeHelper.ValidStartDate(Convert.ToDateTime(startDateText.Text), Mode == BusinessEvents.ActionMode.Edit))
                {
                    ShowSnack(coordinatorLayout, ToastMessage.InvalidStartDate, "OK");
                    return;
                }
                if (!TimeHelper.ValidEndDate(Convert.ToDateTime(startDateText.Text), Convert.ToDateTime(endDateText.Text)))
                {
                    ShowSnack(coordinatorLayout, ToastMessage.InvalidEndDate, "OK");
                    return;
                }
                if (!TimeHelper.ValidStartTime(Convert.ToDateTime(startTimeText.Text), Mode == BusinessEvents.ActionMode.Edit))
                {
                    ShowSnack(coordinatorLayout, ToastMessage.InvalidStartTime, "OK");
                    return;
                }
                if (!TimeHelper.ValidEndTime(Convert.ToDateTime(startTimeText.Text), Convert.ToDateTime(endTimeText.Text)))
                {
                    ShowSnack(coordinatorLayout, ToastMessage.InvalidEndTime, "OK");
                    return;
                }
                if ((int)selectTypeSpinner.SelectedView.Tag == 0)
                {
                    ShowSnack(coordinatorLayout, ToastMessage.SelectEventType, "OK");
                }
                if ((int)selectCategorySpinner.SelectedView.Tag == 0)
                {
                    ShowSnack(coordinatorLayout, ToastMessage.SelectEventCategory, "OK");
                }
                else
                {
                    this.ShowProgressbar(true, "", ToastMessage.Saving);
                    try
                    {
                        var businessInfo = await App.BusinessFactory.GetByUserId(this.CurrentUser.UserId);

                        if (businessInfo != null)
                        {
                            var businessEvent = new BusinessEvents();

                            DateTime startDate = Convert.ToDateTime(startDateText.Text.Trim());
                            DateTime startTime = Convert.ToDateTime(startTimeText.Text.Trim());
                            TimeSpan startTs = new TimeSpan(startTime.TimeOfDay.Hours, startDate.TimeOfDay.Minutes, startDate.TimeOfDay.Seconds);
                            DateTime combinedStartDateTime = startDate.Date + startTs;

                            DateTime endDate = Convert.ToDateTime(endDateText.Text.Trim());
                            DateTime endTime = Convert.ToDateTime(endTimeText.Text.Trim());
                            TimeSpan endTs = new TimeSpan(endTime.TimeOfDay.Hours, endTime.TimeOfDay.Minutes, endTime.TimeOfDay.Seconds);
                            DateTime combinedEndDateTime = endDate.Date + endTs;

                            businessEvent.BusinessId = businessInfo.BusinessId;
                            businessEvent.EventCategoryId = (int)selectCategorySpinner.SelectedView.Tag;
                            businessEvent.EventTypeId = (int)selectTypeSpinner.SelectedView.Tag;
                            businessEvent.Title = title.Text.Trim();
                            businessEvent.EventDescription = description.Text.Trim();
                            businessEvent.Venue = venue.Text.Trim();
                            businessEvent.State = state.Text.Trim();
                            businessEvent.Country = "USA";
                            businessEvent.StreetAddress = address.Text.Trim();
                            businessEvent.ZipCode = zipCode.Text.Trim();
                            businessEvent.City = city.Text.Trim();
                            businessEvent.StartDateTimeString = combinedStartDateTime.ToString();
                            businessEvent.EndDateTimeString = combinedEndDateTime.ToString();

                            //businessEvent.StartDatestring = startDateText.Text.Trim();
                            //businessEvent.StartTimestring = startTimeText.Text.Trim();
                            //businessEvent.EndDatestring = endDateText.Text.Trim();
                            //businessEvent.EndTimestring = endTimeText.Text.Trim();

                            businessEvent.PrivateEvent = false; //privateRadio.Checked;
                            businessEvent.Free = true; //freeRadio.Checked;
                            businessEvent.Paid = false; //paidRadio.Checked;

                            businessEvent.StartDateTime = combinedStartDateTime;
                            businessEvent.EndDateTime = combinedEndDateTime;

                            //businessEvent.StartDate = Convert.ToDateTime(startDateText.Text.Trim());
                            //businessEvent.EndDate = Convert.ToDateTime(endDateText.Text.Trim());
                            //businessEvent.EndTime = Convert.ToDateTime(endTimeText.Text.Trim());
                            //businessEvent.StartTime = Convert.ToDateTime(startTimeText.Text.Trim());

                            if (Mode == BusinessEvents.ActionMode.Edit)
                            {
                                businessEvent.EventId = BusinessEvent.EventId;
                                await App.BusinessEventsFactory.Update(businessEvent);
                                await AddEventLogo(businessEvent.EventId);
                                Individuals.Events.EventInfoActivity.RequiresRefresh = true;
                                Individuals.Events.EventInfoActivity.UpdateBusinessEvent = businessEvent;
                            }
                            else
                            {
                                var eventId = await App.BusinessEventsFactory.Add(businessEvent);

                                if (eventId > 0)
                                {
                                    await AddEventLogo(eventId);
                                }
                            }

                            this.ShowProgressbar(false, "", ToastMessage.PleaseWait);
                            this.Finish();
                        }
                        else
                        {
                            ShowSnack(coordinatorLayout, ToastMessage.ServerError, "OK");
                        }

                    }

                    catch (Exception)
                    {
                        this.ShowProgressbar(false, "", ToastMessage.PleaseWait);
                        ShowSnack(coordinatorLayout, ToastMessage.ServerError, "OK");
                    }
                }
            }
            catch (Exception)
            {
                this.ShowProgressbar(false, "", ToastMessage.Loading);
            }
        }


        #endregion

    }
}