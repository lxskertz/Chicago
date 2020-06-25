using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Tabs.Mobile.Shared.Models.Individuals;
using Tabs.Mobile.Shared.Resources;
using Tabs.Mobile.Shared.Helpers;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.ChicagoAndroid.Activities.Individuals;
using Tabs.Mobile.ChicagoAndroid.ViewHolders.Individuals;
using Tabs.Mobile.ChicagoAndroid.Fragments.Individuals;

namespace Tabs.Mobile.ChicagoAndroid.Adapters.Individuals
{
    public class ToasterPhotoAdaper : RecyclerView.Adapter
    {

        #region Constants, Enums, and Variables

        public event EventHandler<int> ItemClick;
        private View itemView;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<ToasterPhoto> Rows { get; set; }

        /// <summary>
        /// Gets or sets the adapter context
        /// </summary>
        public SearchToasterProfileActivity MyContext { get; set; }

        public ToasterProfileFragment ToasterProfileFragment { get; set; }

        /// <summary>
        /// Gets or sets view holders
        /// </summary>
        public ToasterPhotoViewHolder ToasterPhotoViewHolder { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ImageViewImage> ImageViewImages { get; set; }

        #endregion

        #region Constructors

        public ToasterPhotoAdaper(SearchToasterProfileActivity context, List<ToasterPhoto> rows,
            List<ImageViewImage> imageViewImages)
        {
            this.Rows = rows;
            this.MyContext = context;
            this.ImageViewImages = imageViewImages;
        }

        public ToasterPhotoAdaper(ToasterProfileFragment context, List<ToasterPhoto> rows,
            List<ImageViewImage> imageViewImages)
        {
            this.Rows = rows;
            this.ToasterProfileFragment = context;
            this.ImageViewImages = imageViewImages;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Return the view type of the item at position for the purposes of view recycling.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override int GetItemViewType(int position)
        {
            return base.GetItemViewType(position);
        }

        /// <summary>
        /// Returns the total number of items in the data set hold by the adapter.
        /// </summary>
        public override int ItemCount
        {
            get
            {
                return Rows.Count;
            }
        }

        /// <summary>
        /// Create a new container/product CardView invoked by the layout manager
        /// Called when RecyclerView needs a new RecyclerView.ViewHolder of the given type to represent an item.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="viewType"></param>
        /// <returns></returns>
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ToasterPhotosListitem, parent, false);

            return new ToasterPhotoViewHolder(this, itemView, OnListItemClick);
        }

        /// <summary>
        /// Fill in the contents of the container/product card invoked by the layout manager
        /// Called by RecyclerView to display the data at the specified position.
        /// This method should update the contents of the ItemView to reflect the item at the given position.
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="position"></param>
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            this.ToasterPhotoViewHolder = holder as ToasterPhotoViewHolder;
            BindCardData(this.ToasterPhotoViewHolder, Rows.ElementAt(position));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewHolder"></param>
        /// <param name="data"></param>
        private void BindCardData(ToasterPhotoViewHolder viewHolder, ToasterPhoto photo)
        {
            var itemLogo = this.ImageViewImages.Where(x => x.Id == photo.ToasterPhotoId).FirstOrDefault();
            if (itemLogo != null && itemLogo.ImageBitmap == null)
            {

                if (this.MyContext != null)
                {
                    this.MyContext.BeginDownloadingImage(itemLogo, viewHolder.Photo);
                } else
                {
                    this.ToasterProfileFragment.HomeContext.BeginDownloadingImage(itemLogo, viewHolder.Photo);
                }
            }
            viewHolder.Photo.SetImageBitmap(itemLogo.ImageBitmap);

            if (itemLogo.ImageBitmap != null)
            {
                //itemLogo.ImageBitmap.Recycle();
                //itemLogo.ImageBitmap = null;
            }
        }

        /// <summary>
        /// Invoke the OnItemClick event with positional data..
        /// </summary>
        /// <param name="position"></param>
        private void OnItemClick(int position)
        {
            ItemClick?.Invoke(this, position);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnListItemClick(int position)
        {
            try
            {
                var item = this.Rows.ElementAt(position);
                var itemLogo = this.ImageViewImages.Where(x => x.Id == item.ToasterPhotoId).FirstOrDefault();

                if (itemLogo != null && itemLogo.ImageBitmap != null)
                {
                    if (this.MyContext != null)
                    {
                        Intent activity = new Intent(this.MyContext, typeof(MyImageActivity));
                        MyImageActivity.SelectedImage = itemLogo.ImageBitmap;
                        this.MyContext.StartActivity(activity);
                    }
                    else
                    {
                        Intent activity = new Intent(this.ToasterProfileFragment.HomeContext, typeof(MyImageActivity));
                        MyImageActivity.SelectedImage = itemLogo.ImageBitmap;
                        this.ToasterProfileFragment.HomeContext.StartActivity(activity);
                    }
                   
                }
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        #endregion


    }
}