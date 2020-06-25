using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Graphics;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using Firebase.Messaging;
using Android.Support.V4.App;
using Build = Android.OS.Build;

namespace Tabs.Mobile.ChicagoAndroid.MyNotificationsService
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {

        private const string TAG = "MyFirebaseMsgService";

        public override void OnMessageReceived(RemoteMessage message)
        {
            Log.Debug(TAG, "From: " + message.From);
            if (message.GetNotification() != null)
            {
                //These is how most messages will be received
                Log.Debug(TAG, "Notification Message Body: " + message.GetNotification().Body);
                SendNotification(message.GetNotification().Body);
            }
            else
            {
                //Only used for debugging payloads sent from the Azure portal
                SendNotification(message.Data.Values.First());

            }
        }

        private void SendNotification(string message)
        {
            var notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;

            // Create a new intent to show the notification in the UI. 
            var intent = new Intent(this, typeof(Activities.Individuals.IndividualHomeActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            PendingIntent contentIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            Notification.BigTextStyle textStyle = new Notification.BigTextStyle();
            textStyle.BigText(message);
            Notification.Builder builder;

            if ((int)Build.VERSION.SdkInt >= (int)BuildVersionCodes.O)
            {

                builder = new Notification.Builder(this, App.PRIMARY_CHANNEL);
                // Create the notification using the builder.
                builder.SetAutoCancel(true);
                builder.SetContentTitle("");
                builder.SetContentText(message);
                builder.SetSmallIcon(Resource.Mipmap.ic_app_icon_no_bgd);
                builder.SetContentIntent(contentIntent);
                builder.SetStyle(textStyle);
                builder.SetLargeIcon(BitmapFactory.DecodeResource(this.Resources, Resource.Mipmap.ic_app_icon_no_bgd));
                //builder.SetPriority((int)NotificationPriority.Default);
                builder.SetVisibility(NotificationVisibility.Private);
                builder.SetAutoCancel(true);
                // builder.SetDefaults(NotificationDefaults.Sound);

                builder.SetChannelId(App.PRIMARY_CHANNEL);
            }
            else
            {
                builder = new Notification.Builder(this);

                // Create the notification using the builder.
                builder.SetAutoCancel(true);
                builder.SetContentTitle("");
                builder.SetContentText(message);
                builder.SetSmallIcon(Resource.Mipmap.ic_app_icon_no_bgd);
                builder.SetContentIntent(contentIntent);
                builder.SetStyle(textStyle);
                builder.SetLargeIcon(BitmapFactory.DecodeResource(this.Resources, Resource.Mipmap.ic_app_icon_no_bgd));
                builder.SetPriority((int)NotificationPriority.Default);
                builder.SetVisibility(NotificationVisibility.Private);
                builder.SetAutoCancel(true);
                builder.SetDefaults(NotificationDefaults.Sound);
            }
            var notification = builder.Build();

            // Display the notification in the Notifications Area.
            //if (message.ToLower() != NotificationMessages.UpdatePlaceInLine.ToLower() || !message.ToLower().Contains("update"))
            //{
                ++App.localNotificationId;
                notificationManager.Notify(App.localNotificationId, notification);
            //}

            //App.RefreshApp(message);
        }

        //void SendNotification(string messageBody)
        //{
        //    var intent = new Intent(this, typeof(MainActivity));
        //    intent.AddFlags(ActivityFlags.ClearTop);
        //    var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

        //    var notificationBuilder = new NotificationCompat.Builder(this)
        //                .SetContentTitle("FCM Message")
        //                .SetSmallIcon(Resource.Drawable.ic_launcher)
        //                .SetContentText(messageBody)
        //                .SetAutoCancel(true)
        //                .SetShowWhen(false)
        //                .SetContentIntent(pendingIntent);

        //    if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        //    {
        //        notificationBuilder.SetChannelId(Activities.BaseActivity.CHANNEL_ID);
        //    }

        //    var notificationManager = NotificationManager.FromContext(this);

        //    notificationManager.Notify(0, notificationBuilder.Build());
        //}

    }
}