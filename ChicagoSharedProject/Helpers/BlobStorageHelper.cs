using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Tabs.Mobile.Shared.Helpers
{
    public class BlobStorageHelper
    {

        #region Constants, Enums, and Variables

        private static string businessContainerName = "businesslogocontainer";
        public static string businessLogoBlob = "businesslogoblob";

        private static string toasterContainer = "toasterprofilecontainer";
        public static string toasterProfileBlob = "toasterprofileblob";

        public static string businesseventsBlob = "businesseventsblob";
        private static string businesseventsContainer = "businesseventscontainer";

        public static string checkinsBlob = "checkinsblob";
        private static string checkinsContainer = "checkinscontainer";

        public static string businessPhotosBlob = "businessphotosblob";
        private static string businessPhotosContainer = "businessphotoscontainer";

        public static string toasterPhotosBlob = "toasterphotosblob";
        private static string toasterPhotosContainer = "toasterphotoscontainer";

        #endregion

        #region Properties

        public static string ConnectionString { get; set; } = string.Empty;

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logo"></param>
        /// <returns></returns>
        public static async Task<byte[]> GetImageData(Uri imageUri)
        {
            byte[] data = null;
            try
            {
                using (var c = new GzipWebClient())
                {
                    data = await c.DownloadDataTaskAsync(imageUri);
                }
            }
            finally
            {
            }

            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static async Task SaveToasterProfileBlob(string path, int userId)
        {
            try
            {
                // Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference(toasterContainer);

                // Create the container if it doesn't already exist.
                await container.CreateIfNotExistsAsync();

                // Retrieve reference to a blob named "myblob".
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(toasterProfileBlob + userId.ToString());

                // Create the "myblob" blob with the text "Hello, world!"
                //var source = System.IO.File(filePath);
                //blockBlob.UploadFromStreamAsync(new Stream(source), source.length());

                //if (blockBlob != null && await blockBlob.ExistsAsync())
                //{
                //    await blockBlob.DeleteAsync();
                //}

                await blockBlob.UploadFromFileAsync(path);
                blockBlob.Properties.ContentType = string.IsNullOrEmpty(Path.GetExtension(path)) ? ".png" : Path.GetExtension(path);
                await blockBlob.SetPropertiesAsync();
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="userId"></param>
       /// <returns></returns>
        public static async Task<string> GetToasterBlobUri(int userId)
        {
            try
            {
                // Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference(toasterContainer);

                // Create the container if it doesn't already exist.
                await container.CreateIfNotExistsAsync();

                // Retrieve reference to a blob named "myblob".
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(toasterProfileBlob + userId.ToString());


                SharedAccessBlobPolicy policy = new SharedAccessBlobPolicy()
                {
                    Permissions = SharedAccessBlobPermissions.Read,
                    SharedAccessExpiryTime = DateTime.UtcNow.AddDays(3)
                };
                //Set content-disposition header for force download
                SharedAccessBlobHeaders headers = new SharedAccessBlobHeaders()
                {
                    ContentDisposition = string.Format("attachment;filename=\"{0}\"", userId),
                };

                if (blockBlob != null && await blockBlob.ExistsAsync())
                {
                    var sasToken = blockBlob.GetSharedAccessSignature(policy, headers);
                    return blockBlob.Uri.AbsoluteUri + sasToken;
                }
            }
            catch (Exception ex)
            {
                var a = ex;
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static async Task SaveEventLogoBlob(string path, int eventId)
        {
            try
            {
                // Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference(businesseventsContainer);

                // Create the container if it doesn't already exist.
                await container.CreateIfNotExistsAsync();

                // Retrieve reference to a blob named "myblob".
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(businesseventsBlob + eventId.ToString());

                // Create the "myblob" blob with the text "Hello, world!"
                //var source = System.IO.File(filePath);
                //blockBlob.UploadFromStreamAsync(new Stream(source), source.length());

                await blockBlob.UploadFromFileAsync(path);
                blockBlob.Properties.ContentType = string.IsNullOrEmpty(Path.GetExtension(path)) ? ".png" : Path.GetExtension(path);
                await blockBlob.SetPropertiesAsync();
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static async Task DeleteEventLogoBlob(int eventId)
        {
            try
            {
                // Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference(businesseventsContainer);

                if(container != null)
                {
                    // Retrieve reference to a blob named "myblob".
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(businesseventsBlob + eventId.ToString());

                    await blockBlob.DeleteIfExistsAsync();
                }

               
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async static Task<string> GetEventLogoUri(int eventId)
        {          
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(businesseventsContainer);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(businesseventsBlob + eventId.ToString());

            SharedAccessBlobPolicy policy = new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessExpiryTime = DateTime.UtcNow.AddDays(3)
            };
            //Set content-disposition header for force download
            SharedAccessBlobHeaders headers = new SharedAccessBlobHeaders()
            {
                ContentDisposition = string.Format("attachment;filename=\"{0}\"", eventId),
            };

            if (blockBlob != null && await blockBlob.ExistsAsync())
            {
                var sasToken = blockBlob.GetSharedAccessSignature(policy, headers);
                return blockBlob.Uri.AbsoluteUri + sasToken;
            }

            return "";
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static async Task SaveCheckinImageBlob(string path, int checkIn)
        {
            try
            {
                // Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference(checkinsContainer);

                // Create the container if it doesn't already exist.
                await container.CreateIfNotExistsAsync();

                // Retrieve reference to a blob named "myblob".
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(checkinsBlob + checkIn.ToString());

                // Create the "myblob" blob with the text "Hello, world!"
                //var source = System.IO.File(filePath);
                //blockBlob.UploadFromStreamAsync(new Stream(source), source.length());

                await blockBlob.UploadFromFileAsync(path);
                blockBlob.Properties.ContentType = string.IsNullOrEmpty(Path.GetExtension(path)) ? ".png" : Path.GetExtension(path);
                await blockBlob.SetPropertiesAsync();
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async static Task<string> GetCheckIntLogoUri(int checkIn)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(checkinsContainer);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(checkinsBlob + checkIn.ToString());

            SharedAccessBlobPolicy policy = new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessExpiryTime = DateTime.UtcNow.AddDays(3)
            };
            //Set content-disposition header for force download
            SharedAccessBlobHeaders headers = new SharedAccessBlobHeaders()
            {
                ContentDisposition = string.Format("attachment;filename=\"{0}\"", checkIn),
            };

            if (blockBlob != null && await blockBlob.ExistsAsync())
            {
                var sasToken = blockBlob.GetSharedAccessSignature(policy, headers);
                return blockBlob.Uri.AbsoluteUri + sasToken;
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static async Task SaveBusinessLogoBlob(string path, int businessId)
        {
            try
            {
                // Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference(businessContainerName);

                // Create the container if it doesn't already exist.
                await container.CreateIfNotExistsAsync();

                // Retrieve reference to a blob named "myblob".
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(businessLogoBlob + businessId.ToString());

                if (blockBlob != null && await blockBlob.ExistsAsync())
                {
                    await blockBlob.DeleteAsync();
                }

                await blockBlob.UploadFromFileAsync(path);
                blockBlob.Properties.ContentType = string.IsNullOrEmpty(Path.GetExtension(path)) ? ".png" : Path.GetExtension(path);
                await blockBlob.SetPropertiesAsync();
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async static Task<string> GetBusinessLogoUri(int businessId)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(businessContainerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(businessLogoBlob + businessId.ToString());

            SharedAccessBlobPolicy policy = new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessExpiryTime = DateTime.UtcNow.AddDays(3)
            };
            //Set content-disposition header for force download
            SharedAccessBlobHeaders headers = new SharedAccessBlobHeaders()
            {
                ContentDisposition = string.Format("attachment;filename=\"{0}\"", businessId),
            };

            if (blockBlob != null && await blockBlob.ExistsAsync())
            {
                var sasToken = blockBlob.GetSharedAccessSignature(policy, headers);
                return blockBlob.Uri.AbsoluteUri + sasToken;
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static async Task SaveBusinessPhotosBlob(string path, int businessId, int photoId)
        {
            try
            {
                var blob = businessPhotosBlob + photoId.ToString();
                // Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference(businessPhotosContainer + businessId);

                // Create the container if it doesn't already exist.
                await container.CreateIfNotExistsAsync();

                // Retrieve reference to a blob named "myblob".
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(blob);

                await blockBlob.UploadFromFileAsync(path);
                blockBlob.Properties.ContentType = string.IsNullOrEmpty(Path.GetExtension(path)) ? ".png" : Path.GetExtension(path);
                await blockBlob.SetPropertiesAsync();
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async static Task<string> GetBusinessPhotosUri(int businessId, int photoId)
        {
            var blob = businessPhotosBlob + photoId.ToString();

            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(businessPhotosContainer + businessId);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blob);

            SharedAccessBlobPolicy policy = new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessExpiryTime = DateTime.UtcNow.AddDays(3)
            };
            //Set content-disposition header for force download
            SharedAccessBlobHeaders headers = new SharedAccessBlobHeaders()
            {
                ContentDisposition = string.Format("attachment;filename=\"{0}\"", photoId),
            };

            if (blockBlob != null && await blockBlob.ExistsAsync())
            {
                var sasToken = blockBlob.GetSharedAccessSignature(policy, headers);
                return blockBlob.Uri.AbsoluteUri + sasToken;
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static async Task SaveToasterPhotosBlob(string path, int userId, int photoId)
        {
            try
            {
                var blob = toasterPhotosBlob + photoId.ToString();
                // Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference(toasterPhotosContainer + userId);

                // Create the container if it doesn't already exist.
                await container.CreateIfNotExistsAsync();

                // Retrieve reference to a blob named "myblob".
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(blob);

                await blockBlob.UploadFromFileAsync(path);
                blockBlob.Properties.ContentType = string.IsNullOrEmpty(Path.GetExtension(path)) ? ".png" : Path.GetExtension(path);
                await blockBlob.SetPropertiesAsync();
            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async static Task<string> GetToasterPhotosUri(int userId, int photoId)
        {
            var blob = toasterPhotosBlob + photoId.ToString();

            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(toasterPhotosContainer + userId);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blob);

            SharedAccessBlobPolicy policy = new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessExpiryTime = DateTime.UtcNow.AddDays(3)
            };
            //Set content-disposition header for force download
            SharedAccessBlobHeaders headers = new SharedAccessBlobHeaders()
            {
                ContentDisposition = string.Format("attachment;filename=\"{0}\"", photoId),
            };

            if (blockBlob != null && await blockBlob.ExistsAsync())
            {
                var sasToken = blockBlob.GetSharedAccessSignature(policy, headers);
                return blockBlob.Uri.AbsoluteUri + sasToken;
            }

            return "";
        }

        #endregion

    }
}
