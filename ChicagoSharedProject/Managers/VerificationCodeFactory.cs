using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Interfaces;

namespace Tabs.Mobile.Shared.Managers
{
    public class VerificationCodeFactory
    {

        #region Constants, Enums, and Variables

        private IVerificationCode _verificationCode;

        #endregion

        #region Constructors

        public VerificationCodeFactory(IVerificationCode verificationCode)
        {
            _verificationCode = verificationCode;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add new code
        /// </summary>
        /// <param name="verificationCode"></param>
        /// <returns></returns>
        public Task AddVerificationCode(VerificationCode verificationCode)
        {
            return _verificationCode.AddVerificationCode(verificationCode);
        }

        /// <summary>
        /// Delete code
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task DeleteVerificationCode(string email, int userId)
        {
            return _verificationCode.DeleteVerificationCode(email, userId);
        }

        /// <summary>
        /// Get code
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task<VerificationCode> GetVerificationCode(int userId)
        {
            return _verificationCode.GetVerificationCode(userId);
        }

        #endregion

    }
}
