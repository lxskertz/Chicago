using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Tabs.Mobile.ChicagoAndroid.Activities.Individuals
{
    [Activity(Label = "", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class MyImageActivity : BaseActivity
    {
        public static Android.Graphics.Bitmap SelectedImage { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                this.SupportActionBar.SetDisplayShowHomeEnabled(true);
                SetContentView(Resource.Layout.MyImage);

                var _image = FindViewById<ImageView>(Resource.Id.photo);

                _image.SetImageBitmap(SelectedImage);
            }
            catch (Exception)
            {

            }

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    this.Finish();
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}