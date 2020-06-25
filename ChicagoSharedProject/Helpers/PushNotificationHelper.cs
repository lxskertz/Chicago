using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Managers;
using Tabs.Mobile.Shared.Resources;

namespace Tabs.Mobile.Shared.Helpers
{
    public class PushNotificationHelper
    {
        public enum PushPlatform
        {
            Android,
            iOS
        }

        private NotificationRegisterFactory NotificationRegisterFactory { get; set; }

        public PushPlatform SelectedPushPlatform { get; set; }

        public PushNotificationHelper(NotificationRegisterFactory notificationRegisterFactory, PushPlatform selectedPushPlatform)
        {
            this.NotificationRegisterFactory = notificationRegisterFactory;
            this.SelectedPushPlatform = selectedPushPlatform;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public async Task ToasterRequestPush(string fromName, int toUserId)
        {
            var query = new NotificationQuery();
            query.PNS = this.SelectedPushPlatform == PushPlatform.Android ? DeviceRegistration.Fcm : DeviceRegistration.Apns;
            query.Message = fromName + NotificationTag.ToasterRequest;
            query.Tags = new List<string>();
            query.Tags.Add(NotificationTag.Toaster + toUserId);
            var a = await NotificationRegisterFactory.SendPush(query);
        }

        public async Task ToasterAddedPush(string fromName, int toUserId)
        {
            var query = new NotificationQuery();
            query.PNS = this.SelectedPushPlatform == PushPlatform.Android ? DeviceRegistration.Fcm : DeviceRegistration.Apns;
            query.Message = fromName + NotificationTag.AddedYou;
            query.Tags = new List<string>();
            query.Tags.Add(NotificationTag.Toaster + toUserId);
            var a = await NotificationRegisterFactory.SendPush(query);
        }

        public async Task ToasterAcceptedPush(string fromName, int toUserId)
        {
            var query = new NotificationQuery();
            query.PNS = this.SelectedPushPlatform == PushPlatform.Android ? DeviceRegistration.Fcm : DeviceRegistration.Apns;
            query.Message = fromName + NotificationTag.ToasterAccepted;
            query.Tags = new List<string>();
            query.Tags.Add(NotificationTag.Toaster + toUserId);
            var a = await NotificationRegisterFactory.SendPush(query);
        }

        public async Task SentDrinkPush(string fromName, int toUserId)
        {
            var query = new NotificationQuery();
            query.PNS = this.SelectedPushPlatform == PushPlatform.Android ? DeviceRegistration.Fcm : DeviceRegistration.Apns;
            query.Message = fromName + NotificationTag.SentDrink;
            query.Tags = new List<string>();
            query.Tags.Add(NotificationTag.Toaster + toUserId);
            var a = await NotificationRegisterFactory.SendPush(query);
        }

        public async Task DrinkPickedUpPush(int toUserId)
        {
            var query = new NotificationQuery();
            query.PNS = this.SelectedPushPlatform == PushPlatform.Android ? DeviceRegistration.Fcm : DeviceRegistration.Apns;
            query.Message = NotificationTag.DrinkPickedUp;
            query.Tags = new List<string>();
            query.Tags.Add(NotificationTag.Toaster + toUserId);
            var a = await NotificationRegisterFactory.SendPush(query);
        }

        public async Task LikedCheckInPush(string fromName, int toUserId)
        {
            var query = new NotificationQuery();
            query.PNS = this.SelectedPushPlatform == PushPlatform.Android ? DeviceRegistration.Fcm : DeviceRegistration.Apns;
            query.Message = fromName + NotificationTag.LikedCheckIn;
            query.Tags = new List<string>();
            query.Tags.Add(NotificationTag.Toaster + toUserId);
            var a = await NotificationRegisterFactory.SendPush(query);
        }

        public async Task NewPointsPush(int toUserId)
        {
            var query = new NotificationQuery();
            query.PNS = this.SelectedPushPlatform == PushPlatform.Android ? DeviceRegistration.Fcm : DeviceRegistration.Apns;
            query.Message = NotificationTag.Newpoints;
            query.Tags = new List<string>();
            query.Tags.Add(NotificationTag.Toaster + toUserId);
            var a = await NotificationRegisterFactory.SendPush(query);
        }

        public async Task SentDrinkChargePush(int toUserId, string message)
        {
            var query = new NotificationQuery();
            query.PNS = this.SelectedPushPlatform == PushPlatform.Android ? DeviceRegistration.Fcm : DeviceRegistration.Apns;
            query.Message = message;
            query.Tags = new List<string>();
            query.Tags.Add(NotificationTag.Toaster + toUserId);
            var a = await NotificationRegisterFactory.SendPush(query);
        }

    }
}
