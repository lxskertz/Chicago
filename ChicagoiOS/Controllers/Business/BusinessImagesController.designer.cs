// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS
{
    [Register ("BusinessImagesController")]
    partial class BusinessImagesController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Tabs.Mobile.ChicagoiOS.BusinessPhotoCollection PhotosCollectionView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (PhotosCollectionView != null) {
                PhotosCollectionView.Dispose ();
                PhotosCollectionView = null;
            }
        }
    }
}