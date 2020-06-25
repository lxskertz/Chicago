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
    [Register ("PrivacyPolicyController")]
    partial class PrivacyPolicyController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIWebView Webview { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Webview != null) {
                Webview.Dispose ();
                Webview = null;
            }
        }
    }
}