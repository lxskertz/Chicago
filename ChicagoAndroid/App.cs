using System;
using System.Collections.Generic;
using Android.App;
using Android.Runtime;
using Android.OS;
using Android.Preferences;
using Android.Content;
using Autofac;
using Plugin.CurrentActivity;
using Tabs.Mobile.Shared;
using Tabs.Mobile.Shared.Managers;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace Tabs.Mobile.ChicagoAndroid
{
    [Application]
    public class App : Application, Application.IActivityLifecycleCallbacks
    {

        #region Constants, Enums, and Variables

        private static int activityResumed;
        private static int activityPaused;
        private static int activityStarted;
        private static int activityStopped;
        private static Activity currentActivity;
        public static int localNotificationId = 0;

        public static AlarmManager reminderAlarm;
        public static PendingIntent pendingReminderServiceIntent;

        public static AlarmManager autoUpdateAlarm;
        public static PendingIntent autoUpdateServiceIntent;

        public static string PRIMARY_CHANNEL = "Tabs Primary Channel";
        public static string channelName = "Tabs Channel";

        public static string city = "Chicago";
        public static string zipCode = "60616";
        public static string state = "";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the account store manager
        /// </summary>
        public static Shared.Managers.Accounts.BusinessAccountFactory BusinessAccountFactory { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public static Shared.Managers.Businesses.BusinessFactory BusinessFactory { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public static Shared.Managers.Individuals.IndividualFactory IndividualFactory { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public static Shared.Managers.Users.UsersFactory UsersFactory { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public static VerificationCodeFactory VerificationCodeFactory { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public static Shared.Managers.Individuals.ToastersFactory ToastersFactory { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public static Shared.Managers.Businesses.AddressFactory AddressFactory { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public static Shared.Managers.Businesses.BusinessTypesFactory BusinessTypesFactory { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public static Shared.Managers.Events.BusinessEventsFactory BusinessEventsFactory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static Shared.Managers.Events.RsvpFactory RsvpFactory { get; set; }

        /// <summary>
        /// 
        /// </summary>
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

        public static NotificationRegisterFactory NotificationRegisterFactory { get; set; }

        public static Shared.Managers.Reports.Spams.ReportedSpamCheckInFactory ReportedSpamCheckInFactory { get; set; }

        public static Shared.Managers.Reports.InappropriateReports.InappropriateReportCheckInFactory InappropriateReportCheckInFactory { get; set; }

        public static Shared.Managers.Reports.Users.ReportedUserFactory ReportedUserFactory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static string PnsHandle { get; set; }

        #endregion

        #region Constructor

        public App(IntPtr h, JniHandleOwnership jho) : base(h, jho) { }

        #endregion

        #region Methods

        /// <summary>
        /// Called when any activity is created
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="savedInstanceState"></param>
        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        /// <summary>
        /// Called when any activity is destroyed
        /// </summary>
        /// <param name="activity"></param>
        public void OnActivityDestroyed(Activity activity) { }

        /// <summary>
        /// Called when any activity is paused
        /// </summary>
        /// <param name="activity"></param>
        public void OnActivityPaused(Activity activity)
        {
            ++activityPaused;
        }


        /// <summary>
        /// Called when any activity is resumed
        /// </summary>
        /// <param name="activity"></param>
        public void OnActivityResumed(Activity activity)
        {
            currentActivity = activity;
            ++activityResumed;
            CrossCurrentActivity.Current.Activity = activity;
        }


        /// <summary>
        /// Called when any activity is saved
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="outState"></param>
        public void OnActivitySaveInstanceState(Activity activity, Bundle outState) { }

        /// <summary>
        /// Called when any activity is started
        /// </summary>
        /// <param name="activity"></param>
        public void OnActivityStarted(Activity activity)
        {
            ++activityStarted;
            currentActivity = activity;
            CrossCurrentActivity.Current.Activity = activity;
        }


        /// <summary>
        /// Called when any activity is stopped
        /// </summary>
        /// <param name="activity"></param>
        public void OnActivityStopped(Activity activity)
        {
            ++activityStopped;
        }

        /// <summary>
        /// Determine if application is running in the foregrounf and visible to the user
        /// </summary>
        /// <returns></returns>
        public static bool ApplicationIsVisible()
        {
            return activityStarted > activityStopped;
        }

        /// <summary>
        /// Determine if application is running in the foreground but not visible
        /// </summary>
        /// <returns></returns>
        public static bool ApplicationinForeground()
        {
            return activityResumed >= activityPaused;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        /// <summary>
        ///  Override on create
        /// </summary>
        public override void OnCreate()
        {
            try
            {
                RegisterActivityLifecycleCallbacks(this);
                //CurrentPlatform.Init();
                AppStart.Init();
                using (var scope = AppStart.AutoFacContainer.BeginLifetimeScope())
                {
                    BusinessAccountFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Accounts.BusinessAccountFactory>();
                    BusinessFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Businesses.BusinessFactory>();
                    IndividualFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Individuals.IndividualFactory>();
                    UsersFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Users.UsersFactory>();
                    VerificationCodeFactory = AppStart.AutoFacContainer.Resolve<VerificationCodeFactory>();
                    ToastersFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Individuals.ToastersFactory>();
                    AddressFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Businesses.AddressFactory>();
                    BusinessTypesFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Businesses.BusinessTypesFactory>();
                    BusinessEventsFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Events.BusinessEventsFactory>();
                    RsvpFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Events.RsvpFactory>();
                    EventLikesFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Events.EventLikesFactory>();
                    CheckInFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.CheckIns.CheckInFactory>();
                    CheckInLikesFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.CheckIns.CheckInLikesFactory>();
                    BusinessDrinkFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Drinks.BusinessDrinkFactory>();
                    StripeCustomerInfoFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Payments.StripeCustomerInfoFactory>();
                    CustomerPaymentInfoFactory = AppStart.AutoFacContainer.Resolve<Shared.Interfaces.Payments.ICustomerPaymentInfoFactory>();
                    ToasterOrderFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Orders.ToasterOrderFactory>();
                    BusinessPhotoFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Businesses.BusinessPhotoFactory>();
                    ToasterPhotoFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Individuals.ToasterPhotoFactory>();
                    ToasterPointsFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Points.ToasterPointsFactory>();
                    SMSMessageFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Individuals.SMSMessageFactory>();
                    NotificationRegisterFactory = AppStart.AutoFacContainer.Resolve<NotificationRegisterFactory>();
                    ReportedSpamCheckInFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Reports.Spams.ReportedSpamCheckInFactory>();
                    InappropriateReportCheckInFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Reports.InappropriateReports.InappropriateReportCheckInFactory>();
                    ReportedUserFactory = AppStart.AutoFacContainer.Resolve<Shared.Managers.Reports.Users.ReportedUserFactory>();
                }

                Xamarin.Essentials.Platform.Init(this); // add this line to your code, it may also be called: bundle

                AppCenter.Start("a008809f-eaf6-4a07-95dd-98805980c201",
                   typeof(Analytics), typeof(Crashes));

                base.OnCreate();
            }
            catch (Exception) { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        //public static void Track(string property, string value)
        //{
            //HockeyApp.MetricsManager.TrackEvent("Custom Event",
            //    new Dictionary<string, string> { { property, value } },
            //    new Dictionary<string, double> { { "time", 1.0 } }
            //);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void RefreshApp(string message)
        {
            if (ApplicationinForeground() && ApplicationIsVisible())
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        private static bool IsHomeFragmentActive(Activity activity)
        {
            //Android.App.Fragment fragment = activity.FragmentManager.FindFragmentById(Resource.Id.fragmentContainer);
            //if (fragment != null)
            //{
            //    if (fragment is Fragment.HomeFragment)
            //    {
            //        return true;
            //    }
            //}

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        private static bool IsHomeActivityActive(Activity activity)
        {
            //if (activity is Activities.HomeActivity)
            //{
            //    return true;
            //}

            return false;
        }

        #endregion

    }
}