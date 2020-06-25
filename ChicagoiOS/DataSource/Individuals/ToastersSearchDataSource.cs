using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Models.Individuals;

namespace Tabs.Mobile.ChicagoiOS.DataSource.Individuals
{
    public class ToastersSearchDataSource : UITableViewSource
    {

        #region Constants, Enums, and Variables

        /// <summary>
        /// Gets or sets the cell
        /// </summary>
        private NSString ToastersSearchCell = new NSString("ToastersSearchCell");

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        public ToastersSearchViewController Controller { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ToastersSearchItem> Rows { get; set; } = new List<ToastersSearchItem>();

        public List<ImageViewImage> ImageViewImages { get; set; }

        #endregion

        #region Constructors

        public ToastersSearchDataSource(ToastersSearchViewController controller,
            List<ToastersSearchItem> rows, List<ImageViewImage> imageViewImages)
        {
            this.Controller = controller;
            this.Rows = rows;
            this.ImageViewImages = imageViewImages;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rows"></param>
        public async void AddRowItems(List<ToastersSearchItem> rows)
        {
            foreach (var row in rows)
            {
                Rows.Add(row);
            }
            await GetPicUris(rows);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task GetPicUris(List<ToastersSearchItem> rows)
        {
            foreach (var b in rows)
            {
                ImageViewImage itemLogo = new ImageViewImage();
                itemLogo.Id = b.UserId;
                var uriString = await Shared.Helpers.BlobStorageHelper.GetToasterBlobUri(b.UserId);
                if (!string.IsNullOrEmpty(uriString))
                {
                    Uri imageUri = new Uri(uriString);
                    itemLogo.ImageUrl = imageUri;
                    this.ImageViewImages.Add(itemLogo);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        private void AddButtonBorder(UIButton button)
        {
            button.Layer.CornerRadius = 4;
            button.Layer.BorderWidth = 1;
            button.Layer.BorderColor = UIColor.FromRGB(145, 200, 244).CGColor;
        }

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
            var row = this.Rows[indexPath.Row];
            var cell = (ToastersSearchCell)tableView.DequeueReusableCell(this.ToastersSearchCell);
            cell.Name.Text = !string.IsNullOrEmpty(row.Name) ? row.Name : string.Empty;
            cell.Username.Text = !string.IsNullOrEmpty(row.Username) ? row.Username : string.Empty;
            this.Controller.MakeImageViewRound(cell._ProfilePic);
            AddButtonBorder(cell.RequestBtn);
            cell.RequestBtn.Hidden = true;
            cell.Item = row;
            cell.DataSource = this;
            cell.IndexPath = indexPath;
            cell.Tag = indexPath.Row;

            var itemLogo = this.ImageViewImages.Where(x => x.Id == row.UserId).FirstOrDefault();

            if (itemLogo != null)
            {
                if (itemLogo.Image == null)
                {
                    //app.Image = PlaceholderImage;
                    BeginDownloadingImage(itemLogo, indexPath, tableView);
                }
                cell._ProfilePic.Image = itemLogo.Image;
            }
            else
            {
                cell._ProfilePic.Image = null;
            }

            return cell;

        }

        private async void BeginDownloadingImage(ImageViewImage logo, NSIndexPath path, UITableView tableView)
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
                    //var cell = (ToastersSearchCell)tableView.VisibleCells.Where(c => c.Tag == this.ImageViewImages.IndexOf(logo)).FirstOrDefault();
                    var cell = (ToastersSearchCell)tableView.VisibleCells.Where(c => c.Tag == path.Row).FirstOrDefault();

                    if (cell != null)
                        cell._ProfilePic.Image = logo.Image;
                });
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Load more
        /// </summary>
        /// <param name="scrollView"></param>
        /// <param name="willDecelerate"></param>
        public async override void DraggingEnded(UIScrollView scrollView, bool willDecelerate)
        {
            if (this.Rows.Count == 0 || !this.Controller.loadMore)
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
            var row = this.Rows[indexPath.Row];
            var controller = this.Controller.Storyboard.InstantiateViewController("ToasterProfileController") as ToasterProfileController;
            controller.FromSearchedUser = true;
            controller.SearchedUser = new Shared.Models.Users.Users()
            {
                Email = row.Email,
                UserId = row.UserId,
                FirstName = row.Name,
                LastName = row.Name,
                Username = row.Username
            };

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
            return this.Rows.Count;
        }

        #endregion

    }
}