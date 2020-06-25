using System;

namespace Tabs.Mobile.Shared.Models
{
    public class ImageViewImage 
    {

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Uri ImageUrl { get; set; }

#if __IOS__

        /// <summary>
        /// 
        /// </summary>
        public UIKit.UIImage Image { get; set; }

#endif

#if __ANDROID__

        /// <summary>
        /// 
        /// </summary>
        public Android.Graphics.Bitmap ImageBitmap { get; set; }

#endif

    }
}
