namespace Tabs.Mobile.Shared
{
    public class MyEnvironment
    {

        #region Constants, Enums, and Variables

        public const string DevelopmentURL = "https://chitownapi-dev.azurewebsites.net/v1/";
        public const string ProductionURL = "https://chitownapi.azurewebsites.net/v1/";
        public const string StagingURL = "https://chitownapi-staging.azurewebsites.net/v1/"; 

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current enviroment
        /// </summary>
        public static string CurrentEnvironment
        {
            get
            {
                return Environment;
            }
        }

        /// <summary>
        /// Gets or sets the environment
        /// </summary>
        public static string Environment { get; set; }

        /// <summary>
        /// Gets or sets the User Agent
        /// </summary>
        public static string UserAgent { get; set; }

        #endregion

    }
}
