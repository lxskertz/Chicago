using Foundation;
using System;
using UIKit;
using Tabs.Mobile.Shared.Models;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class BaseViewController : UIViewController
    {

        #region Constructors

        public BaseViewController() { }

        public BaseViewController(IntPtr handle) : base(handle) { }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        public void AddButtonBorder(UIButton button)
        {
            button.Layer.CornerRadius = 4;
            button.Layer.BorderWidth = 1;
            button.Layer.BorderColor = UIColor.FromRGB(145, 200, 244).CGColor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public string DetermineEmail(string username)
        {
            string Username = "";

            if (username.Contains("@@dev|"))
            {
                Username = username.Remove(0, 6);

                if (Username.Contains("|"))
                {
                    var index = Username.IndexOf('|');
                    Username = Username.Remove(0, index + 1);

                    return Username;
                }

                return Username;
            }
            else if (username.Contains("@@staging|"))
            {
                Username = username.Remove(0, 10);

                if (Username.Contains("|"))
                {
                    var index = Username.IndexOf('|');
                    Username = Username.Remove(0, index + 1);

                    return Username;
                }

                return Username;
            }
            else
            {
                Username = username;

                if (Username.Contains("|"))
                {
                    var index = Username.IndexOf('|');
                    Username = Username.Remove(0, index + 1);

                    return Username;

                }

                return Username;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool IsValidEmail(string formattedEmail)
        {
            var email = DetermineEmail(formattedEmail);
            if (System.Text.RegularExpressions.Regex.Match(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
                this.NavigationController.NavigationBarHidden = false;
                this.NavigationItem.HidesBackButton = false;

                this.NavigationController.NavigationBar.BarTintColor = UIColor.FromRGB(145, 200, 244);
                this.NavigationController.NavigationBar.TintColor = UIColor.White;
                this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes()
                {
                    ForegroundColor = UIColor.White
                };

                //this.EdgesForExtendedLayout = UIRectEdge.None;
            }
            catch (Exception)
            {
            }

        }

        /// <summary>
        /// Overrides PreferredStatusBarStyle
        /// </summary>
        /// <returns>The status bar style.</returns>
        public override UIStatusBarStyle PreferredStatusBarStyle()
        {
            return UIStatusBarStyle.LightContent;
        }

        /// <summary>
        /// Check if there is network connection available
        /// </summary>
        public void CheckNetWorkConnection()
        {
            if (AppDelegate.DetermineNetworkConnection() != NetworkStatus.NotReachable)
            {
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        public void MakeImageViewRound(UIImageView image)
        {
            image.Layer.BorderWidth = 1;
            image.Layer.BorderColor = UIColor.FromRGB(145, 200, 244).CGColor;
            image.Layer.MasksToBounds = false;
            image.Layer.CornerRadius = image.Frame.Height / 2;
            image.ClipsToBounds = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void DidReceiveMemoryWarning ()
        {
            base.DidReceiveMemoryWarning ();
            // Release any cached data, images, etc that aren't in use.
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logo"></param>
        /// <param name="path"></param>
        /// <param name="tableView"></param>
        public async void BeginDownloadingImage(ImageViewImage logo, UIImageView imageView)
        {
            try
            {
                // Queue the image to be downloaded. This task will execute
                // as soon as the existing ones have finished.
                byte[] data = null;

                data = await Shared.Helpers.BlobStorageHelper.GetImageData(logo.ImageUrl);
                logo.Image = UIImage.LoadFromData(NSData.FromArray(data));

                InvokeOnMainThread(() =>
                {
                    imageView.Image = logo.Image;
                });
            }
            catch (Exception)
            {
            }
        }

        #endregion
    }
}