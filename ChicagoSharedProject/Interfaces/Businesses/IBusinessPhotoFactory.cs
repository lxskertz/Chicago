using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Businesses;

namespace Tabs.Mobile.Shared.Interfaces.Businesses
{
    public interface IBusinessPhotoFactory
    {

        Task<int> Add(BusinesPhoto businesPhoto);

        Task<ICollection<BusinesPhoto>> Get(int businessId);
    }
}
