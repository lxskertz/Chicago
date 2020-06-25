using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Tabs.Mobile.Shared.WebServices
{
    public class BaseService
    {

        #region Constants, Enums, and Variables

        //private const int Timeout = 1000000;
        //private const int ReadWriteTimeout = 1000000;
        //private const string UserAgent = "Tabs App";
        //private static Encoding Encoding = Encoding.UTF8;

        #endregion

        #region Properties

        //public static HttpClient client { get; set; } = new HttpClient();

        public ServiceClient ServiceClient { get; set; } = new ServiceClient();

        /// <summary>
        /// Gets or Sets current username
        /// </summary>
        public string Username { get; set; }

        #endregion

        #region Constructors

        public BaseService()
        {
            //this.ServiceClient.APIID = ApiId;
            //this.ServiceClient.APIKey = ApiKey;
         
            //client.BaseAddress = new Uri("http://localhost:55321/");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //Uri baseUri = new Uri(baseUrl);
            //HttpClientHandler httpClientHandler = new HttpClientHandler();
            //client = new HttpClient(httpClientHandler);
            //client.BaseAddress = baseUri;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public string DetermineEnvironmentandUsername(string username)
        {
            //DetermineEnvironment(MyEnvironment.DevelopmentURL);
            //return this.Username;

            if (username.Contains("@@dev|"))
            {
                this.Username = username.Remove(0, 6);
                DetermineEnvironment(MyEnvironment.DevelopmentURL);

                if (this.Username.Contains("|"))
                {
                    var index = this.Username.IndexOf('|');
                    this.Username = this.Username.Remove(0, index + 1);

                    return this.Username;
                }

                return this.Username;
            } else if (username.Contains("@@staging|"))
            {
                this.Username = username.Remove(0, 10);
                DetermineEnvironment(MyEnvironment.StagingURL);

                if (this.Username.Contains("|"))
                {
                    var index = this.Username.IndexOf('|');
                    this.Username = this.Username.Remove(0, index + 1);

                    return this.Username;
                }

                return this.Username;
            }
            else
            {
                DetermineEnvironment(MyEnvironment.ProductionURL);
                this.Username = username;

                if (this.Username.Contains("|"))
                {
                    var index = this.Username.IndexOf('|');
                    this.Username = this.Username.Remove(0, index + 1);

                    return this.Username;

                }

                return this.Username;
            }
        }

        /// <summary>
        /// Get environment
        /// </summary>
        /// <param name="environment"></param>
        /// <returns></returns>
        public string DetermineEnvironment(string environment)
        {
            switch (environment)
            {
                case MyEnvironment.DevelopmentURL:
                    return MyEnvironment.Environment = MyEnvironment.DevelopmentURL;
                case MyEnvironment.StagingURL:
                    return MyEnvironment.Environment = MyEnvironment.StagingURL;
                default:
                    return MyEnvironment.Environment = MyEnvironment.ProductionURL;
            }
        }

        #endregion

    }
}
