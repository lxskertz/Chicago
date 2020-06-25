using System;
using System.Net;
using Foundation;
using SystemConfiguration;
using CoreFoundation;
using UIKit;

namespace Tabs.Mobile.ChicagoiOS
{

    //based on https://github.com/xamarin/monotouch-samples/blob/master/ReachabilitySample/reachability.cs

    public enum NetworkStatus
    {
        NotReachable,
        ReachableViaCarrierDataNetwork,
        ReachableViaWiFiNetwork
    }

    public static class Reachability
    {

        /// <summary>
        /// The host name
        /// </summary>
        public static string HostName = "www.google.com";

        //
        // Returns true if it is possible to reach the AdHoc WiFi network
        // and optionally provides extra network reachability flags as the
        // out parameter
        //
        static NetworkReachability adHocWiFiNetworkReachability;

        /// <summary>
        /// The default route reachability
        /// </summary>
        static NetworkReachability defaultRouteReachability;

        /// <summary>
        /// The remote host reachability
        /// </summary>
        static NetworkReachability remoteHostReachability;

        //
        // Raised every time there is an interesting reachable event,
        // we do not even pass the info as to what changed, and
        // we lump all three status we probe into one
        //
        public static event EventHandler ReachabilityChanged;

        /// <summary>
        /// Whether the last network status was offline
        /// </summary>
        public static bool PreviouslyOffline;

        #region Methods

        /// <summary>
        /// Internets the connection status.
        /// </summary>
        /// <returns></returns>
        public static NetworkStatus InternetConnectionStatus()
        {
            NetworkReachabilityFlags flags;
            bool defaultNetworkAvailable = IsNetworkAvailable(out flags);
            if (defaultNetworkAvailable && ((flags & NetworkReachabilityFlags.IsDirect) != 0))
            {
                return NetworkStatus.NotReachable;
            }
            else if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
                return NetworkStatus.ReachableViaCarrierDataNetwork;
            else if (flags == 0 || flags.HasFlag(NetworkReachabilityFlags.ConnectionRequired))
                return NetworkStatus.NotReachable;
            return NetworkStatus.ReachableViaWiFiNetwork;
        }

        /// <summary>
        /// Determines whether [is ad hoc wi fi network available] [the specified flags].
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <returns></returns>
        public static bool IsAdHocWiFiNetworkAvailable(out NetworkReachabilityFlags flags)
        {
            if (adHocWiFiNetworkReachability == null)
            {
                adHocWiFiNetworkReachability = new NetworkReachability(new IPAddress(new byte[] { 169, 254, 0, 0 }));
                adHocWiFiNetworkReachability.SetNotification(OnChange);
                adHocWiFiNetworkReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
            }
            return adHocWiFiNetworkReachability.TryGetFlags(out flags) && IsReachableWithoutRequiringConnection(flags);
        }

        // Is the host reachable with the current network configuration
        public static bool IsHostReachable(string host)
        {
            if (string.IsNullOrEmpty(host))
                return false;
            using (var r = new NetworkReachability(host))
            {
                NetworkReachabilityFlags flags;
                if (r.TryGetFlags(out flags))
                {
                    return IsReachableWithoutRequiringConnection(flags);
                }
            }
            return false;
        }

        /// <summary>
        /// Determines whether [is reachable without requiring connection] [the specified flags].
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <returns></returns>
        public static bool IsReachableWithoutRequiringConnection(NetworkReachabilityFlags flags)
        {
            // Is it reachable with the current network configuration?
            bool isReachable = (flags & NetworkReachabilityFlags.Reachable) != 0;
            // Do we need a connection to reach it?
            bool noConnectionRequired = (flags & NetworkReachabilityFlags.ConnectionRequired) == 0;
            // Since the network stack will automatically try to get the WAN up,
            // probe that
            if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
                noConnectionRequired = true;
            return isReachable && noConnectionRequired;
        }

        public static NetworkStatus LocalWifiConnectionStatus()
        {
            NetworkReachabilityFlags flags;
            if (IsAdHocWiFiNetworkAvailable(out flags))
            {
                if ((flags & NetworkReachabilityFlags.IsDirect) != 0)
                    return NetworkStatus.ReachableViaWiFiNetwork;
            }
            return NetworkStatus.NotReachable;
        }

        /// <summary>
        /// Remotes the host status.
        /// </summary>
        /// <returns></returns>
        public static NetworkStatus RemoteHostStatus()
        {
            NetworkReachabilityFlags flags;
            bool reachable;
            if (remoteHostReachability == null)
            {
                remoteHostReachability = new NetworkReachability(HostName);
                // Need to probe before we queue, or we wont get any meaningful values
                // this only happens when you create NetworkReachability from a hostname
                reachable = remoteHostReachability.TryGetFlags(out flags);
                remoteHostReachability.SetNotification(OnChange);
                remoteHostReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
            }
            else
                reachable = remoteHostReachability.TryGetFlags(out flags);
            if (!reachable)
                return NetworkStatus.NotReachable;
            if (!IsReachableWithoutRequiringConnection(flags))
                return NetworkStatus.NotReachable;
            if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
                return NetworkStatus.ReachableViaCarrierDataNetwork;
            return NetworkStatus.ReachableViaWiFiNetwork;
        }

        /// <summary>
        /// Determines whether [is network available] [the specified flags].
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <returns></returns>
        static bool IsNetworkAvailable(out NetworkReachabilityFlags flags)
        {
            if (defaultRouteReachability == null)
            {
                defaultRouteReachability = new NetworkReachability(new IPAddress(0));
                defaultRouteReachability.SetNotification(OnChange);
                //defaultRouteReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
                defaultRouteReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
            }
            return defaultRouteReachability.TryGetFlags(out flags) && IsReachableWithoutRequiringConnection(flags);
        }

        /// <summary>
        /// Called when [change].
        /// </summary>
        /// <param name="flags">The flags.</param>
        static void OnChange(NetworkReachabilityFlags flags)
        {
            // We only care about network access change, not when switching from wifi to carrier and vice versa
            if (!AppDelegate.IsOfflineMode() && !PreviouslyOffline)
            {
                return;
            }
            PreviouslyOffline = AppDelegate.IsOfflineMode();

            var h = ReachabilityChanged;
            if (h != null)
                h(null, EventArgs.Empty);
        }

        #endregion

    }

}