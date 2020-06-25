using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserNotifications;
using Foundation;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS.Delegates
{
    public class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        #region Constructors

        public UserNotificationCenterDelegate() { }

        #endregion

        #region Override Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="center"></param>
        /// <param name="notification"></param>
        /// <param name="completionHandler"></param>
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            var message = notification.Request.Content.Body;
            completionHandler(UNNotificationPresentationOptions.Alert);
        }

        #endregion
    }

}