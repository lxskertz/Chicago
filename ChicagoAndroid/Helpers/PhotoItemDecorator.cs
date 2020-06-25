using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Android.Graphics;

namespace Tabs.Mobile.ChicagoAndroid.Helpers
{
    public class PhotoItemDecorator : RecyclerView.ItemDecoration
    {
        #region Properties

        public int Spacing { get; set; }

        #endregion

        #region Constructors

        public PhotoItemDecorator(int spacing)
        {
            this.Spacing = spacing;
        }

        #endregion

        #region Methods

        public override void GetItemOffsets(Rect outRect, View view, RecyclerView parent, RecyclerView.State state)
        {
            base.GetItemOffsets(outRect, view, parent, state);
            outRect.Set(Spacing, Spacing, Spacing, Spacing);
        }

        #endregion
    }
}