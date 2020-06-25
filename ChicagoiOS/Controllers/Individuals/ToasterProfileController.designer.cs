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
    [Register ("ToasterProfileController")]
    partial class ToasterProfileController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Tabs.Mobile.ChicagoiOS.ToasterPhotoCollection ToasterPhotoCollectionView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ToasterPhotoCollectionView != null) {
                ToasterPhotoCollectionView.Dispose ();
                ToasterPhotoCollectionView = null;
            }
        }
    }
}