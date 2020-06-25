using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS.Delegates
{
    public class BusinessPhotoDelegate : UICollectionViewDelegateFlowLayout
    {

        #region Properties      
        
        private BaseViewController Controller { get; set; }

        #endregion

        #region Constructors

        public BusinessPhotoDelegate() { }

        public BusinessPhotoDelegate(BaseViewController controller) {
            this.Controller = controller;
        }

        #endregion

        #region Methods

        public override UIEdgeInsets GetInsetForSection(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            return new UIEdgeInsets(10.0f, 10.0f, 5.0f, 5.0f);
        }

        public override nfloat GetMinimumInteritemSpacingForSection(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            return 10.0f;
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            //base.ItemSelected(collectionView, indexPath);
            if (this.Controller is ToasterProfileController)
            {
                var profileController = (ToasterProfileController)this.Controller;
                var item = profileController.ToasterPhotosDataSource.ToasterPhotos.ElementAt(indexPath.Row);
                var itemLogo = profileController.ToasterPhotosDataSource.ImageViewImages.Where(x => x.Id == item.ToasterPhotoId).FirstOrDefault();

                if (itemLogo != null && itemLogo.Image != null)
                {
                    var controller = this.Controller.Storyboard.InstantiateViewController("MyImageViewController") as MyImageViewController;
                    controller.SelectedImage = itemLogo.Image;
                    this.Controller.NavigationController.PushViewController(controller, true);
                }
            }
        }

        //public override CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        //{
        //    return base.GetSizeForItem(collectionView, layout, indexPath);
        //}

        #endregion

    }
}