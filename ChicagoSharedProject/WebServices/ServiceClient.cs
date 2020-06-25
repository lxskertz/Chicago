using System;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Tabs.Mobile.Shared.WebServices
{
    public class ServiceClient
    {

        #region Constants, Enums, and Variables

        private const int Timeout = 1000000;
        private const int ReadWriteTimeout = 1000000;
        private const string UserAgent = "TabsMobileApp";
        private static Encoding Encoding = Encoding.UTF8;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Authentication Token for service calls.
        /// </summary>
        public static string AuthenticationToken { get; set; }

        /// <summary>
        /// Return Api ID
        /// </summary>
        public string APIID
        {
            get
            {
#if __ANDROID__
                return ChicagoAndroid.Activities.BaseActivity.ApiId;
#endif
#if __IOS__
                return ChicagoiOS.AppDelegate.CurrentUser.Identifier;

#endif
            }
        }

        /// <summary>
        /// Return Api Key
        /// </summary>
        public string APIKey
        {
            get
            {
#if __ANDROID__
                return ChicagoAndroid.Activities.BaseActivity.ApiKey;
#endif
#if __IOS__
                return ChicagoiOS.AppDelegate.CurrentUser.UserMobileSessionId;

#endif
            }
        }



        #endregion

        #region Methods

        /// <summary>
        /// Cert Validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        private static bool CertificateValidationCallBack(
         object sender,
         X509Certificate certificate,
         X509Chain chain,
         SslPolicyErrors sslPolicyErrors)
        {
            // If the certificate is a valid, signed certificate, return true.
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }

            // If there are errors in the certificate chain, look at each error to determine the cause.
            if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateChainErrors) != 0)
            {
                if (chain != null && chain.ChainStatus != null)
                {
                    foreach (X509ChainStatus status in chain.ChainStatus)
                    {
                        if ((certificate.Subject == certificate.Issuer) &&
                           (status.Status == X509ChainStatusFlags.UntrustedRoot))
                        {
                            // Self-signed certificates with an untrusted root are valid. 
                            continue;
                        }
                        else
                        {
                            if (status.Status != X509ChainStatusFlags.NoError)
                            {
                                // If there are any other errors in the certificate chain, the certificate is invalid,
                                // so the method returns false.
                                return false;
                            }
                        }
                    }
                }

                // When processing reaches this line, the only errors in the certificate chain are 
                // untrusted root errors for self-signed certificates. These certificates are valid
                // for default Exchange server installations, so return true.
                return true;
            }
            else
            {
                // In all other cases, return false.
                return false;
            }
        }

        /// <summary>
        /// Gets response as string
        /// </summary>
        /// <param name="response"></param>
        /// <returns> string </returns>
        /// </summary>
        public string GetResponseAsString<T>(WebResponse response)
        {
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding))
            {

                return streamReader.ReadToEnd();
            }
        }

        /// <summary>
        /// Calls the MSDSonline API JSON data returning JSON data of a specified type.
        /// </summary>
        /// <typeparam name="T"> type of object to retrieve </typeparam>
        /// <param name="url"> path of the method we want to call </param>
        /// <param name="parameters"> object in json format </param>
        /// <param name="includeToken"> if this call should include the login token </param>
        /// <param name="method"> type of http method to use </param>
        /// <returns></returns>
        /// </summary>
        /// //TODO.. remove static
        public T MakeRequest<T>(string url, object parameters, bool includeToken, string method, string userAgent = UserAgent)
        {
            //Set up request
            //var environment = MyEnvironment.CurrentEnvironment;
            //var UserAgent = !string.IsNullOrEmpty(MyEnvironment.UserAgent) ? MyEnvironment.UserAgent : userAgent;
     
            var environment = MyEnvironment.CurrentEnvironment;
            var UserAgent = !string.IsNullOrEmpty(MyEnvironment.UserAgent) ? MyEnvironment.UserAgent : userAgent;
            string URL = environment + url;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.UserAgent = UserAgent;
            request.Timeout = Timeout;
            request.ReadWriteTimeout = ReadWriteTimeout;
            request.ContentType = "application/json";
            request.Method = method;
            request.CachePolicy = new System.Net.Cache.HttpRequestCachePolicy(System.Net.Cache.HttpRequestCacheLevel.NoCacheNoStore);

            // we need the token for every service call except for the initial login call
            if (includeToken)
            {
                // hmac using shared secret a representation of the message, as we are
                // including the time in the representation we also need it in the header
                // to check at the other end.
                // You might want to extend this to also include a username if, for instance,
                // the secret key varies by username
                request.Date = DateTime.UtcNow;
                var datePart = request.Date.ToUniversalTime().ToString(CultureInfo.InvariantCulture);
                var fullUri = URL;
                var contentMD5 = "";

                if (parameters != null)
                {
                    var json = JsonConvert.SerializeObject(parameters);
                    contentMD5 = Hashing.GetHashMD5OfString(json);
                }

                var messageRepresentation =
                    method + "\n" +
                    contentMD5 + "\n" +
                    datePart + "\n" +
                    fullUri;

                var hmac = Hashing.GetHashHMACSHA256OfString(messageRepresentation, APIKey);

                string auth = String.Format("MBL-HMAC-SHA256 key=\"{0}\", signature=\"{1}\"", APIID, hmac);
                request.Headers.Add("Authorization", auth);
                request.Headers.Add("APIID", APIID);
                request.Headers.Add("HMAC", hmac);
            }

            string serviceParams = "";
            serviceParams = JsonConvert.SerializeObject(parameters);
            if (request.Method == "POST")
            {
                byte[] data = Encoding.GetBytes(serviceParams);
                request.ContentLength = serviceParams == "null" ? 0 : data.Length;
            }

            ServicePointManager.ServerCertificateValidationCallback = CertificateValidationCallBack;

            if (request.Method != "GET" && parameters != null)
            {
                using (StreamWriter stream = new StreamWriter(request.GetRequestStream()))
                {
                    stream.Write(serviceParams);
                    stream.Flush();
                    stream.Close();
                }
            }

            HttpWebResponse response = null;
            response = (HttpWebResponse)request.GetResponse();
            string responsetream = null;
            if (response != null)
            {
                responsetream = GetResponseAsString<string>(response);
            }

            return JsonConvert.DeserializeObject<T>(responsetream);
        }

        #endregion

    }
}
