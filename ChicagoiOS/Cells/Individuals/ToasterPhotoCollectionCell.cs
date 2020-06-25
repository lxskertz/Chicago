using Foundation;
using System;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class ToasterPhotoCollectionCell : UICollectionViewCell
    {

        public UIImageView _Photo
        {
            get
            {
                return Photo;
            }
        }

        public ToasterPhotoCollectionCell (IntPtr handle) : base (handle)
        {
        }
    }
}