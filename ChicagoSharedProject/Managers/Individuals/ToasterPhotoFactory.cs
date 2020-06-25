using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models.Individuals;
using Tabs.Mobile.Shared.Interfaces.Individuals;

namespace Tabs.Mobile.Shared.Managers.Individuals
{
    public class ToasterPhotoFactory : IToasterPhotoFactory
    {

        #region Constants, Enums, and Variables

        private IToasterPhotoFactory _ToasterPhotoFactory;

        #endregion

        #region Constructors

        public ToasterPhotoFactory(IToasterPhotoFactory toasterPhotoFactory)
        {
            _ToasterPhotoFactory = toasterPhotoFactory;
        }

        #endregion

        #region Methods

        public Task<int> Add(ToasterPhoto toasterPhoto)
        {
            return _ToasterPhotoFactory.Add(toasterPhoto);
        }

        public Task<ICollection<ToasterPhoto>> Get(int userId)
        {
            return _ToasterPhotoFactory.Get(userId);
        }

        #endregion

    }
}
