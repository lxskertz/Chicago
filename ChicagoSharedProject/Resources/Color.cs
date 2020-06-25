using System;

#if __IOS__
using UIKit;
using CoreGraphics;
#endif

namespace Tabs.Mobile.Shared.Resources
{

    public struct Color
    {
        public static readonly Color Purple = 0xB455B6;
        public static readonly Color Blue = 0x1E88E5;
        public static readonly Color DarkBlue = 0x2C3E50;
        public static readonly Color Green = 0x477501;
        public static readonly Color Gray = 0xAAAAAA;
        public static readonly Color LightGray = 0xDFDFDF;
        public static readonly Color DarkGray = 0x666666;
        public static readonly Color PrimaryBlue = 0x095ca4;
        public static readonly Color Orange = 0xFE6601;
        public static readonly Color BackgroundColor = 0xf7f7f5;
        public static readonly Color NearBlack = 0x222222;
        public static readonly Color Red = 0XDD0000;
        public static readonly Color ToolbarColor = 0Xf7f7f5;
        public static readonly Color NavBarSelectedText = 0x91c8f4;
        public static readonly Color NavBarUnSelectedText = 0x212121;
        public static readonly Color ConnectionTooolbarOnline = 0x33992e;
        public static readonly Color ConnectionToolbarOffline = 0x1e1e1e;
        public static readonly Color SearchHintColor = 0x959595;
        public static readonly Color TextHeaderColor = 0x91c8f4;
        public static readonly Color ParagraphColor = 0x494a5c;
        public static readonly Color NavigationBarColor = 0x292f66;


        public double R, G, B;

        public static Color FromHex(int hex)
        {
            Func<int, int> at = offset => (hex >> offset) & 0xFF;
            return new Color
            {
                R = at(16) / 255.0,
                G = at(8) / 255.0,
                B = at(0) / 255.0
            };
        }

        public static implicit operator Color(int hex)
        {
            return FromHex(hex);
        }

#if __IOS__
		public UIColor ToUIColor () {
			return UIColor.FromRGB ((float)R, (float)G, (float)B);
		}

		public static implicit operator UIColor (Color color) {
			return color.ToUIColor ();
		}

		public static implicit operator CGColor (Color color) {
			return color.ToUIColor ().CGColor;
		}
#endif

#if __ANDROID__
        public Android.Graphics.Color ToAndroidColor()
        {
            return Android.Graphics.Color.Rgb((int)(255 * R), (int)(255 * G), (int)(255 * B));
        }

        public static implicit operator Android.Graphics.Color(Color color)
        {
            return color.ToAndroidColor();
        }
#endif
    }
}
