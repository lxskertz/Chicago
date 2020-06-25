using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Businesses;
using Tabs.Mobile.Shared.Interfaces.Businesses;

namespace Tabs.Mobile.Shared.Managers.Businesses
{
    public class BusinessPhotoFactory : IBusinessPhotoFactory
    {

        #region Constants, Enums, and Variables

        private IBusinessPhotoFactory _BusinessPhotoFactory;

        #endregion

        #region Constructors

        public BusinessPhotoFactory(IBusinessPhotoFactory businessPhotoFactory)
        {
            _BusinessPhotoFactory = businessPhotoFactory;
        }

        #endregion

        #region Methods

        public Task<int> Add(BusinesPhoto businesPhoto)
        {
            return _BusinessPhotoFactory.Add(businesPhoto);
        }

        public Task<ICollection<BusinesPhoto>> Get(int businessId)
        {
            return _BusinessPhotoFactory.Get(businessId);
        }

        #endregion

    }
}
