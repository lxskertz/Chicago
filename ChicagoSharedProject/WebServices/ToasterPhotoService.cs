using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Individuals;
using Tabs.Mobile.Shared.Interfaces.Individuals;

namespace Tabs.Mobile.Shared.WebServices
{
    public class ToasterPhotoService : BaseService, IToasterPhotoFactory
    {

        #region Methods

        public async Task<int> Add(ToasterPhoto toasterPhoto)
        {
            string methodPath = "toasterphoto/";
            int response = 0;
            var parameters = new
            {
                UserId = toasterPhoto.UserId,
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<int>(methodPath, parameters, true, "PUT"));
            response = await request;

            return response;
        }

        public async Task<ICollection<ToasterPhoto>> Get(int userId)
        {
            string methodPath = "toasterphoto/" + userId;
            ICollection<ToasterPhoto> response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<ToasterPhoto>>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        #endregion

    }
}
