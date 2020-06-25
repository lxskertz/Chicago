using Foundation;
using System;
using System.Threading.Tasks;
using UIKit;
using Tabs.Mobile.Shared.Models.Individuals;
using Tabs.Mobile.Shared.Resources;
using BigTed;
using CoreGraphics;
using Plugin.Media;
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.Shared.Models;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class EditToasterProfileController : BaseViewController
    {

        #region Properties

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        private DataSource.Individuals.EditToasterProfileDataSource EditToasterProfileDataSource { get; set; }

        /// <summary>
        /// Gets or sets first name
        /// </summary>
        public UITextField Firstname { get; set; }

        /// <summary>
        /// Gets or sets last name
        /// </summary>
        public UITextField Lastname { get; set; }

        /// <summary>
        /// Gets or sets phone number
        /// </summary>
        public UITextField PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets phone number
        /// </summary>
        public UITextField Headline { get; set; }

        /// <summary>
        /// Gets or sets phone number
        /// </summary>
        public UITextField Location { get; set; }

        /// <summary>
        /// Gets or sets phone number
        /// </summary>
        //public UITextField Age { get; set; }

        /// <summary>
        /// Gets or sets phone number
        /// </summary>
        //public UITextField ProfileDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public UISwitch Male { get; set; }

        /// <summary>
        /// Gets or sets
        /// </summary>
        public UISwitch Female { get; set; }

        /// <summary>
        /// Gets or sets
        /// </summary>
        public UISwitch OtherSex { get; set; }

        /// <summary>
        /// Gets or sets
        /// </summary>
        public UISwitch PrivateAccount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Individual Individual { get; set; } = new Individual();

        #endregion

        #region Constructors

        public EditToasterProfileController (IntPtr handle) : base (handle)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load Data
        /// </summary>
        private void LoadData()
        {
            try
            {
                EditToasterProfileTable.EstimatedRowHeight = 44f;
                EditToasterProfileTable.RowHeight = UITableView.AutomaticDimension;
                this.EditToasterProfileDataSource = new DataSource.Individuals.EditToasterProfileDataSource(this);
                EditToasterProfileTable.Source = EditToasterProfileDataSource;
                EditToasterProfileTable.TableFooterView = new UIView();

                UITapGestureRecognizer tapGesture = new UITapGestureRecognizer(() =>
                {
                    EditToasterProfileTable.EndEditing(true);
                });

                tapGesture.CancelsTouchesInView = false;
                EditToasterProfileTable.AddGestureRecognizer(tapGesture);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animated"></param>
        public override void ViewDidAppear(bool animated)
        {
            try
            {
                base.ViewDidAppear(animated);

                if (Female != null)
                {
                    this.Female.ValueChanged += delegate
                    {
                        Male.On = !Female.On;
                        OtherSex.On = !Female.On;
                    };
                }

                if (Male != null)
                {
                    this.Male.ValueChanged += delegate
                    {
                        Female.On = !Male.On;
                        OtherSex.On = !Male.On;
                    };
                }

                if (OtherSex != null)
                {
                    this.OtherSex.ValueChanged += delegate
                    {
                        Male.On = !OtherSex.On;
                        Female.On = !OtherSex.On;
                    };
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public async override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Save", UIBarButtonItemStyle.Plain, (sender, args) =>
                {
                    EditToasterProfile();
                }), true);

                await GetProfileInfo();
                await GetProfilePic();
                LoadData();
            }
            catch (Exception ex)
            {
                var a = ex;
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
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else
                {
                    BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);
                    this.Individual = await AppDelegate.IndividualFactory.GetToasterByUserId(AppDelegate.CurrentUser.UserId);
 
                    BTProgressHUD.Dismiss();
                }
            }
            catch (Exception)
            {
                BTProgressHUD.Dismiss();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async void EditToasterProfile()
        {
            try
            {
                if (string.IsNullOrEmpty(Firstname.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.EmptyFirstname, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else if (string.IsNullOrEmpty(Lastname.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.EmptyLastname, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else
                {
                    if (AppDelegate.IsOfflineMode())
                    {
                        BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);

                        return;
                    }
                    else
                    {
                        BTProgressHUD.Show(ToastMessage.Saving, -1f, ProgressHUD.MaskType.Black);
                        AppDelegate.CurrentUser.LastName = Lastname.Text.Trim();
                        AppDelegate.CurrentUser.FirstName = Firstname.Text.Trim();
                        AppDelegate.SaveCurrentUser(AppDelegate.CurrentUser);
                        await AppDelegate.UsersFactory.EditUser(AppDelegate.CurrentUser);

                        if (this.Individual == null)
                        {
                            this.Individual = new Individual();
                        }

                        this.Individual.Headline = Headline != null ? Headline.Text.Trim() : this.Individual.Headline;
                        this.Individual.HomeTown = Location != null ? Location.Text.Trim() : this.Individual.HomeTown;
                        this.Individual.Female = Female != null ? Female.On : this.Individual.Female;
                        this.Individual.Male = Male != null ? Male.On : this.Individual.Male;
                        this.Individual.OtherSex = OtherSex != null ? OtherSex.On : this.Individual.OtherSex;
                        this.Individual.ProfileDescription = ""; //ProfileDescription != null ? ProfileDescription.Text.Trim() : this.Individual.ProfileDescription;
                        this.Individual.PrivateAccount = PrivateAccount != null ? PrivateAccount.On : this.Individual.PrivateAccount;
                        await AppDelegate.IndividualFactory.EditIndividual(this.Individual);

                        BTProgressHUD.Dismiss();
                        ToasterProfileController.RequireRefresh = true;
                        ToasterProfileController.UpdatedHeadline = Headline != null ? Headline.Text.Trim() : this.Individual.Headline;
                        ToasterProfileController.UpdatedHometown = Location != null ? Location.Text.Trim() : this.Individual.HomeTown;
                        this.NavigationController.PopViewController(false);
                    }
                }
            }
            catch (Exception)
            {
                BTProgressHUD.Dismiss();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task GetProfilePic()
        {
            try
            {
                ImageViewImage itemLogo = new ImageViewImage();
                itemLogo.Id = AppDelegate.CurrentUser.UserId;
                itemLogo.ImageUrl = new Uri(await BlobStorageHelper.GetToasterBlobUri(AppDelegate.CurrentUser.UserId));

                if (itemLogo.Image == null)
                {
                    this.BeginDownloadingImage(itemLogo, ProfilePicture);
                }

                ProfilePicture.Image = itemLogo.Image;
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        private async void EditToasterProfilePic()
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

                UIImage image = new UIImage(file.Path);
                ProfilePicture.Image = image;

                if (!AppDelegate.IsOfflineMode())
                {
                    ToasterProfileController.ProfilePicUpdated = true;
                    BTProgressHUD.Show(ToastMessage.Updating, -1f, ProgressHUD.MaskType.Black);
                    await BlobStorageHelper.SaveToasterProfileBlob(file.Path, AppDelegate.CurrentUser.UserId);
                    BTProgressHUD.Dismiss();
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                BTProgressHUD.Dismiss();
            }
        }

        partial void EditPicBtn_TouchUpInside(UIButton sender)
        {
            EditToasterProfilePic();
        }

        #endregion

    }
}