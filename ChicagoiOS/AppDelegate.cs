using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Foundation;
using UIKit;
using UserNotifications;
using Newtonsoft.Json;
using BigTed;
using WindowsAzure.Messaging;
using Tabs.Mobile.Shared.Models.Users;
using Tabs.Mobile.Shared.Models;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace Tabs.Mobile.ChicagoiOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {

        #region Constants, Enums, and Variables

        public const string environment = "Environment";
        public const string currentUserPref = "CurrentUser";
        public static string City = "Chicago";
        public static string ZipCode = "60616";
        public static Plugin.Geolocator.Abstractions.Address CurrentAddy;

        private static List<string> tags = new List<string>();
        private static DeviceRegistration deviceRegistration = new DeviceRegistration();

        #endregion

        #region Properties

        public override UIWindow Window { get; set; }

        public static NavController NavController { get; private set; }

		public static RootViewController RootViewController { get; set; }

        public static bool AppInBackground { get; set; }

        public static string ApiId { get; set; }

        public static string ApiKey { get; set; }

        public static Users CurrentUser { get; set; } = new Users();

        public static UIStoryboard MyStoryBoard { get; set; }

        public static Shared.Managers.Accounts.BusinessAccountFactory BusinessAccountFactory { get; set; }

        public static Shared.Managers.Businesses.BusinessFactory BusinessFactory { get; set; }

        public static Shared.Managers.Individuals.IndividualFactory IndividualFactory { get; set; }

        public static Shared.Managers.Users.UsersFactory UsersFactory { get; set; }

        public static Shared.Managers.VerificationCodeFactory VerificationCodeFactory { get; set; }

        public static Shared.Managers.Individuals.ToastersFactory ToastersFactory { get; set; }

        public static Shared.Managers.Businesses.AddressFactory AddressFactory { get; set; }

        public static Shared.Managers.Businesses.BusinessTypesFactory BusinessTypesFactory { get; set; }

        public static Shared.Managers.Events.BusinessEventsFactory BusinessEventsFactory { get; set; }

        public static Shared.Managers.Events.RsvpFactory RsvpFactory { get; set; }

        public static Shared.Managers.Events.EventLikesFactory EventLikesFactory { get; set; }

        public static Shared.Managers.CheckIns.CheckInFactory CheckInFactory { get; set; }

        public static Shared.Managers.CheckIns.CheckInLikesFactory CheckInLikesFactory { get; set; }

        public static Shared.Managers.Drinks.BusinessDrinkFactory BusinessDrinkFactory { get; set; }

        public static Shared.Managers.Payments.StripeCustomerInfoFactory StripeCustomerInfoFactory { get; set; }

        public static Shared.Interfaces.Payments.ICustomerPaymentInfoFactory CustomerPaymentInfoFactory { get; set; }

        public static Shared.Managers.Orders.ToasterOrderFactory ToasterOrderFactory { get; set; }

        public static Shared.Managers.Businesses.BusinessPhotoFactory BusinessPhotoFactory { get; set; }

        public static Shared.Managers.Individuals.ToasterPhotoFactory ToasterPhotoFactory { get; set; }

        public static Shared.Managers.Points.ToasterPointsFactory ToasterPointsFactory { get; set; }

        public static Shared.Managers.Individuals.SMSMessageFactory SMSMessageFactory { get; set; }

        public static Shared.Managers.Reports.Spams.ReportedSpamCheckInFactory ReportedSpamCheckInFactory { get; set; }

        public static Shared.Managers.Reports.InappropriateReports.InappropriateReportCheckInFactory InappropriateReportCheckInFactory { get; set; }

        public static Shared.Managers.NotificationRegisterFactory NotificationRegisterFactory { get; set; }

        public static Shared.Managers.Reports.Users.ReportedUserFactory ReportedUserFactory { get; set; }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="launchOptions"></param>
        /// <returns></returns>
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {

            MyStoryBoard = UIStoryboard.FromName("Main", null);
            RootViewController = MyStoryBoard.InstantiateViewController("RootViewController") as RootViewController;

            //NavController = new NavController(RootViewController);
            Window.RootViewController = new UINavigationController(RootViewController);
            Window.MakeKeyAndVisible();

            Shared.MyEnvironment.Environment = Shared.MyEnvironment.Environment == null ? GetEnvironment() : Shared.MyEnvironment.Environment;
            CurrentUser = GetCurrentUser();
            ApiId = CurrentUser != null ? CurrentUser.Identifier : string.Empty;
            ApiKey = CurrentUser != null ? CurrentUser.UserMobileSessionId : string.Empty;

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | 
                    UNAuthorizationOptions.Sound | UNAuthorizationOptions.Badge,
                    (granted, error) =>
                    {
                        if (granted)
                            InvokeOnMainThread(UIApplication.SharedApplication.RegisterForRemoteNotifications);
                    });
                UNUserNotificationCenter.Current.Delegate = new Delegates.UserNotificationCenterDelegate();

            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                        UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                        new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert |
                    UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }

            AppCenter.Start("98ae5104-6a83-40c5-a626-44454727c1a8",
                   typeof(Analytics), typeof(Crashes));

            return true;
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            //base.RegisteredForRemoteNotifications(application, deviceToken);

            string DeviceToken;
            // Get current device token
            if (UIDevice.CurrentDevice.CheckSystemVersion(12, 0))
            {
                byte[] bytes = deviceToken.ToArray<byte>();
                string[] hexArray = bytes.Select(b => b.ToString("x2")).ToArray();
                DeviceToken = string.Join(string.Empty, hexArray);
            }
            else
            {
                DeviceToken = deviceToken.Description;
                if (!string.IsNullOrWhiteSpace(DeviceToken))
                {
                    DeviceToken = DeviceToken.Trim('<').Trim('>').Replace(" ", "");
                }
            }

            // Get previous device token
            var oldDeviceToken = ApnsHandle();

            AddToAzureNotificationHub(oldDeviceToken, DeviceToken, false);

            // Save new device token
            SaveApnsHandle(DeviceToken);
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            ProcessNotification(userInfo, false);
        }

        private void ProcessNotification(NSDictionary userInfo, bool fromFinishedLaunching)
        {
            if (userInfo != null && userInfo.ContainsKey(new NSString("aps")))
            {
                NSDictionary aps = userInfo.ObjectForKey(new NSString("aps")) as NSDictionary;
                NSDictionary alert = null;
                if (aps != null)
                {
                    alert = aps.ObjectForKey(new NSString("alert")) as NSDictionary;
                }
                if (alert != null)
                {
                    var message = alert.ObjectForKey(new NSString("body"));
                    // Tell system to display the notification anyway or use
                    // `None` to say we have handled the display locally.

                    if (ObjCRuntime.Class.GetHandle("UIAlertController") == IntPtr.Zero)
                    {
                        var alertView = new UIAlertView("", message.ToString(), null, null, "OK");
                        //alertView.Clicked += 
                        alertView.Show();
                    }
                    else
                    {
                        var alertController = UIAlertController.Create("", message.ToString(), UIAlertControllerStyle.Alert);
                        alertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                        //var window = UIApplication.SharedApplication.KeyWindow.RootViewController;
                        //var vc = window.RootViewController;
                        UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alertController, true, null);
                    }
                    //RefreshApp(message.ToString());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="error"></param>
        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            //remove for production
            BTProgressHUD.ShowErrorWithStatus("Failed to register for remote notification");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldDeviceToken"></param>
        /// <param name="newToken"></param>
        public async static void AddToAzureNotificationHub(string oldDeviceToken, string newToken, bool fromLogin)
        {
            if (AppDelegate.CurrentUser != null)
            {
                // Has the token changed?
                if ((string.IsNullOrEmpty(oldDeviceToken) || !oldDeviceToken.Equals(newToken)) && !fromLogin)
                {
                    //TODO: Put your own logic here to notify your server that the device token has changed/been created!
                    deviceRegistration.Handle = newToken.Trim();
                    deviceRegistration.Platform = DeviceRegistration.Apns;
                    if (CurrentUser.IsBusiness)
                    {
                        tags.Add(Shared.Resources.NotificationTag.Business + CurrentUser.UserId);
                        tags.Add(Shared.Resources.NotificationTag.Businesses);
                    }
                    else
                    {
                        tags.Add(Shared.Resources.NotificationTag.Toaster + CurrentUser.UserId);
                        tags.Add(Shared.Resources.NotificationTag.Toaster);
                    }
                   
                    deviceRegistration.Tags = tags;
                    await RegisterDeviceAsync(deviceRegistration);
                }
                else if (fromLogin)
                {
                    //TODO: Put your own logic here to notify your server that the device token has changed/been created!
                    deviceRegistration.Handle = ApnsHandle();
                    if (!string.IsNullOrEmpty(deviceRegistration.Handle))
                    {
                        deviceRegistration.Platform = DeviceRegistration.Apns;
                        if (CurrentUser.IsBusiness)
                        {
                            tags.Add(Shared.Resources.NotificationTag.Business + CurrentUser.UserId);
                            tags.Add(Shared.Resources.NotificationTag.Businesses);
                        }
                        else
                        {
                            tags.Add(Shared.Resources.NotificationTag.Toaster + CurrentUser.UserId);
                            tags.Add(Shared.Resources.NotificationTag.Toaster);
                        }
                        deviceRegistration.Tags = tags;
                        await RegisterDeviceAsync(deviceRegistration);
                    }
                }
            }
        }

        /// <summary>
        /// Register device
        /// </summary>
        /// <param name="deviceRegistration"></param>
        /// <returns></returns>
        public static async Task RegisterDeviceAsync(DeviceRegistration deviceRegistration)
        {
            try
            {
                var regId = await RetrieveRegistrationIdOrRequestNewOneAsync(deviceRegistration.Handle);

                deviceRegistration.RegistrationId = regId;
                //deviceRegistration.Handle = ApnsHandle();
                var result = await UpdateRegistrationAsync(regId, deviceRegistration);

                if (result == null)
                {
                    DeleteDeviceRegistrationId();
                    regId = await RetrieveRegistrationIdOrRequestNewOneAsync(deviceRegistration.Handle);

                    deviceRegistration.RegistrationId = regId;
                    result = await UpdateRegistrationAsync(regId, deviceRegistration);
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                DeleteDeviceRegistrationId();
                //var regId = await RetrieveRegistrationIdOrRequestNewOneAsync(deviceRegistration.Handle);
                //deviceRegistration.RegistrationId = regId;
                //await UpdateRegistrationAsync(regId, deviceRegistration);
            }
        }

        /// <summary>
        /// Add or update new reg
        /// </summary>
        /// <param name="regId"></param>
        /// <param name="deviceRegistration"></param>
        /// <returns></returns>
        private static async Task<HttpResponseMessage> UpdateRegistrationAsync(string regId, DeviceRegistration deviceRegistration)
        {
            try
            {
                return await NotificationRegisterFactory.AppUpdateRegistration(deviceRegistration);
            } catch(Exception ex)
            {
                var a = ex;
            }

            return null;
        }

        /// <summary>
        /// Retrieve or register new id
        /// </summary>
        /// <returns></returns>
        private static async Task<string> RetrieveRegistrationIdOrRequestNewOneAsync(string handle)
        {
            var settings = DeviceRegistrationId();
            if (string.IsNullOrEmpty(settings))
            {
                var response = await NotificationRegisterFactory.CreateId(handle);
                SaveDeviceRegistrationId(response);

            }

            return DeviceRegistrationId();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string ApnsHandle()
        {
            string pnsHandle = NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken");

            return pnsHandle;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void DeleteApnsHandle()
        {
            NSUserDefaults.StandardUserDefaults.SetString("", "PushDeviceToken");
            NSUserDefaults.StandardUserDefaults.Init();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pnsHandle"></param>
		public static void SaveApnsHandle(string pnsHandle)
        {
            NSUserDefaults.StandardUserDefaults.SetString(pnsHandle, "PushDeviceToken");
            NSUserDefaults.StandardUserDefaults.Init();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string DeviceRegistrationId()
        {
            string pnsHandle = NSUserDefaults.StandardUserDefaults.StringForKey("DeviceRegistrationId");

            return pnsHandle;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void DeleteDeviceRegistrationId()
        {
            NSUserDefaults.StandardUserDefaults.SetString("", "DeviceRegistrationId");
            NSUserDefaults.StandardUserDefaults.Init();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceRegistrationId"></param>
		public static void SaveDeviceRegistrationId(string deviceRegistrationId)
        {
            NSUserDefaults.StandardUserDefaults.SetString(deviceRegistrationId, "DeviceRegistrationId");
            NSUserDefaults.StandardUserDefaults.Init();
        }

        /// <summary>
		/// Figure out if Device is connected to a network
		/// </summary>
		public static NetworkStatus DetermineNetworkConnection()
        {
            return Reachability.InternetConnectionStatus();
        }

        /// <summary>
        /// Determines if network is currently offline
        /// </summary>
        /// <returns><c>true</c> if is offline mode; otherwise, <c>false</c>.</returns>
        public static bool IsOfflineMode()
        {
            return AppDelegate.DetermineNetworkConnection() == NetworkStatus.NotReachable;
        }
   
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetEnvironment()
        {
            return NSUserDefaults.StandardUserDefaults.StringForKey(environment);
        }

        /// <summary>
        /// Delete current user
        /// </summary>
        public static void DeleteEnvironment()
        {
            NSUserDefaults.StandardUserDefaults.SetString(string.Empty, environment);
            NSUserDefaults.StandardUserDefaults.Init();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Environment"></param>
		public static void SaveEnvironment(string environmentvalue)
        {
            NSUserDefaults.StandardUserDefaults.SetString(environmentvalue, environment);
            NSUserDefaults.StandardUserDefaults.Init();
        }

        /// <summary>
        /// Get current user
        /// </summary>
        /// <returns></returns>
        public static Users GetCurrentUser()
        {
            string userJson = NSUserDefaults.StandardUserDefaults.StringForKey(currentUserPref);
            if (string.IsNullOrEmpty(userJson))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<Users>(NSUserDefaults.StandardUserDefaults.StringForKey(currentUserPref));
        }

        /// <summary>
        /// Delete current user
        /// </summary>
        public static void DeleteCurrentUser()
        {
            NSUserDefaults.StandardUserDefaults.SetString(string.Empty, currentUserPref);
            NSUserDefaults.StandardUserDefaults.Init();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void DeleteSettings()
        {
            DeleteCurrentUser();
            DeleteDeviceRegistrationId();
            DeleteApnsHandle();
            DeleteEnvironment();
        }


        /// <summary>
        /// Save current user
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="cuid"></param>
        public static void SaveCurrentUser(Users user)
        {
            string userJson = JsonConvert.SerializeObject(user);
            NSUserDefaults.StandardUserDefaults.SetString(userJson, currentUserPref);
            NSUserDefaults.StandardUserDefaults.Init();
        }


        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message)
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            AppInBackground = true;
        }

        public override void WillEnterForeground(UIApplication application)
        {
            AppInBackground = false;
        }

        public override void OnActivated(UIApplication application)
        {
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }
    }
}

