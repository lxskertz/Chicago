using Foundation;
using System;
using System.Threading.Tasks;
using UIKit;
using Plugin.Media;
using BigTed;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.Events;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class EventNameDescController : BaseViewController
    {

        #region Properties

        public BusinessEvents.ActionMode Mode { get; set; }

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        private DataSource.Business.EventNameDataSource EventNameDataSource { get; set; }

        /// <summary>
        /// Gets or sets first name
        /// </summary>
        public string EventTitle { get; set; }

        /// <summary>
        /// Gets or sets first name
        /// </summary>
        public string EventDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Plugin.Media.Abstractions.MediaFile SelectedLogoFile;

        /// <summary>
        /// Gets or sets last name
        /// </summary>
        public static DateTime? EventStartDateTime { get; set; }

        /// <summary>
        /// Gets or sets phone number
        /// </summary>
        public static DateTime? EventEndDateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static bool RequiresRefresh { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static bool CloseController { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? SelectedSection { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static string InputValue { get; set; }

        public BusinessEvents BusinessEvent { get; set; }
        
        #endregion

        #region Constructors

        public EventNameDescController(IntPtr handle) : base(handle) { }

        #endregion

        #region Methods

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
                    this.EventTitle = BusinessEvent.Title;
                    this.EventDescription = BusinessEvent.EventDescription;

                    EventStartDateTime = this.BusinessEvent.StartDateTime.Value;
                    EventEndDateTime = this.BusinessEvent.EndDateTime.Value;
                    await GetEventPic();
                }

                EventNameDecTable.EstimatedRowHeight = 44f;
                EventNameDecTable.RowHeight = UITableView.AutomaticDimension;
                EventNameDataSource = new DataSource.Business.EventNameDataSource(this);
                EventNameDecTable.Source = EventNameDataSource;
                EventNameDecTable.TableFooterView = new UIView();
            }
            catch (Exception)
            {
            }
        }

        private async Task GetEventPic()
        {
            try
            {
                ImageViewImage logo = new ImageViewImage();
                logo.Id = this.BusinessEvent.EventId;
                Uri imageUri = new Uri(await BlobStorageHelper.GetEventLogoUri(this.BusinessEvent.EventId));
                logo.ImageUrl = imageUri;

                if (logo.Image == null)
                {
                    this.BeginDownloadingImage(logo, _EventLogo);
                }

                _EventLogo.Image = logo.Image;
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        /// <summary>
        /// Raises the left swipe detected event.
        /// </summary>
        private void OnLeftSwipeDetected()
        {
            OtherEventInfoController controller = this.Storyboard.InstantiateViewController("OtherEventInfoController") as OtherEventInfoController;
            controller.EventTitle = this.EventTitle;
            controller.EventDescription = this.EventDescription;
            controller.EventStartDateTime = EventStartDateTime;
            controller.EventEndDateTime = EventEndDateTime;
            controller.SelectedLogoFile = this.SelectedLogoFile;
            controller.Mode = this.Mode;
            controller.BusinessEvent = this.BusinessEvent;
            this.NavigationController.PushViewController(controller, true);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                // Left swipe
                UISwipeGestureRecognizer recognizer = new UISwipeGestureRecognizer(OnLeftSwipeDetected);
                recognizer.Direction = UISwipeGestureRecognizerDirection.Left;
                View.AddGestureRecognizer(recognizer);

                LoadData();
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

                    if (CloseController)
                    {
                        CloseController = false;
                        this.NavigationController.PopViewController(true);
                    }
                    else
                    {
                        if (SelectedSection != null)
                        {
                            switch (SelectedSection)
                            {
                                case 0:
                                    this.EventTitle = InputValue;
                                    break;
                                case 1:
                                    this.EventDescription = InputValue;
                                    break;
                            }

                            InputValue = string.Empty;
                            EventNameDecTable.ReloadData();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        partial void _LogoButton_TouchUpInside(UIButton sender)
        {
            AddEventLogo();
        }

        /// <summary>
        /// 
        /// </summary>
        private async void AddEventLogo()
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

                this.SelectedLogoFile = file;
                UIImage image = new UIImage(file.Path);
                _EventLogo.Image = image;
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        #endregion

    }
}