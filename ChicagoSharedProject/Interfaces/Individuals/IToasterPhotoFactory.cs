using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Individuals;

namespace Tabs.Mobile.Shared.Interfaces.Individuals
{
    public interface IToasterPhotoFactory
    {

        Task<int> Add(ToasterPhoto toasterPhoto);

        Task<ICollection<ToasterPhoto>> Get(int businessId);

    }
}
