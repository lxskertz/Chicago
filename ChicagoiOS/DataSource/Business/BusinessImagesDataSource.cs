using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using Tabs.Mobile.Shared.Models.Businesses;
using BigTed;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.Shared.Models;

namespace Tabs.Mobile.ChicagoiOS.DataSource.Business
{
    public class BusinessImagesDataSource : UICollectionViewDataSource
    {

        #region Constants, Enums, and Variables

        /// <summary>
        /// Gets or sets the cell
        /// </summary>
        private NSString BusinessPhotosCollectionCell = new NSString("BusinessPhotosCollectionCell");

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<BusinesPhoto> BusinesPhotos { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ImageViewImage> ImageViewImages { get; set; }

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        public BusinessImagesController Controller { get; set; }

        #endregion

        #region Constructors

        public BusinessImagesDataSource(BusinessImagesController controller, List<BusinesPhoto> businesPhotos,
             List<ImageViewImage> ImageViewImage)
        {
            this.Controller = controller;
            this.BusinesPhotos = businesPhotos;
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
            return this.BusinesPhotos.Count;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (BusinessPhotosCollectionCell)collectionView.DequeueReusableCell(BusinessPhotosCollectionCell, indexPath);
            var item = this.BusinesPhotos.ElementAt(indexPath.Row);

            if (item != null)
            {
                var itemLogo = this.ImageViewImages.Where(x => x.Id == item.BusinessPhotoId).FirstOrDefault();
                cell.Tag = indexPath.Row;

                if (itemLogo != null && itemLogo.Image == null)
                {
                    //app.Image = PlaceholderImage;
                    BeginDownloadingImage(itemLogo, indexPath, collectionView);
                }
                cell._Photo.Image = itemLogo.Image;
            }

            return cell;
        }

        private async void BeginDownloadingImage(ImageViewImage logo, NSIndexPath path, UICollectionView collectionView)
        {
            try
            {
                // Queue the image to be downloaded. This task will execute
                // as soon as the existing ones have finished.
                byte[] data = null;

                data = await BlobStorageHelper.GetImageData(logo.ImageUrl);
                logo.Image = UIImage.LoadFromData(NSData.FromArray(data));

                InvokeOnMainThread(() =>
                {
                    var cell = (BusinessPhotosCollectionCell)collectionView.VisibleCells.Where(c => c.Tag == this.ImageViewImages.IndexOf(logo)).FirstOrDefault();
                    if (cell != null)
                        cell._Photo.Image = logo.Image;
                });
            }
            catch (Exception) { }
        }

        #endregion

    }
}