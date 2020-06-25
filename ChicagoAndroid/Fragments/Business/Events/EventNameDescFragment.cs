using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Views.InputMethods;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.Shared.Resources;
using Plugin.Media;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Models.Events;
using Android.Text;

namespace Tabs.Mobile.ChicagoAndroid.Fragments.Business.Events
{
    public class EventNameDescFragment : BaseBusinessFragment
    {

        #region Constants, Enums, and Variables
      
        //private FrameLayout parentLayout;


        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        private Activities.Businesses.CreateEventsActivity CreateEventContext { get; set; }

        #endregion

        #region Constructors

        public EventNameDescFragment(Activities.Businesses.CreateEventsActivity context)
        {
            this.CreateEventContext = context;
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
        /// Hide scan menu icon
        /// </summary>
        /// <param name="menu"></param>
        public override void OnPrepareOptionsMenu(IMenu menu)
        {
            IMenuItem item = menu.FindItem(Resource.Id.done);
            item.SetVisible(false);
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
            var view = inflater.Inflate(Resource.Layout.EventNameDescription, container, false);
            try
            {
                this.CreateEventContext.startDateText = view.FindViewById<TextView>(Resource.Id.startDate);
                this.CreateEventContext.startTimeText = view.FindViewById<TextView>(Resource.Id.startTime);
                this.CreateEventContext.eventLogo = view.FindViewById<ImageView>(Resource.Id.eventLogo);
                this.CreateEventContext.endDateText = view.FindViewById<TextView>(Resource.Id.endDate);
                this.CreateEventContext.endTimeText = view.FindViewById<TextView>(Resource.Id.endTime);

                this.CreateEventContext.titleLayout = view.FindViewById<TextInputLayout>(Resource.Id.eventTitle_layout);
                this.CreateEventContext.descriptionLayout = view.FindViewById<TextInputLayout>(Resource.Id.eventDescription_layout);

                this.CreateEventContext.title = view.FindViewById<AppCompatEditText>(Resource.Id.eventTitle);
                this.CreateEventContext.description = view.FindViewById<AppCompatEditText>(Resource.Id.eventDescription);

                this.CreateEventContext.title.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(80) });
                this.CreateEventContext.description.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(250) });

                this.CreateEventContext.startDateText.Click += delegate
                {
                    new DatePickerDialog(this.CreateEventContext, StartDateSet, DateTime.Today.Year, DateTime.Today.Month - 1, DateTime.Today.Day).Show();
                };

                this.CreateEventContext.startTimeText.Click += delegate
                {
                    var endTimeDialog = new TimePickerDialog(this.CreateEventContext, StartTimeSet, DateTime.Now.Hour, DateTime.Now.Minute, false);
                    endTimeDialog.Show();
                };

                this.CreateEventContext.endDateText.Click += delegate
                {
                    new DatePickerDialog(this.CreateEventContext, EndDateSet, DateTime.Today.Year, DateTime.Today.Month - 1, DateTime.Today.Day).Show();
                };

                this.CreateEventContext.endTimeText.Click += delegate
                {
                    var endTimeDialog = new TimePickerDialog(this.CreateEventContext, EndTimeSet, DateTime.Now.Hour, DateTime.Now.Minute, false);
                    endTimeDialog.Show();
                };

                this.CreateEventContext.eventLogo.Click += delegate
                {
                    PickLogo();
                };

                LoadData();
            }
            catch (Exception)
            {
            }

            return view;
        }

        private async void LoadData()
        {
            try
            {
                if (this.CreateEventContext.BusinessEvent != null && this.CreateEventContext.Mode == BusinessEvents.ActionMode.Edit)
                {
                    this.CreateEventContext.title.Text = this.CreateEventContext.BusinessEvent.Title;
                    this.CreateEventContext.description.Text = this.CreateEventContext.BusinessEvent.EventDescription;

                    this.CreateEventContext.startDateText.Text = this.CreateEventContext.BusinessEvent.StartDateTime.Value.ToLongDateString();
                    this.CreateEventContext.startTimeText.Text = this.CreateEventContext.BusinessEvent.StartDateTime.Value.ToShortTimeString();
                    this.CreateEventContext.endDateText.Text = this.CreateEventContext.BusinessEvent.EndDateTime.Value.ToLongDateString();
                    this.CreateEventContext.endTimeText.Text = this.CreateEventContext.BusinessEvent.EndDateTime.Value.ToShortTimeString();
                    if (Activities.Businesses.CreateEventsActivity.ImageBitmap != null)
                    {
                        this.CreateEventContext.eventLogo.SetImageBitmap(Activities.Businesses.CreateEventsActivity.ImageBitmap);
                    }
                    else
                    {

                        if (this.CreateEventContext.CheckNetworkConnectivity() == null)
                        {
                            Toast.MakeText(this.CreateEventContext, ToastMessage.NoInternet, ToastLength.Short).Show();
                            return;
                        }

                        await GetEventLogo();
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
        private async Task GetEventLogo()
        {
            ImageViewImage logo = new ImageViewImage();
            logo.Id = this.CreateEventContext.BusinessEvent.EventId;
            Uri imageUri = new Uri(await BlobStorageHelper.GetEventLogoUri(this.CreateEventContext.BusinessEvent.EventId));
            logo.ImageUrl = imageUri;

            if (logo.ImageBitmap == null)
            {
                this.CreateEventContext.BeginDownloadingImage(logo, this.CreateEventContext.eventLogo);
            }

            this.CreateEventContext.eventLogo.SetImageBitmap(logo.ImageBitmap);
        }

        /// <summary>
        /// 
        /// </summary>
        private async void PickLogo()
        {
            try
            {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    return;
                }
                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });


                if (file == null)
                {
                    return;
                }
                this.CreateEventContext.SelectedLogoFile = file;
                var ImageUri = Android.Net.Uri.Parse(file.Path);
                this.CreateEventContext.eventLogo.SetImageURI(ImageUri);
            }
            catch (Exception)
            {
            }
        }    

        /// <summary>
        /// Appointment date callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            this.CreateEventContext.startDateText.Text = e.Date.ToLongDateString();
        }

        /// <summary>
        /// Appointment date callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartTimeSet(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            var hour = TimeHelper.GetPMTime(e.HourOfDay);
            var minutes = e.Minute < 10 ? "0" + e.Minute.ToString() : e.Minute.ToString();
            var amPM = e.HourOfDay < 12 ? "AM" : "PM";
            this.CreateEventContext.startTimeText.Text = hour + ":" + minutes + " " + amPM;
        }

        /// <summary>
        /// Appointment date callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EndDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            this.CreateEventContext.endDateText.Text = e.Date.ToLongDateString();
        }

        /// <summary>
        /// Appointment date callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EndTimeSet(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            var hour = TimeHelper.GetPMTime(e.HourOfDay);
            var minutes = e.Minute < 10 ? "0" + e.Minute.ToString() : e.Minute.ToString();
            var amPM = e.HourOfDay < 12 ? "AM" : "PM";
            this.CreateEventContext.endTimeText.Text = hour + ":" + minutes + " " + amPM;
        }

        #endregion

    }
}