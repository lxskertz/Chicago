using Foundation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UIKit;
using Tabs.Mobile.Shared.Models.Events;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Helpers;
using BigTed;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class OtherEventInfoController : BaseViewController
    {

        #region Properties

        public BusinessEvents.ActionMode Mode { get; set; }

        public BusinessEvents BusinessEvent { get; set; }

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        private DataSource.Business.OtherEventInfoDataSource OtherEventInfoDataSource { get; set; }

        /// <summary>
        /// Gets or sets first name
        /// </summary>
        public string EventTitle { get; set; }

        /// <summary>
        /// Gets or sets first name
        /// </summary>
        public string EventDescription { get; set; }

        /// <summary>
        /// Gets or sets last name
        /// </summary>
        public DateTime? EventStartDateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Plugin.Media.Abstractions.MediaFile SelectedLogoFile { get; set; }

        /// <summary>
        /// Gets or sets phone number
        /// </summary>
        public DateTime? EventEndDateTime { get; set; }

        /// <summary>
        /// Gets or sets first name
        /// </summary>
        public string Venue { get; set; }

        /// <summary>
        /// Gets or sets first name
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// Gets or sets last name
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets phone number
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// Gets or sets last name
        /// </summary>
        public string Zipcode { get; set; }

        /// <summary>
        /// Gets or sets phone number
        /// </summary>
        public DateTime EventEndTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static bool RequiresRefresh { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? SelectedSection { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static EventType SelectedEventType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static EventCategory SelectedEventCategory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static string InputValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<EventCategory> EventCategories { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<EventType> EventTypes { get; set; }

        #endregion

        #region Constructors

        public OtherEventInfoController (IntPtr handle) : base (handle)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Done, (sender, args) =>
                {
                    CreateEvent();
                }), true);

                // Right swipe
                var recognizer = new UISwipeGestureRecognizer(OnRightSwipeDetected);
                recognizer.Direction = UISwipeGestureRecognizerDirection.Right;
                View.AddGestureRecognizer(recognizer);

                LoadData();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
		/// Raises the left swipe detected event.
		/// </summary>
		private void OnRightSwipeDetected()
        {
            this.NavigationController.PopViewController(true);
        }

        /// <summary>
        /// Load Data
        /// </summary>
        private async void LoadData()
        {
            try
            {
                if (BusinessEvent != null && this.Mode == BusinessEvents.ActionMode.Edit)
                {
                    if (AppDelegate.IsOfflineMode())
                    {
                        BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                        return;
                    }
                    this.Title = AppText.EditEvent;
                    this.Venue = BusinessEvent.Venue;
                    this.StreetAddress = BusinessEvent.StreetAddress;
                    this.City = BusinessEvent.City;
                    this.State = BusinessEvent.State;
                    this.Zipcode = BusinessEvent.ZipCode;
                    await GetEventTypes();
                    await GetEventCategories();
                }

                OtherEventInfoTable.EstimatedRowHeight = 44f;
                OtherEventInfoTable.RowHeight = UITableView.AutomaticDimension;
                OtherEventInfoDataSource = new DataSource.Business.OtherEventInfoDataSource(this);
                OtherEventInfoTable.Source = OtherEventInfoDataSource;
                OtherEventInfoTable.TableFooterView = new UIView();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animated"></param>
        public override void ViewWillAppear(bool animated)
        {
            try
            {
                base.ViewWillAppear(animated);

                if (RequiresRefresh)
                {
                    RequiresRefresh = false;
                    if (SelectedSection != null)
                    {
                        switch (SelectedSection)
                        {
                            case 0:
                                this.Venue = InputValue;
                                break;
                            case 1:
                                this.StreetAddress = InputValue;
                                break;
                            case 2:
                                this.City = InputValue;
                                break;
                            case 3:
                                this.State = InputValue;
                                break;
                            case 4:
                                this.Zipcode = InputValue;
                                break;
                        }

                        InputValue = string.Empty;
                        OtherEventInfoTable.ReloadData();
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
        private async Task GetEventTypes()
        {
            if (AppDelegate.IsOfflineMode())
            {
                BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
            }
            else
            {
                this.EventTypes = await AppDelegate.BusinessEventsFactory.GetEventTypes();

                if (this.EventTypes != null)
                {
                    SelectedEventType = this.EventTypes.Where(x => x.EventTypeId == BusinessEvent.EventTypeId).SingleOrDefault();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task GetEventCategories()
        {
            if (AppDelegate.IsOfflineMode())
            {
                BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
            }
            else
            {
                this.EventCategories = await AppDelegate.BusinessEventsFactory.GetEventCategories();

                if(this.EventCategories != null)
                {
                    SelectedEventCategory = this.EventCategories.Where(x => x.EventCategoryId == BusinessEvent.EventCategoryId).SingleOrDefault();
                }
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
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                    return;
                }
                if (string.IsNullOrEmpty(this.EventTitle))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.RequiredEventTitle, Helpers.ToastTime.ErrorTime);
                    return;
                }
                if (string.IsNullOrEmpty(this.EventDescription))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.RequiredEventDescription, Helpers.ToastTime.ErrorTime);
                    return;
                }
                if (string.IsNullOrEmpty(this.Venue))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.RequiredEventVenue, Helpers.ToastTime.ErrorTime);
                    return;
                }
                if (string.IsNullOrEmpty(this.StreetAddress))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.RequiredEventStreet, Helpers.ToastTime.ErrorTime);
                    return;
                }
                if (string.IsNullOrEmpty(this.City))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.RequiredEventCity, Helpers.ToastTime.ErrorTime);
                    return;
                }
                if (string.IsNullOrEmpty(this.State))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.RequiredEventState, Helpers.ToastTime.ErrorTime);
                    return;
                }
                if (string.IsNullOrEmpty(this.Zipcode))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.RequiredEventZipcode, Helpers.ToastTime.ErrorTime);
                    return;
                }
                if (this.EventStartDateTime == null)
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.RequiredStartDate, Helpers.ToastTime.ErrorTime);
                    return;
                }
                if (this.EventEndDateTime == null)
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.RequiredEndDate, Helpers.ToastTime.ErrorTime);
                    return;
                }
                if (!TimeHelper.ValidStartDate(EventStartDateTime.Value, Mode == BusinessEvents.ActionMode.Edit))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.InvalidStartDate, Helpers.ToastTime.ErrorTime);
                    return;
                }
                if (!TimeHelper.ValidEndDate(EventStartDateTime.Value, EventEndDateTime.Value.Date))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.InvalidEndDate, Helpers.ToastTime.ErrorTime);
                    return;
                }
                if (!TimeHelper.ValidStartTime(EventStartDateTime.Value, Mode == BusinessEvents.ActionMode.Edit))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.InvalidStartTime, Helpers.ToastTime.ErrorTime);
                    return;
                }
                if (!TimeHelper.ValidEndTime(EventStartDateTime.Value, EventEndDateTime.Value))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.InvalidEndTime, Helpers.ToastTime.ErrorTime);
                    return;
                }

                if (SelectedEventType == null)
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.SelectEventType, Helpers.ToastTime.ErrorTime);
                }
                if (SelectedEventCategory == null)
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.SelectEventCategory, Helpers.ToastTime.ErrorTime);
                }
                else
                {
                    BTProgressHUD.Show(ToastMessage.Saving, -1f, ProgressHUD.MaskType.Black);
                    var businessInfo = await AppDelegate.BusinessFactory.GetByUserId(AppDelegate.CurrentUser.UserId);

                    if (businessInfo != null)
                    {
                        var businessEvent = new BusinessEvents();

                        TimeSpan startTs = new TimeSpan(EventStartDateTime.Value.TimeOfDay.Hours,
                            EventStartDateTime.Value.TimeOfDay.Minutes, EventStartDateTime.Value.TimeOfDay.Seconds);
                        DateTime combinedStartDateTime = EventStartDateTime.Value.Date + startTs;

                        TimeSpan endTs = new TimeSpan(EventEndDateTime.Value.TimeOfDay.Hours,
                            EventEndDateTime.Value.TimeOfDay.Minutes, EventEndDateTime.Value.TimeOfDay.Seconds);
                        DateTime combinedEndDateTime = EventEndDateTime.Value.Date + endTs;

                        businessEvent.BusinessId = businessInfo.BusinessId;
                        businessEvent.EventCategoryId = SelectedEventCategory.EventCategoryId;
                        businessEvent.EventTypeId = SelectedEventType.EventTypeId;
                        businessEvent.Title = EventTitle.Trim();
                        businessEvent.EventDescription = EventDescription.Trim();
                        businessEvent.Venue = Venue.Trim();
                        businessEvent.State = State.Trim();
                        businessEvent.Country = "USA";
                        businessEvent.StreetAddress = StreetAddress.Trim();
                        businessEvent.ZipCode = Zipcode.Trim();
                        businessEvent.City = City.Trim();
                        businessEvent.StartDateTimeString = combinedStartDateTime.ToString();
                        businessEvent.EndDateTimeString = combinedEndDateTime.ToString();

                        //businessEvent.StartDatestring = EventStartDateTime.Value.ToShortDateString();
                        //businessEvent.StartTimestring = EventStartDateTime.Value.ToShortTimeString();
                        //businessEvent.EndDatestring = EventEndDateTime.Value.ToShortDateString();
                        //businessEvent.EndTimestring = EventEndDateTime.Value.ToShortTimeString();

                        businessEvent.PrivateEvent = false; //PrivateEvent.On;
                        businessEvent.Free = true; //PaidEvent.On;
                        businessEvent.Paid = false; //PaidEvent.On;

                        businessEvent.StartDateTime = combinedStartDateTime;
                        businessEvent.EndDateTime = combinedEndDateTime;

                        //businessEvent.StartDate = EventStartDateTime.Value;
                        //businessEvent.EndDate = EventEndDateTime.Value;
                        //businessEvent.EndTime = EventEndDateTime.Value;
                        //businessEvent.StartTime = EventStartDateTime.Value;

                        if (Mode == BusinessEvents.ActionMode.Edit)
                        {
                            businessEvent.EventId = BusinessEvent.EventId;
                            await AppDelegate.BusinessEventsFactory.Update(businessEvent);
                            await AddEventLogo(BusinessEvent.EventId);
                            EventInfoController.RequiresRefresh = true;
                            EventInfoController.UpdateBusinessEvent = businessEvent;
                        }
                        else
                        {
                            var eventId = await AppDelegate.BusinessEventsFactory.Add(businessEvent);
                            if (eventId > 0)
                            {
                                await AddEventLogo(eventId);
                            }
                        }

                        BTProgressHUD.Dismiss();

                        EventNameDescController.RequiresRefresh = true;
                        EventNameDescController.CloseController = true;
                        this.NavigationController.PopViewController(true);
                    }
                    else
                    {
                        BTProgressHUD.ShowErrorWithStatus(ToastMessage.ServerError, Helpers.ToastTime.ErrorTime);
                    }

                }
            }

            catch (Exception)
            {
                BTProgressHUD.ShowErrorWithStatus(ToastMessage.ServerError, Helpers.ToastTime.ErrorTime);
                //BTProgressHUD.Dismiss();
            }
        }

        /// <summary>
        /// Add event logo
        /// </summary>
        private async Task AddEventLogo(int eventId)
        {
            if (this.SelectedLogoFile != null)
            {
                await BlobStorageHelper.SaveEventLogoBlob(this.SelectedLogoFile.Path, eventId);
            }
        }

        #endregion

    }
}