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
    public class ToastersDataSource : UITableViewSource
    {

        #region Constants, Enums, and Variables

        /// <summary>
        /// Gets or sets the cell
        /// </summary>
        private NSString ToastersCell = new NSString("ToastersCell"); 

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        public ToastersController Controller { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Toasters> Rows { get; set; } = new List<Toasters>();

        public List<ImageViewImage> ImageViewImages { get; set; }

        #endregion

        #region Constructors

        public ToastersDataSource(ToastersController controller, List<Toasters> rows,
            List<ImageViewImage> imageViewImages)
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
        public async void AddRowItems(List<Toasters> rows)
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
        public async Task GetPicUris(List<Toasters> rows)
        {
            foreach (var b in rows)
            {
                ImageViewImage itemLogo = new ImageViewImage();
                var userId = b.UserOneId == AppDelegate.CurrentUser.UserId ? b.UserTwoId : b.UserOneId;
                itemLogo.Id = userId;

                var uriString = await Shared.Helpers.BlobStorageHelper.GetToasterBlobUri(userId);
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
            var cell = (ToastersCell)tableView.DequeueReusableCell(this.ToastersCell);
            cell.Name.Text = !string.IsNullOrEmpty(row.FirstName) ? row.FirstName : string.Empty;
            cell.Username.Text = !string.IsNullOrEmpty(row.Username) ? row.Username : string.Empty;
            this.Controller.MakeImageViewRound(cell.ProfilePic);
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
                cell.ProfilePic.Image = itemLogo.Image;
            } else
            {
                cell.ProfilePic.Image = null;
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
                    //var cell = (ToastersCell)tableView.VisibleCells.Where(c => c.Tag == this.ImageViewImages.IndexOf(logo)).FirstOrDefault();
                    var cell = (ToastersCell)tableView.VisibleCells.Where(c => c.Tag == path.Row).FirstOrDefault();

                    if (cell != null)
                        cell.ProfilePic.Image = logo.Image;
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
            controller.FromRequestPending = this.Controller.pendingRequestShown;
            controller.Toaster = row;
            controller.SearchedUser = new Shared.Models.Users.Users()
            {
                Email = row.Email,
                UserId = row.UserId,
                FirstName = row.FirstName,
                LastName = row.LastName,
                Username = row.Username,
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