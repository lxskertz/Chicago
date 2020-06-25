using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using Tabs.Mobile.Shared.Models.CheckIns;
using BigTed;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.Shared.Models;

namespace Tabs.Mobile.ChicagoiOS.DataSource.Individuals.CheckIns
{
    public class LiveToastersDataSource : UITableViewSource
    {

        #region Constants, Enums, and Variables

        /// <summary>
        /// Gets or sets the cell
        /// </summary>
        private NSString LiveToastersCell = new NSString("LiveToastersCell"); 

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<CheckIn> CheckIns { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ImageViewImage> ImageViewImage { get; set; }

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        public LiveToastersEventsController Controller { get; set; }

        #endregion

        #region Constructors

        public LiveToastersDataSource(LiveToastersEventsController controller, List<CheckIn> checkIn,
             List<ImageViewImage> ImageViewImage)
        {
            this.Controller = controller;
            this.CheckIns = checkIn;
            this.ImageViewImage = ImageViewImage;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a table cell for the row indicated by row property of the NSIndexPath
        /// This method is called multiple times to populate each row of the table.
        /// The method automatically uses cells that have scrolled off the screen or creates new ones as necessary
        /// </summary>
        /// <param name="tableView"></param>
        /// <param name="indexPath"></param>
        /// <returns></returns>
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = (LiveToastersCell)tableView.DequeueReusableCell(this.LiveToastersCell);
            var item = this.CheckIns.ElementAt(indexPath.Row);

            if (item != null)
            {
                var itemLogo = this.ImageViewImage.Where(x => x.Id == item.CheckInId).FirstOrDefault();
                var userLikedEvent = this.Controller.CheckInsLikeList.Where(x => x.Key == item.CheckInId).FirstOrDefault();

                var username = string.IsNullOrEmpty(item.Username) ? "" : item.Username;
                var businessName = string.IsNullOrEmpty(item.BusinessName) ? "" : " is at " + item.BusinessName;

                cell._Username.Text = username + businessName;
                
                cell.Tag = indexPath.Row;

                cell._SendDrinkBtn.Layer.CornerRadius = 4;
                cell._SendDrinkBtn.Layer.BorderWidth = 1;
                cell._SendDrinkBtn.Layer.BorderColor = UIColor.FromRGB(145, 200, 244).CGColor;
                cell._SendDrinkBtn.Hidden = item.UserId == AppDelegate.CurrentUser.UserId;

                cell._MoreBtn.Hidden = item.UserId == AppDelegate.CurrentUser.UserId;

                if (userLikedEvent.Value)
                {
                    cell._LikeBtn.SetImage(UIImage.FromFile("Favorites/favorite_24pt"), UIControlState.Normal);
                }
                else
                {
                    cell._LikeBtn.SetImage(UIImage.FromFile("Favorites/favorite_border_24pt"), UIControlState.Normal);
                }

                if (item.LikeCount == null)
                {
                    GetLikeCount(item, indexPath, tableView);
                }
                SetLikeCount(item, cell._LikeCount);

                // If the Image for this App has not been downloaded,
                // use the Placeholder image while we try to download
                // the real image from the web.
                cell._UserImage.ClipsToBounds = true;
                if (itemLogo != null)
                {
                    if (itemLogo != null && itemLogo.Image == null)
                    {
                        //app.Image = PlaceholderImage;
                        BeginDownloadingImage(itemLogo, indexPath, tableView);
                    }
                    cell._UserImage.Image = itemLogo.Image;
                }

                cell.Item = item;
                cell.DataSource = this;
                cell.IndexPath = indexPath;
                cell.LikedCheckIn = userLikedEvent;
            }

            return cell;

        }

        /// <summary>
        /// Gets number of section.... which is 1 in this case
        /// </summary>
        /// <param name="tableView"></param>
        /// <returns></returns>
        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="likeCountTxt"></param>
        public void SetLikeCount(CheckIn item, UILabel likeCountTxt)
        {
            var text = item.LikeCount != null ? item.LikeCount > 1 ? item.LikeCount + " likes" : item.LikeCount + " like" : "";
            InvokeOnMainThread(() =>
            {
                likeCountTxt.Text = text;
            });
        }

        /// <summary>
        /// Called when a row is touched
        /// </summary>
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.DeselectRow(indexPath, true);
            var item = this.CheckIns.ElementAt(indexPath.Row);
            var itemLogo = this.ImageViewImage.Where(x => x.Id == item.CheckInId).FirstOrDefault();

            if (itemLogo != null && itemLogo.Image != null)
            {
                var controller = this.Controller.Storyboard.InstantiateViewController("MyImageViewController") as MyImageViewController;
                controller.SelectedImage = itemLogo.Image;
                this.Controller.NavigationController.PushViewController(controller, true);
            }
        }

        /// <summary>
        /// return num of rows that will be in the section
        /// </summary>
        /// <param name="tableview"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return this.CheckIns.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logo"></param>
        /// <param name="path"></param>
        /// <param name="tableView"></param>
        private async void BeginDownloadingImage(ImageViewImage logo, NSIndexPath path, UITableView tableView)
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
                    var cell = tableView.VisibleCells.Where(c => c.Tag == path.Row).FirstOrDefault();
                    if (cell != null && cell is LiveToastersCell)
                    {
                        var bcell = (LiveToastersCell)cell;
                        bcell._UserImage.Image = logo.Image;
                    }

                    //var cell = (LiveToastersCell)tableView.VisibleCells.Where(c => c.Tag == this.ImageViewImage.IndexOf(logo)).FirstOrDefault();
                    //if (cell != null)
                    //    cell._UserImage.Image = logo.Image;
                });
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="path"></param>
        /// <param name="tableView"></param>
        private async void GetLikeCount(CheckIn item, NSIndexPath path, UITableView tableView)
        {
            var count = await AppDelegate.CheckInLikesFactory.GetLikeCount(item.CheckInId);
            item.LikeCount = count;

            InvokeOnMainThread(() => {
                var cell = (LiveToastersCell)tableView.VisibleCells.Where(c => c.Tag == this.CheckIns.IndexOf(item)).FirstOrDefault();
                if (cell != null)
                {
                    SetLikeCount(item, cell._LikeCount);
                }
            });
        }

        #endregion

    }
}