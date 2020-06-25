using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using BigTed;
using Tabs.Mobile.Shared.Models.Businesses;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.Shared.Models;

namespace Tabs.Mobile.ChicagoiOS.DataSource.Business
{
    public class BusinessListDataSource : UITableViewSource
    {

        #region Constants, Enums, and Variables

        /// <summary>
        /// Gets or sets the cell
        /// </summary>
        private NSString BusinessListCell = new NSString("BusinessListCell");

        private NSString BusinessListNoLogoCell = new NSString("BusinessListNoLogoCell"); 

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<BusinessSearch> Businesses { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ImageViewImage> ImageViewImage { get; set; }

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        public BusinessListController Controller { get; set; }

        #endregion

        #region Constructors

        public BusinessListDataSource(BusinessListController controller, List<BusinessSearch> businesses,
             List<ImageViewImage> ImageViewImage)
        {
            this.Controller = controller;
            this.Businesses = businesses;
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
            var item = this.Businesses.ElementAt(indexPath.Row);

            if (item != null)
            {
                var itemLogo = this.ImageViewImage.Where(x => x.Id == item.BusinessId).FirstOrDefault();

                if (itemLogo == null)
                {
                    var cell = (BusinessListNoLogoCell)tableView.DequeueReusableCell(this.BusinessListNoLogoCell);
                    cell._BusinessName.Text = string.IsNullOrEmpty(item.BusinessName) ? "" : item.BusinessName;
                    var bar = item.Bar ? AppText.Bar + ", " : "";
                    var club = item.Club ? AppText.Club + ", " : "";
                    var lounge = item.Lounge ? AppText.Lounge + ", " : "";
                    var restaurant = item.Restaurant ? AppText.Restaurant + ", " : "";
                    var other = item.Other ? AppText.Other : "";
                    cell._BusinessType.Text = bar + club + lounge + restaurant + other;

                    //cell.Tag = indexPath.Row;

                    cell._CheckInBtn.Layer.CornerRadius = 4;
                    cell._CheckInBtn.Layer.BorderWidth = 1;
                    cell._CheckInBtn.Layer.BorderColor = UIColor.FromRGB(145, 200, 244).CGColor;
                                     
                    cell.Item = item;
                    cell.DataSource = this;
                    cell.IndexPath = indexPath;

                    return cell;

                }
                else
                {
                    var cell = (BusinessListCell)tableView.DequeueReusableCell(this.BusinessListCell);
                    cell._BusinessName.Text = string.IsNullOrEmpty(item.BusinessName) ? "" : item.BusinessName;
                    var bar = item.Bar ? AppText.Bar + ", " : "";
                    var club = item.Club ? AppText.Club + ", " : "";
                    var lounge = item.Lounge ? AppText.Lounge + ", " : "";
                    var restaurant = item.Restaurant ? AppText.Restaurant + ", " : "";
                    var other = item.Other ? AppText.Other : "";
                    cell._BusinessType.Text = bar + club + lounge + restaurant + other;

                    cell.Tag = indexPath.Row;

                    cell._CheckInBtn.Layer.CornerRadius = 4;
                    cell._CheckInBtn.Layer.BorderWidth = 1;
                    cell._CheckInBtn.Layer.BorderColor = UIColor.FromRGB(145, 200, 244).CGColor;

                    cell._BusinessLogo.ClipsToBounds = true;
                    if (itemLogo != null)
                    {
                        if (itemLogo.Image == null)
                        {
                            //app.Image = PlaceholderImage;
                            BeginDownloadingImage(itemLogo, indexPath, tableView);
                        }
                        cell._BusinessLogo.Image = itemLogo.Image;
                    }

                    cell._BusinessLogo.Layer.BorderWidth = 1;
                    cell._BusinessLogo.Layer.BorderColor = UIColor.FromRGB(223, 223, 223).CGColor;

                    cell.Item = item;
                    cell.DataSource = this;
                    cell.IndexPath = indexPath;

                    return cell;
                }
            }

            return new UITableViewCell();

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
            var item = this.Businesses.ElementAt(indexPath.Row);
            var itemLogo = this.ImageViewImage.Where(x => x.Id == item.BusinessId).FirstOrDefault();
            var controller = this.Controller.Storyboard.InstantiateViewController("BusinessProfileController") as BusinessProfileController;
            controller.BusinessSearchInfo = item;
            controller.BusinessSearchInfoImage = itemLogo != null ? itemLogo.Image : null;
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
            return this.Businesses.Count;
        }

        /// <summary>
        /// Load more
        /// </summary>
        /// <param name="scrollView"></param>
        /// <param name="willDecelerate"></param>
        public async override void DraggingEnded(UIScrollView scrollView, bool willDecelerate)
        {
            if (this.Businesses.Count == 0 || !this.Controller.loadMore)
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
        public async void AddRowItems(List<BusinessSearch> rows)
        {
            foreach (var row in rows)
            {
                Businesses.Add(row);
            }

            await GetLogoUris(rows);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task GetLogoUris(List<BusinessSearch> rows)
        {
            foreach (var b in rows)
            {
                ImageViewImage logo = new ImageViewImage();
                logo.Id = b.BusinessId;
                var uriString = await BlobStorageHelper.GetBusinessLogoUri(b.BusinessId);
                if (!string.IsNullOrEmpty(uriString))
                {
                    Uri imageUri = new Uri(uriString); //new Uri(await Shared.Helpers.BlobStorageHelper.GetBusinessLogoUri(b.BusinessId));
                    logo.ImageUrl = imageUri;
                    this.ImageViewImage.Add(logo);
                }

                //ImageViewImage logo = new ImageViewImage();
                //logo.Id = b.BusinessId;
                //Uri imageUri = new Uri(await BlobStorageHelper.GetBusinessLogoUri(b.BusinessId));
                //logo.ImageUrl = imageUri;
                //this.ImageViewImage.Add(logo);
            }
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
                    //var cell = tableView.VisibleCells.Where(c => c.Tag == this.ImageViewImage.IndexOf(logo)).FirstOrDefault();

                    if (cell != null && cell is BusinessListCell)
                    {
                        var bcell = (BusinessListCell)cell;
                        bcell._BusinessLogo.Image = logo.Image;
                    }
                });
            }
            catch (Exception) { }
        }

        #endregion

    }
}