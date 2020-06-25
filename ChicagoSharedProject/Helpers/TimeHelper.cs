using System;
using System.Collections.Generic;
using System.Text;

namespace Tabs.Mobile.Shared.Helpers
{
    public static class TimeHelper
    {

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool ValidStartDate(DateTime date, bool edit)
        {
            if (edit)
            {
                return true;
            }
            return date.Date >= DateTime.Now.Date;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static bool ValidEndDate(DateTime startDate, DateTime endDate)
        {
            return endDate.Date >= startDate.Date;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool ValidStartTime(DateTime time, bool edit)
        {
            if (edit)
            {
                return true;
            }
            return time.TimeOfDay > DateTime.Now.TimeOfDay;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static bool ValidEndTime(DateTime startTime, DateTime endTime)
        {
            return true; //endTime.TimeOfDay > startTime.TimeOfDay;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hourIn24"></param>
        /// <returns></returns>
        public static int GetPMTime(int hourIn24)
        {
            switch (hourIn24)
            {
                case 13:
                    return 1;
                case 14:
                    return 2;
                case 15:
                    return 3;
                case 16:
                    return 4;
                case 17:
                    return 5;
                case 18:
                    return 6;
                case 19:
                    return 7;
                case 20:
                    return 8;
                case 21:
                    return 9;
                case 22:
                    return 10;
                case 23:
                    return 11;
                case 24:
                    return 12;
                default:
                    return hourIn24;
            }
        }

        #endregion

    }
}
