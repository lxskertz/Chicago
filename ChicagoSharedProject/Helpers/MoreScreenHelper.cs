using System;
using System.Collections.Generic;
using System.Text;

namespace Tabs.Mobile.Shared.Helpers
{
    public static class MoreScreenHelper
    {

        #region Constants, Enums, and Variables

        public const string Payment = "Payment Methods";
        public const string Points = "Points";
        public const string Settings = "Settings";
        public const string Logout = "Logout";
        public const string Orders = "Orders";
        public const string LiveCheckIns = "Live Check-Ins";
        public const string DiscountCodes = "Discount Codes";
        public const string PostReports = "Reports"; 

        #endregion

        #region Methods

        public static List<string> GetIndividualTableRows()
        {
            List<string> rows = new List<string>();
            rows.Add(Points);
            //rows.Add(Settings);
            //rows.Add(Orders);
            rows.Add(Payment);
            rows.Add(Logout);

            return rows;
        }

        public static List<string> GetBusinessTableRows()
        {
            List<string> rows = new List<string>();
            //rows.Add(LiveCheckIns);
            //rows.Add(Payment);
            //rows.Add(DiscountCodes);
            rows.Add(Logout);

            return rows;
        }

        #endregion

    }
}
