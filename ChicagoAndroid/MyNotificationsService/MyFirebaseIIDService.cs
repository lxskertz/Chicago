//using System;
//using Android.App;
//using Android.Content;
//using Android.Util;
//using Firebase.Iid;

//namespace Tabs.Mobile.ChicagoAndroid.MyNotificationsService
//{
//    [Service]
//    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
//    public class MyFirebaseIIDService : FirebaseInstanceIdService
//    {
//        private const string TAG = "MyFirebaseIIDService";
//        //private NotificationHub hub;

//        public override void OnTokenRefresh()
//        {
//            try
//            {
//                var refreshedToken = FirebaseInstanceId.Instance.Token;
//                //Log.Debug(TAG, "FCM token: " + refreshedToken);
//                Activities.BaseActivity.CurrentActivity.MyPreferences.SavePnsHandle(refreshedToken);
//                Activities.BaseActivity.CurrentActivity.MyPreferences.SavePnsHandleUpdated(true);
//            }
//            catch (Exception)
//            {
//            }
//        }


//        //void SendRegistrationToServer(string token)
//        //{
//        //    // Register with Notification Hubs
//        //    hub = new NotificationHub(Constants.NotificationHubName,
//        //                                Constants.ListenConnectionString, this);

//        //    var tags = new List<string>() { };
//        //    var regID = hub.Register(token, tags.ToArray()).RegistrationId;

//        //    Log.Debug(TAG, $"Successful registration of ID {regID}");
//        //}
//    }
//}