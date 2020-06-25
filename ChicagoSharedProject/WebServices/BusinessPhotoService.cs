using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Businesses;
using Tabs.Mobile.Shared.Interfaces.Businesses;

namespace Tabs.Mobile.Shared.WebServices
{
    public class BusinessPhotoService : BaseService, IBusinessPhotoFactory
    {

        #region Methods

        public async Task<int> Add(BusinesPhoto businesPhoto)
        {
            string methodPath = "businessphoto/";
            int response = 0;
            var parameters = new
            {
                BusinessId = businesPhoto.BusinessId,
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<int>(methodPath, parameters, true, "PUT"));
            response = await request;

            return response;
        }

        public async Task<ICollection<BusinesPhoto>> Get(int businessId)
        {
            string methodPath = "businessphoto/" + businessId;
            ICollection<BusinesPhoto> response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<BusinesPhoto>>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        #endregion

    }
}
