using System.Threading.Tasks;

namespace Tabs.Mobile.Shared.Interfaces.Users
{
    public interface IUserFactory
    {

        #region Methods

        /// <summary>
        /// Add user
        /// </summary>
        /// <param name="user"></param>
        Task AddNewUser(Models.Users.Users user);

        /// <summary>
        /// Add or edit individual
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userId"></param>
        /// <param name="isIndividual"></param>
        Task AddEditIndividual(string email, int userId, bool isIndividual);

        /// <summary>
        /// Add or edit barber
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userId"></param>
        /// <param name="isBusiness"></param>
        Task AddEditBusiness(string email, int userId, bool isBusiness);

        /// <summary>
        /// Does email exist
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<bool> EmailExist(string email);

        /// <summary>
        /// Does username exist
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<bool> UsernameExist(string username);

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<Models.Users.Users> Login(string email, string password);

        /// <summary>
        /// Logout
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task Logout(string email);

        /// <summary>
        /// Update user phone number
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userId"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        Task UpdateUserPhoneNumber(string email, int userId, long phoneNumber);

        /// <summary>
        /// Edit User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task EditUser(Models.Users.Users user);

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<Models.Users.Users> GetUser(string email, bool fromLogin = true);

        /// <summary>
        /// Get user by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Models.Users.Users> GetUser(int userId);

        /// <summary>
        /// Mark active
        /// </summary>
        /// <param name="userId"></param>
        Task MarkActive(int barberId);

        /// <summary>
        /// Mark inactive
        /// </summary>
        /// <param name="userId"></param>
        Task MarkInactive(int userId);

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
        Task<Models.Users.Users> GetUserByIdentifier(string identifier);

        /// <summary>
        /// update use password
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        Task UpdatePassword(int userId, string password);

        Task<string> GetStripeKey();

        Task<string> GetStorageConnectionKey();

        #endregion
    }
}
