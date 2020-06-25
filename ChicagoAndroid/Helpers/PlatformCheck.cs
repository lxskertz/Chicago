using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;       
using Android.Content.PM;
using Android.Support.V4.App;
using Android.Support.V4.Content;

namespace Tabs.Mobile.ChicagoAndroid.Helpers
{
    public class PlatformChecks
    {

        #region Constants, Enums, and Variables

        //public const string PERMISSION_CAMERA = "android.permission.CAMERA";
        //public const string PERMISSION_FLASHLIGHT = "android.permission.FLASHLIGHT";
        public const string PERMISSION_WRITE_EXTERNAL_STORAGE = "android.permission.WRITE_EXTERNAL_STORAGE";
        public const string PERMISSION_READ_EXTERNAL_STORAGE = "android.permission.READ_EXTERNAL_STORAGE";

        #endregion

        #region Methods

        /// <summary>
        /// Check if permission is in manifest
        /// </summary>
        /// <param name="context"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        public static bool IsPermissionInManifest(Context context, string permission)
        {
            PermissionInfo pi = null;

            try
            {
                pi = context.PackageManager.GetPermissionInfo(permission, PackageInfoFlags.Permissions);
            }
            catch { }

            if (pi == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check if the permission is granted
        /// </summary>
        /// <param name="context"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        public static bool IsPermissionGranted(Context context, string permission)
        {
            return ContextCompat.CheckSelfPermission(context, permission) == Permission.Granted;
        }

        /// <summary>
        /// Request permission
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="permissions"></param>
        /// <param name="requestCode"></param>
        /// <returns></returns>
        public static bool RequestPermissions(Activity activity, string[] permissions, int requestCode)
        {
            var permissionsToRequest = new List<string>();
            foreach (var permission in permissions)
            {
                if (ContextCompat.CheckSelfPermission(activity, permission) != Permission.Granted)
                    permissionsToRequest.Add(permission);
            }
            if (permissionsToRequest.Any())
            {
                ActivityCompat.RequestPermissions(activity, permissionsToRequest.ToArray(), requestCode);
                return true;
            }

            return false;
        }

        #endregion

    }
}

