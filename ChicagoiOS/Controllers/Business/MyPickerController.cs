using Foundation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UIKit;
using Tabs.Mobile.ChicagoiOS.Models;
using Tabs.Mobile.Shared.Models.Events;
using Tabs.Mobile.Shared.Resources;
using BigTed;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class MyPickerController : UIViewController //BaseViewController
    {

        #region Constants, Enums, and Variables

        public enum ComponentType
        {
            EventTypes = 1,
            EventCategories = 2,
            StartDateTime = 3,
            EndDateTime = 4,
            Date = 5
        }

        public enum PickerViewType
        {
            Pickerview = 1,
            DatePicker = 2
        }

        public enum Caller
        {
            EventNameController = 1,
            OtherEventInfo = 2,
            OrdersController = 3
        }

        private Dictionary<int, string> ComponentItem = new Dictionary<int, string>();

        private PickerViewModel pickerViewModel;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Caller ControllerCaller { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ComponentType PickerComponentType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PickerViewType ControllerPickerViewType { get; set; } = PickerViewType.Pickerview;

        /// <summary>
        /// 
        /// </summary>
        public ICollection<EventCategory> EventCategories { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<EventType> EventTypes { get; set; }

        #endregion

        #region Contructors

        public MyPickerController (IntPtr handle) : base (handle)
        {
        }

        #endregion

        #region Methods

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
            }
        }

        /// <summary>
        /// Open Picker
        /// </summary>
        private async void OpenPicker()
        {
            try
            {
                switch (PickerComponentType)
                {
                    case ComponentType.EventCategories:
                        if (AppDelegate.IsOfflineMode())
                        {
                            BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                        }
                        else
                        {
                            await GetEventCategories();
                            if (this.EventCategories != null)
                            {
                                foreach (var category in this.EventCategories)
                                {
                                    ComponentItem.Add(category.EventCategoryId, category.CategoryName);
                                }
                            }
                        }
                        break;
                    case ComponentType.EventTypes:
                        if (AppDelegate.IsOfflineMode())
                        {
                            BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                        }
                        else
                        {
                            await GetEventTypes();
                            if (EventTypes != null)
                            {
                                foreach (var type in this.EventTypes)
                                {
                                    ComponentItem.Add(type.EventTypeId, type.TypeName);
                                }
                            }
                        }
                        break;
                }
                _PickerView.ShowSelectionIndicator = true;
                pickerViewModel = new PickerViewModel(ComponentItem);
                pickerViewModel.SelectedItemRow = 0;
                _PickerView.Model = pickerViewModel;
            }
            catch (Exception) { }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OpenDateTimePicker()
        {
            if (ControllerCaller == Caller.OrdersController)
            {
                _DatePicker.Mode = UIDatePickerMode.Date;
                _DatePicker.Date = (NSDate)DateTime.Today;
            }
            else
            {
                _DatePicker.Mode = UIDatePickerMode.DateAndTime;
                _DatePicker.Date = (NSDate)DateTime.Today;
                _DatePicker.MinimumDate = NSDate.Now;
                //_DatePicker.MaximumDate = (NSDate)DateTime.Today.AddMonths(2);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                if (this.ControllerPickerViewType == PickerViewType.Pickerview)
                {
                    _DatePicker.Hidden = true;
                    OpenPicker();
                }
                else
                {
                    _PickerView.Hidden = true;
                    OpenDateTimePicker();
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animated"></param>
        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (this.ControllerPickerViewType == PickerViewType.Pickerview)
            {
                SetPickerViewValue();
            }
            else
            {
                SetDateTimeValue();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetDateTimeValue()
        {
            var dateFormatter = new NSDateFormatter()
            {
                //DateFormat = "MM dd, yyyy HH:mm:ss"
                DateFormat = "MM/dd/yyyy H:mm:ss a"
            };

            switch (this.ControllerCaller)
            {
                case Caller.EventNameController:
                    EventNameDescController.RequiresRefresh = true;
                    switch (this.PickerComponentType)
                    {
                        case ComponentType.StartDateTime:
                            var a = dateFormatter.ToString(_DatePicker.Date);
                            EventNameDescController.EventStartDateTime = Convert.ToDateTime(a);

                            break;
                        case ComponentType.EndDateTime:
                            var b = dateFormatter.ToString(_DatePicker.Date);
                            EventNameDescController.EventEndDateTime = Convert.ToDateTime(b);
                            break;
                    }
                    break;
                case Caller.OtherEventInfo:
                    //OtherEventInfoController.RequiresRefresh = true;                 
                    break;
                case Caller.OrdersController:
                    var c = dateFormatter.ToString(_DatePicker.Date);
                    OrdersController.FilterDate = Convert.ToDateTime(c);
                    OrdersController.ApplyDateFilter = true;
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetPickerViewValue()
        {
            if (ComponentItem != null && ComponentItem.Count > 0)
            {
                switch (this.ControllerCaller)
                {
                    case Caller.EventNameController:
                        //EventNameDescController.RequiresRefresh = true;
                        //EventNameDescController.InputValue = _Textview.Text;
                        break;
                    case Caller.OtherEventInfo:
                        OtherEventInfoController.RequiresRefresh = true;
                        switch (this.PickerComponentType)
                        {
                            case ComponentType.EventTypes:
                                var typeId = ComponentItem.ElementAt((int)pickerViewModel.SelectedItemRow).Key;
                                var typeName = ComponentItem.ElementAt((int)pickerViewModel.SelectedItemRow).Value;
                                OtherEventInfoController.SelectedEventType = new EventType() { EventTypeId = typeId, TypeName = typeName };
                                break;
                            case ComponentType.EventCategories:
                                var id = ComponentItem.ElementAt((int)pickerViewModel.SelectedItemRow).Key;
                                var name = ComponentItem.ElementAt((int)pickerViewModel.SelectedItemRow).Value;
                                OtherEventInfoController.SelectedEventCategory = new EventCategory() { EventCategoryId = id, CategoryName = name };
                                break;
                        }
                        break;
                }
            }
        }

        #endregion

    }
}