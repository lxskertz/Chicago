using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Views.InputMethods;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Models.Users;
using Tabs.Mobile.Shared.Models.Individuals;
using Plugin.Media;
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.Shared.Models;

namespace Tabs.Mobile.ChicagoAndroid.Activities.Individuals
{
    [Activity(Label = "Edit Profile", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class EditToasterProfileActivity : BaseActivity
    {

        #region Constants, Enums, and Variables

        private AppCompatEditText fname;
        private AppCompatEditText lname;
        private AppCompatEditText phoneNumber;
        private AppCompatEditText headline;
        private AppCompatEditText hometown;
        private AppCompatEditText age;
        private AppCompatEditText profileDescription;
        private TextInputLayout fnameLayout;
        private TextInputLayout lastNameLayout;
        private TextInputLayout phoneNumberLayout;
        private TextInputLayout headlineLayout;
        private TextInputLayout hometownLayout;
        private TextInputLayout ageLayout;
        private TextInputLayout profileDescriptionLayout;
        private FrameLayout pageLayout;
        private RadioButton maleRadio;
        private RadioButton femaleRadio;
        private RadioButton otherSexRadio;
        private RadioButton privateRadio;
        private ImageView profilePicture;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Individual Individual { get; set; } = new Individual();

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
                    EditToasterProfile();
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
                SetContentView(Resource.Layout.EditToasterProfile);

                //add the back arrow
                this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                this.SupportActionBar.SetDisplayShowHomeEnabled(true);

                fnameLayout = FindViewById<TextInputLayout>(Resource.Id.firstname_layout);
                lastNameLayout = FindViewById<TextInputLayout>(Resource.Id.lastname_layout);
                phoneNumberLayout = FindViewById<TextInputLayout>(Resource.Id.phoneNumber_layout);
                headlineLayout = FindViewById<TextInputLayout>(Resource.Id.headline_layout);
                hometownLayout = FindViewById<TextInputLayout>(Resource.Id.location_layout);
                ageLayout = FindViewById<TextInputLayout>(Resource.Id.age_layout);
                profileDescriptionLayout = FindViewById<TextInputLayout>(Resource.Id.profileDesc_layout);
                pageLayout = FindViewById<FrameLayout>(Resource.Id.editToasterProfilelayout);
                profilePicture = FindViewById<ImageView>(Resource.Id.profilePic);

                fname = FindViewById<AppCompatEditText>(Resource.Id.firstName);
                lname = FindViewById<AppCompatEditText>(Resource.Id.lastName);
                phoneNumber = FindViewById<AppCompatEditText>(Resource.Id.phoneNumber);
                headline = FindViewById<AppCompatEditText>(Resource.Id.headline);
                hometown = FindViewById<AppCompatEditText>(Resource.Id.location);
                age = FindViewById<AppCompatEditText>(Resource.Id.age);
                profileDescription = FindViewById<AppCompatEditText>(Resource.Id.profileDesc);

                age.Visibility = ViewStates.Gone;
                profileDescription.Visibility = ViewStates.Gone;

                maleRadio = FindViewById<RadioButton>(Resource.Id.isMale);
                femaleRadio = FindViewById<RadioButton>(Resource.Id.isFemale);
                otherSexRadio = FindViewById<RadioButton>(Resource.Id.isOtherSex);
                privateRadio = FindViewById<RadioButton>(Resource.Id.isPrivate);

                fname.Text = string.IsNullOrEmpty(this.CurrentUser.FirstName) ? string.Empty : this.CurrentUser.FirstName;
                lname.Text = string.IsNullOrEmpty(this.CurrentUser.LastName) ? string.Empty : this.CurrentUser.LastName;
                phoneNumber.Text = this.CurrentUser.PhoneNumber.ToString();

                profilePicture.Click += delegate
                {
                    AddEditProfilePicture();
                };

                await GetProfileInfo();
                await GetProfilePicture();
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
                if (this.CheckNetworkConnectivity() == null)
                {
                    this.ShowSnack(this.pageLayout, ToastMessage.NoInternet, "OK");
                    return;
                }
                else
                {
                    this.ShowProgressbar(true, "", ToastMessage.Loading);
                    this.Individual = await App.IndividualFactory.GetToasterByUserId(this.CurrentUser.UserId);

                    if(Individual != null)
                    {
                        headline.Text =string.IsNullOrEmpty( this.Individual.Headline) ? string.Empty : this.Individual.Headline;
                        hometown.Text = string.IsNullOrEmpty(this.Individual.HomeTown) ? string.Empty : this.Individual.HomeTown;
                        femaleRadio.Checked = this.Individual.Female;
                        maleRadio.Checked = this.Individual.Male;
                        otherSexRadio.Checked = this.Individual.OtherSex;
                        privateRadio.Checked = this.Individual.PrivateAccount;
                        //age.Text = string.IsNullOrEmpty(this.Individual.)
                        //profileDescription.Text = string.IsNullOrEmpty(this.Individual.ProfileDescription) ? string.Empty : this.Individual.ProfileDescription;
                    }

                    this.ShowProgressbar(false, "", ToastMessage.Loading);
                }
            }
            catch (Exception)
            {
                this.ShowProgressbar(false, "", ToastMessage.Loading);
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
                if (!ValidateInput(fnameLayout, fname, ToastMessage.EmptyFirstname))
                {
                    return;
                }
                else if (!ValidateInput(lastNameLayout, lname, ToastMessage.EmptyLastname))
                {
                    return;
                }
                else
                {
                    if (CheckNetworkConnectivity() == null)
                    {
                        ShowSnack(pageLayout, ToastMessage.NoInternet, "OK");
                        return;
                    }
                    else
                    {
                        this.ShowProgressbar(true, "", ToastMessage.Saving);
                        this.CurrentUser.LastName = lname.Text.Trim();
                        this.CurrentUser.FirstName = fname.Text.Trim();
                        this.SaveCrendentials(this.CurrentUser);
                        await App.UsersFactory.EditUser(this.CurrentUser);

                        if (this.Individual == null)
                        {
                            this.Individual = new Individual();
                        }

                        this.Individual.Headline = headline.Text.Trim();
                        this.Individual.HomeTown = hometown.Text.Trim();
                        this.Individual.Female = femaleRadio.Checked;
                        this.Individual.Male = maleRadio.Checked;
                        this.Individual.OtherSex = otherSexRadio.Checked;
                        this.Individual.ProfileDescription = ""; //profileDescription.Text.Trim();
                        this.Individual.PrivateAccount = privateRadio.Checked;
                        await App.IndividualFactory.EditIndividual(this.Individual);

                        this.ShowProgressbar(false, "", ToastMessage.Saving);

                        Fragments.Individuals.ToasterProfileFragment.RequireRefresh = true;
                        Fragments.Individuals.ToasterProfileFragment.UpdatedHeadline = headline.Text.Trim();
                        Fragments.Individuals.ToasterProfileFragment.UpdatedHometown = hometown.Text.Trim();
                        this.Finish();
                    }
                }
            }
            catch (Exception)
            {
                this.ShowProgressbar(false, "", ToastMessage.Saving);
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
                if (this.CheckNetworkConnectivity() == null)
                {
                    this.ShowSnack(this.pageLayout, ToastMessage.NoInternet, "OK");
                    return;
                }

                ImageViewImage itemLogo = new ImageViewImage();
                itemLogo.Id = this.CurrentUser.UserId;
                itemLogo.ImageUrl = await GetUserAvatarUri(this.CurrentUser.UserId);

                if (itemLogo.ImageBitmap == null)
                {
                    this.BeginDownloadingImage(itemLogo, profilePicture);
                }

                profilePicture.SetImageBitmap(itemLogo.ImageBitmap);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Add/Edit profile picture
        /// </summary>
        private async void AddEditProfilePicture()
        {
            try
            {
                if (this.CheckNetworkConnectivity() == null)
                {
                    Toast.MakeText(this, ToastMessage.NoInternet, ToastLength.Short).Show();
                }
                else
                {
                    if (!CrossMedia.Current.IsPickPhotoSupported)
                    {
                        return;
                    }
                    var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                    {
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small
                    });


                    if (file == null)
                    {
                        return;
                    }
                    var ImageUri = Android.Net.Uri.Parse(file.Path);
                    profilePicture.SetImageURI(ImageUri);

                    if (file != null)
                    {
                        this.ShowProgressbar(true, "", ToastMessage.Updating);
                        Fragments.Individuals.ToasterProfileFragment.ProfilePicUpdated = true;
                        await BlobStorageHelper.SaveToasterProfileBlob(file.Path, this.CurrentUser.UserId);
                        this.ShowProgressbar(false, "", ToastMessage.Updating);
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