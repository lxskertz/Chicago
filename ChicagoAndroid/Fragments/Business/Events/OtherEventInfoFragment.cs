using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using Android.Views.InputMethods;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;
using Tabs.Mobile.Shared.Models.Events;
using Android.Text;

namespace Tabs.Mobile.ChicagoAndroid.Fragments.Business.Events
{
    public class OtherEventInfoFragment : BaseBusinessFragment
    {

        #region Constants, Enums, and Variables

        public LinearLayout parentLayout;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        private Activities.Businesses.CreateEventsActivity CreateEventContext { get; set; }

        /// <summary>
        /// Gets or sets list of the spinner items
        /// </summary>
        private List<Models.SpinnerItems> SpinnerItem { get; set; }

        /// <summary>
        /// Gets or sets the selected spinner item position(index)
        /// </summary>
        private int SelectedItemPosition { get; set; }

        #endregion

        #region Constructors

        public OtherEventInfoFragment(Activities.Businesses.CreateEventsActivity context)
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

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.OtherEventInfo, container, false);

            try
            {
                this.CreateEventContext.selectTypeSpinner = view.FindViewById<Spinner>(Resource.Id.selectEventType);
                this.CreateEventContext.selectCategorySpinner = view.FindViewById<Spinner>(Resource.Id.selectCategory);

                this.CreateEventContext.venueLayout = view.FindViewById<TextInputLayout>(Resource.Id.venue_layout);
                this.CreateEventContext.addressLayout = view.FindViewById<TextInputLayout>(Resource.Id.streetaddress_layout);
                this.CreateEventContext.cityLayout = view.FindViewById<TextInputLayout>(Resource.Id.city_layout);
                this.CreateEventContext.stateLayout = view.FindViewById<TextInputLayout>(Resource.Id.state_layout);
                this.CreateEventContext.zipCodeLayout = view.FindViewById<TextInputLayout>(Resource.Id.zipcode_layout);

                this.CreateEventContext.venue = view.FindViewById<AppCompatEditText>(Resource.Id.venue);
                this.CreateEventContext.address = view.FindViewById<AppCompatEditText>(Resource.Id.streetAddress);
                this.CreateEventContext.city = view.FindViewById<AppCompatEditText>(Resource.Id.city);
                this.CreateEventContext.state = view.FindViewById<AppCompatEditText>(Resource.Id.state);
                this.CreateEventContext.zipCode = view.FindViewById<AppCompatEditText>(Resource.Id.zipcode);
                parentLayout = view.FindViewById<LinearLayout>(Resource.Id.parent_layout);

                this.CreateEventContext.venue.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(50) });
                this.CreateEventContext.address.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(150) });
                this.CreateEventContext.city.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(40) });
                this.CreateEventContext.state.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(40) });
                this.CreateEventContext.zipCode.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(8) });

                LoadData();
            }
            catch (Exception)
            {
            }

            return view;
        }

        private void LoadData()
        {
            try
            {
                if (this.CreateEventContext.BusinessEvent != null && this.CreateEventContext.Mode == BusinessEvents.ActionMode.Edit)
                {
                    this.CreateEventContext.venue.Text = this.CreateEventContext.BusinessEvent.Venue;
                    this.CreateEventContext.address.Text = this.CreateEventContext.BusinessEvent.StreetAddress;
                    this.CreateEventContext.city.Text = this.CreateEventContext.BusinessEvent.City;
                    this.CreateEventContext.state.Text = this.CreateEventContext.BusinessEvent.State;
                    this.CreateEventContext.zipCode.Text = this.CreateEventContext.BusinessEvent.ZipCode;

                    this.CreateEventContext.title.Text = this.CreateEventContext.BusinessEvent.Title;
                    this.CreateEventContext.description.Text = this.CreateEventContext.BusinessEvent.EventDescription;

                    this.CreateEventContext.startDateText.Text = this.CreateEventContext.BusinessEvent.StartDateTime.Value.ToLongDateString();
                    this.CreateEventContext.startTimeText.Text = this.CreateEventContext.BusinessEvent.StartDateTime.Value.ToShortTimeString();
                    this.CreateEventContext.endDateText.Text = this.CreateEventContext.BusinessEvent.EndDateTime.Value.ToLongDateString();
                    this.CreateEventContext.endTimeText.Text = this.CreateEventContext.BusinessEvent.EndDateTime.Value.ToShortTimeString();
                }
            }
            catch (Exception)
            {
            }
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            BuildSpinners();

        }

        public void BuildSpinners()
        {
            try
            {
                SpinnerItem = new List<Models.SpinnerItems>();
                SpinnerItem.Add(new Models.SpinnerItems() { ID = 0, Name = "Select Event Type" });
                foreach (var type in this.CreateEventContext.EventTypes)
                {
                    SpinnerItem.Add(new Models.SpinnerItems() { ID = type.EventTypeId, Name = type.TypeName });
                    if (this.CreateEventContext.Mode == BusinessEvents.ActionMode.Edit &&
                        this.CreateEventContext.BusinessEvent != null)
                    {
                        this.SelectedItemPosition = SpinnerItem.FindIndex(item => item.ID == this.CreateEventContext.BusinessEvent.EventTypeId);
                    }
                }
                this.CreateEventContext.selectTypeSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Spinner_ItemSelected);
                this.CreateEventContext.selectTypeSpinner.Adapter = new Adapters.CustomSpinnerAdapter(this.CreateEventContext, SpinnerItem);
                this.CreateEventContext.selectTypeSpinner.SetSelection(this.SelectedItemPosition);

                SpinnerItem = new List<Models.SpinnerItems>();
                SpinnerItem.Add(new Models.SpinnerItems() { ID = 0, Name = "Select Event Category" });
                foreach (var category in this.CreateEventContext.EventCategories)
                {
                    SpinnerItem.Add(new Models.SpinnerItems() { ID = category.EventCategoryId, Name = category.CategoryName });
                    if (this.CreateEventContext.Mode == BusinessEvents.ActionMode.Edit &&
                        this.CreateEventContext.BusinessEvent != null)
                    {
                        this.SelectedItemPosition = SpinnerItem.FindIndex(item => item.ID == this.CreateEventContext.BusinessEvent.EventCategoryId);
                    }
                }
                this.CreateEventContext.selectCategorySpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Spinner_ItemSelected);
                this.CreateEventContext.selectCategorySpinner.Adapter = new Adapters.CustomSpinnerAdapter(this.CreateEventContext, SpinnerItem);
                this.CreateEventContext.selectCategorySpinner.SetSelection(this.SelectedItemPosition);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Spinner items handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
        }

        #endregion

    }
}