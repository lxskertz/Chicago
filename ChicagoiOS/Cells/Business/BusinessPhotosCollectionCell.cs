using Foundation;
using System;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class BusinessPhotosCollectionCell : UICollectionViewCell
    {

        public UIImageView _Photo
        {
            get
            {
                return Photo;
            }
        }

        public BusinessPhotosCollectionCell (IntPtr handle) : base (handle)
        {
        }
    }
}