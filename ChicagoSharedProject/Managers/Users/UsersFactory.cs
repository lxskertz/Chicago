using System.Threading.Tasks;
using Tabs.Mobile.Shared.Interfaces.Users;
using Tabs.Mobile.Shared.Models.Users;
using Tabs.Mobile.Shared.Models;

namespace Tabs.Mobile.Shared.Managers.Users
{
    public class UsersFactory : IUserFactory
    {

        #region Constants, Enums, and Variables

        private IUserFactory _UserFactory;

        #endregion

        #region Constructors

        public UsersFactory(IUserFactory userFactory)
        {
            _UserFactory = userFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add user
        /// </summary>
        /// <param name="user"></param>
        public Task AddNewUser(Models.Users.Users user)
        {
            return _UserFactory.AddNewUser(user);
        }

        /// <summary>
        /// Add or edit individual
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userId"></param>
        /// <param name="isIndividual"></param>
        public Task AddEditIndividual(string email, int userId, bool isIndividual)
        {
            return _UserFactory.AddEditIndividual(email, userId, isIndividual);
        }

        /// <summary>
        /// Add or edit barber
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userId"></param>
        /// <param name="isBusiness"></param>
        public Task AddEditBusiness(string email, int userId, bool isBusiness)
        {
            return _UserFactory.AddEditBusiness(email, userId, isBusiness);
        }

        /// <summary>
        /// Does email exist
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task<bool> EmailExist(string email)
        {
            return _UserFactory.EmailExist(email);
        }

        /// <summary>
        /// Does username exist
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public Task<bool> UsernameExist(string username)
        {
            return _UserFactory.UsernameExist(username);
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Task<Models.Users.Users> Login(string email, string password)
        {
            return _UserFactory.Login(email, password);
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task Logout(string email)
        {
            return _UserFactory.Logout(email);
        }

        /// <summary>
        /// Update user phone number
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userId"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public Task UpdateUserPhoneNumber(string email, int userId, long phoneNumber)
        {
            return _UserFactory.UpdateUserPhoneNumber(email, userId, phoneNumber);
        }

        /// <summary>
        /// Edit User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task EditUser(Models.Users.Users user)
        {
            return _UserFactory.EditUser(user);
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task<Models.Users.Users> GetUser(string email, bool fromLogin = true)
        {
            return _UserFactory.GetUser(email, fromLogin);
        }

        /// <summary>
        /// Get user by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<Models.Users.Users> GetUser(int userId)
        {
            return _UserFactory.GetUser(userId);
        }

        /// <summary>
        /// Mark active
        /// </summary>
        /// <param name="userId"></param>
        public Task MarkActive(int userId)
        {
            return _UserFactory.MarkActive(userId);
        }

        /// <summary>
        /// Mark inactive
        /// </summary>
        /// <param name="userId"></param>
        public Task MarkInactive(int userId)
        {
            return _UserFactory.MarkInactive(userId);
        }

        /// <summary>
        /// Update session id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="mobileSessionId"></param>
        //void UpdatMobileSessionId(int userId, string mobileSessionId);

        /// <summary>
        /// Get users by identifier
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public Task<Models.Users.Users> GetUserByIdentifier(string identifier)
        {
            return _UserFactory.GetUserByIdentifier(identifier);
        }

        /// <summary>
        /// update use password
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        public Task UpdatePassword(int userId, string password)
        {
            return _UserFactory.UpdatePassword(userId, password);
        }

        public Task<string> GetStripeKey()
        {
            return _UserFactory.GetStripeKey();
        }

        public Task<string> GetStorageConnectionKey()
        {
            return _UserFactory.GetStorageConnectionKey();
        }

        #endregion

    }
}