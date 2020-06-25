using Foundation;
using System;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS
{
    public partial class MyImageViewController : BaseViewController
    {
        public UIImage SelectedImage { get; set; }

        public MyImageViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
                _Image.Image = SelectedImage;
            }
            catch (Exception)
            {

            }
        }
    }
}