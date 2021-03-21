using System;
//using AndroidX.Preference;
using Android.Preferences;
using Android.Views;
using Android.Widget;
using Android.Net;
using Android.Content;

namespace Tabs.Mobile.ChicagoAndroid
{
    public class MyPreferences
    {

        Activities.BaseActivity MyContext { get; set; }

        public MyPreferences(Activities.BaseActivity baseActivity)
        {
            this.MyContext = baseActivity;
        }

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="environment"></param>
        public void SaveNotificationRegId(string id)
        {
            DeleteNotificationRegId();
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this.MyContext);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutString(Activities.BaseActivity.NotificationRegId, id);
            editor.Apply();
        }

        /// <summary>
        /// 
        /// </summary>
        public void DeleteNotificationRegId()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this.MyContext);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.Remove(Activities.BaseActivity.NotificationRegId);
            editor.Apply();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetNotificationRegId()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this.MyContext);

            return prefs.GetString(Activities.BaseActivity.NotificationRegId, "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="environment"></param>
        public void SavePnsHandle(string id)
        {
            DeletePnsHandle();
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this.MyContext);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutString(Activities.BaseActivity.PnsHandle, id);
            editor.Apply();
        }

        /// <summary>
        /// 
        /// </summary>
        public void DeletePnsHandle()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this.MyContext);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.Remove(Activities.BaseActivity.PnsHandle);
            editor.Apply();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetPnsHandle()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this.MyContext);

            return prefs.GetString(Activities.BaseActivity.PnsHandle, "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="environment"></param>
        public void SavePnsHandleUpdated(bool value)
        {
            //DeletePnsHandleUpdated();
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this.MyContext);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutBoolean(Activities.BaseActivity.PnsHandleRefreshed, value);
            editor.Apply();
        }

        /// <summary>
        /// 
        /// </summary>
        //public void DeletePnsHandleUpdated()
        //{
        //    ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this.MyContext);
        //    ISharedPreferencesEditor editor = prefs.Edit();
        //    editor.Remove(Activities.BaseActivity.PnsHandleRefreshed);
        //    editor.Apply();
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsPnsHandleUpdated()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this.MyContext);

            return prefs.GetBoolean(Activities.BaseActivity.PnsHandleRefreshed, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="environment"></param>
        public void SaveEnvironment(string value)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this.MyContext);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutString(Activities.BaseActivity.environment, value);
            editor.Apply();
        }

        /// <summary>
        /// 
        /// </summary>
        public void DeleteENVIRONMENT()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this.MyContext);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.Remove(Activities.BaseActivity.environment);
            editor.Apply();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetEnvironment()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this.MyContext);

            return prefs.GetString(Activities.BaseActivity.environment, "");
        }
        
        #endregion

    }
}