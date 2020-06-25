using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using Tabs.Mobile.Shared.Models.Events;
using BigTed;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.Shared.Models;

namespace Tabs.Mobile.ChicagoiOS.DataSource.Individuals.Events
{
    public class ToastersEventDataSource : UITableViewSource
    {

        #region Constants, Enums, and Variables

        /// <summary>
        /// Gets or sets the cell
        /// </summary>
        private NSString ToastersEventsCell = new NSString("ToastersEventsCell"); 

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<BusinessEvents> BusinessEvents { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ImageViewImage> ImageViewImage { get; set; }

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        public ToastersEventController Controller { get; set; }

        #endregion

        #region Constructors

        public ToastersEventDataSource(ToastersEventController controller, List<BusinessEvents> businessEvents,
             List<ImageViewImage> ImageViewImage)
        {
            this.Controller = controller;
            this.BusinessEvents = businessEvents;
            this.ImageViewImage = ImageViewImage;
        }

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
            var cell = (ToastersEventsCell)tableView.DequeueReusableCell(this.ToastersEventsCell);
            var item = this.BusinessEvents.ElementAt(indexPath.Row);
            cell._ShareBtn.Hidden = true;

            if (item != null)
            {
                var itemLogo = this.ImageViewImage.Where(x => x.Id == item.EventId).FirstOrDefault();
                var userLikedEvent = this.Controller.LikeList.Where(x => x.Key == item.EventId).FirstOrDefault();
                cell._EventTitle.Text = string.IsNullOrEmpty(item.Title) ? "" : item.Title;
                cell._EventVenue.Text = string.IsNullOrEmpty(item.Venue) ? "" : item.Venue;
                var date = item.StartDateTime.HasValue ? item.StartDateTime.Value.ToLongDateString() : "";
                var time = item.StartDateTime.HasValue ? item.StartDateTime.Value.ToShortTimeString() : "";
                cell._EventDate.Text = date + " " + time;
                cell.Tag = indexPath.Row;

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
                cell._EventLogo.ClipsToBounds = true;
                if (itemLogo != null && itemLogo.Image == null)
                {
                    //app.Image = PlaceholderImage;
                    BeginDownloadingImage(itemLogo, indexPath, tableView);
                }
                cell._EventLogo.Image = itemLogo.Image;

                cell.Item = item;
                cell.DataSource = this;
                cell.IndexPath = indexPath;
                cell.LikedEvent = userLikedEvent;
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
        /// Called when a row is touched
        /// </summary>
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.DeselectRow(indexPath, true);
            var item = this.BusinessEvents.ElementAt(indexPath.Row);
            var itemLogo = this.ImageViewImage.Where(x => x.Id == item.EventId).FirstOrDefault();
            var controller = this.Controller.Storyboard.InstantiateViewController("EventInfoController") as EventInfoController;
            controller.BusinessEvents = item;
            controller.EventImage = itemLogo.Image;
            controller.ShowToolbar = true;
            this.Controller.NavigationController.PushViewController(controller, true);
        }

        /// <summary>
        /// return num of rows that will be in the section
        /// </summary>
        /// <param name="tableview"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return this.BusinessEvents.Count;
        }

        /// <summary>
        /// Load more
        /// </summary>
        /// <param name="scrollView"></param>
        /// <param name="willDecelerate"></param>
        public async override void DraggingEnded(UIScrollView scrollView, bool willDecelerate)
        {
            if (this.BusinessEvents.Count == 0 || !this.Controller.loadMore)
            {
                return;
            }

            nfloat currentOffset = scrollView.ContentOffset.Y;
            nfloat maximumOffset = scrollView.ContentSize.Height - scrollView.Frame.Size.Height;

            if (maximumOffset - currentOffset <= -1)
            {
                await this.Controller.ScrolledToBottom();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rows"></param>
        public async void AddRowItems(List<BusinessEvents> rows)
        {
            foreach (var row in rows)
            {
                BusinessEvents.Add(row);
            }

            await GetLogoUris(rows);
            await GetLikeEventList(rows);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task GetLogoUris(List<BusinessEvents> rows)
        {
            foreach (var b in rows)
            {
                ImageViewImage logo = new ImageViewImage();
                logo.Id = b.EventId;
                Uri imageUri = new Uri(await BlobStorageHelper.GetEventLogoUri(b.EventId));
                logo.ImageUrl = imageUri;
                this.ImageViewImage.Add(logo);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task GetLikeEventList(List<BusinessEvents> rows)
        {
            foreach (var b in rows)
            {
                var liked = await AppDelegate.EventLikesFactory.GetEventLike(AppDelegate.CurrentUser.UserId, b.EventId);

                if (liked != null)
                {
                    this.Controller.AddRemoveLike(liked.Liked, b.EventId);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="likeCountTxt"></param>
        public void SetLikeCount(BusinessEvents item, UILabel likeCountTxt)
        {
            var text = item.LikeCount != null ? item.LikeCount > 1 ? item.LikeCount + " likes" : item.LikeCount + " like" : "";
            InvokeOnMainThread(() =>
            {
                likeCountTxt.Text = text;
            });
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
                    var cell = (ToastersEventsCell)tableView.VisibleCells.Where(c => c.Tag == this.ImageViewImage.IndexOf(logo)).FirstOrDefault();
                    if (cell != null)
                        cell._EventLogo.Image = logo.Image;
                });
            }
            catch (Exception) { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="path"></param>
        /// <param name="tableView"></param>
        private async void GetLikeCount(BusinessEvents item, NSIndexPath path, UITableView tableView)
        {
            var count = await AppDelegate.EventLikesFactory.GetLikeCount(item.BusinessId, item.EventId);
            item.LikeCount = count;

            InvokeOnMainThread(() => {
                var cell = (ToastersEventsCell)tableView.VisibleCells.Where(c => c.Tag == this.BusinessEvents.IndexOf(item)).FirstOrDefault();
                if (cell != null)
                {
                    SetLikeCount(item, cell._LikeCount);
                }
            });
        }

        #endregion

        #endregion
    }
}