using System.Threading.Tasks;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Interfaces;

namespace Tabs.Mobile.Shared.WebServices
{
    public class VerificationCodeService : BaseService, IVerificationCode
    {
        #region Methods

        /// <summary>
        /// Add new code
        /// </summary>
        /// <param name="verificationCode"></param>
        /// <returns></returns>
        public async Task AddVerificationCode(VerificationCode verificationCode)
        {
            string methodPath = "verification_code/";
            VerificationCode response = null;
            var parameters = new
            {
                Email = verificationCode.Email,
                UserId = verificationCode.UserId,
                Code = verificationCode.Code,
                SendCode = verificationCode.SendCode

            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<VerificationCode>(methodPath, parameters, false, "POST"));
            response = await request;
        }

        /// <summary>
        /// Delete code
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task DeleteVerificationCode(string email, int userId)
        {
            string methodPath = "verification_code/";
            VerificationCode response = null;
            var parameters = new
            {
                Email = email,
                UserId = userId,
                PhoneNumber = ""
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<VerificationCode>(methodPath, parameters, false, "DELETE"));
            response = await request;
        }

        /// <summary>
        /// Get code
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<VerificationCode> GetVerificationCode(int userId)
        {
            string methodPath = "verification_code/" + userId;
            VerificationCode response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<VerificationCode>(methodPath, null, false, "GET"));
            response = await request;

            return response;
        }

        #endregion
    }
}
