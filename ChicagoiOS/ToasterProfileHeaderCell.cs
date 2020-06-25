using System;
using Tabs.Mobile.Shared.Resources;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class ToasterProfileHeaderCell : UICollectionReusableView
    {

        public UIImageView _ProfilePic
        {
            get
            {
                return ProfilePic;
            }
        }

        public UILabel _Name
        {
            get
            {
                return Name;
            }
        }

        public UILabel _HomeTown
        {
            get
            {
                return HomeTown;
            }
        }
        public UILabel _Headline
        {
            get
            {
                return Headline;
            }
        }

        public UIButton _ToasterRequest
        {
            get
            {
                return RequestBtn;
            }
        }

        public DataSource.Individuals.ToasterPhotosDataSource ToasterPhotosDataSource { get; set; }

        public ToasterProfileHeaderCell (IntPtr handle) : base (handle)
        {
        }

        partial void UIButton1499487_TouchUpInside(UIButton sender)
        {
            ToasterRequestTouchUpInside();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        async void ToasterRequestTouchUpInside()
        {
            if (RequestBtn.Title(UIControlState.Normal) == "Toasters" ||
                !this.ToasterPhotosDataSource.Controller.FromSearchedUser)
            {
                UIViewController controller = this.ToasterPhotosDataSource.Controller.Storyboard.InstantiateViewController("ToastersController") as ToastersController;
                this.ToasterPhotosDataSource.Controller.NavigationController.PushViewController(controller, true);
            }
            else if (RequestBtn.Title(UIControlState.Normal) == AppText.AcceptRequest)
            {
                await this.ToasterPhotosDataSource.Controller.AcceptRequest(RequestBtn);
            }
            else
            {
                await this.ToasterPhotosDataSource.Controller.SendAddToaster(RequestBtn);
            }
        }
    }
}