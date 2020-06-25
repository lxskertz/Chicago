using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using Tabs.Mobile.Shared.Models.Businesses;
using Tabs.Mobile.Shared.Models.Individuals;
using BigTed;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.Shared.Models;

namespace Tabs.Mobile.ChicagoiOS.DataSource.Individuals
{
    public class ToasterPhotosDataSource : UICollectionViewDataSource
    {

        #region Constants, Enums, and Variables

        /// <summary>
        /// Gets or sets the cell
        /// </summary>
        private NSString ToasterPhotoCollectionCell = new NSString("ToasterPhotoCollectionCell");

        private NSString ToasterProfileHeaderCell = new NSString("ToasterProfileHeaderCell"); 

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<ToasterPhoto> ToasterPhotos { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ImageViewImage> ImageViewImages { get; set; }

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        public ToasterProfileController Controller { get; private set; }

        #endregion

        #region Constructors

        public ToasterPhotosDataSource(ToasterProfileController controller, List<ToasterPhoto> toasterPhotos,
             List<ImageViewImage> ImageViewImage)
        {
            this.Controller = controller;
            this.ToasterPhotos = toasterPhotos;
            this.ImageViewImages = ImageViewImage;
        }

        #endregion

        #region Methods

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return this.ToasterPhotos.Count;
        }

        public override UICollectionReusableView GetViewForSupplementaryElement(UICollectionView collectionView, NSString elementKind, NSIndexPath indexPath)
        {
            var headerView = (ToasterProfileHeaderCell)collectionView.DequeueReusableSupplementaryView(elementKind, ToasterProfileHeaderCell, indexPath);

            headerView.ToasterPhotosDataSource = this;
            headerView._ToasterRequest.Layer.CornerRadius = 4;
            headerView._ToasterRequest.Layer.BorderWidth = 1;
            headerView._ToasterRequest.Layer.BorderColor = UIColor.FromRGB(145, 200, 244).CGColor;

            this.Controller.ToasterRequest = headerView._ToasterRequest;
            this.Controller.Headline = headerView._Headline;
            this.Controller.Name = headerView._Name;
            this.Controller.HomeTown = headerView._HomeTown;
            this.Controller.ProfilePic = headerView._ProfilePic;

            this.Controller.LoadProfileInfo();

            return headerView;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (ToasterPhotoCollectionCell)collectionView.DequeueReusableCell(ToasterPhotoCollectionCell, indexPath);
            var item = this.ToasterPhotos.ElementAt(indexPath.Row);

            if (item != null)
            {
                var itemLogo = this.ImageViewImages.Where(x => x.Id == item.ToasterPhotoId).FirstOrDefault();
                cell.Tag = indexPath.Row;

                if (itemLogo != null && itemLogo.Image == null)
                {
                    //app.Image = PlaceholderImage;
                    BeginDownloadingImage(itemLogo, indexPath, collectionView);
                }
                cell._Photo.Image = itemLogo != null ? itemLogo.Image : null;
            }

            return cell;
        }

        private async void BeginDownloadingImage(ImageViewImage logo, NSIndexPath path, UICollectionView collectionView)
        {
            try
            {
                // Queue the image to be downloaded. This task will execute
                // as soon as the existing ones have finished.

                if (logo.ImageUrl != null)
                {
                    byte[] data = null;
                    data = await BlobStorageHelper.GetImageData(logo.ImageUrl);
                    logo.Image = UIImage.LoadFromData(NSData.FromArray(data));

                    InvokeOnMainThread(() =>
                    {
                        var cell = (ToasterPhotoCollectionCell)collectionView.VisibleCells.Where(c => c.Tag == this.ImageViewImages.IndexOf(logo)).FirstOrDefault();
                        if (cell != null)
                            cell._Photo.Image = logo.Image;
                    });
                }
            }
            catch (Exception) { }
        }

        #endregion
    }
}