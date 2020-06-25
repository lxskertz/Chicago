using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Tabs.Mobile.Shared.Models;
using Tabs.Mobile.Shared.Interfaces.Users;

namespace Tabs.Mobile.Shared.WebServices
{
    public class UsersService : BaseService, IUserFactory 
    {

        #region Methods

        /// <summary>
        /// Add user
        /// </summary>
        /// <param name="user"></param>
        public async Task AddNewUser(Models.Users.Users user)
        {
            DetermineEnvironmentandUsername(user.Email);

            string methodPath = "user/add_user/";
            HttpRequestMessage response = null;
            var parameters = new
            {
                Email = this.Username, //user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PasswordHash = user.PasswordHash,
                Username = user.Username,
                Birthday = user.Birthday,
                IsAdmin = user.IsAdmin,
                IsBusiness = user.IsBusiness,
                IsIndividual = user.IsIndividual,
                UserStatus = user.UserStatus,
                AccountLocked = false,
                TabsAdmin = false

            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpRequestMessage>(methodPath, parameters, false, "POST"));
            response = await request;
        }

        /// <summary>
        /// Add or edit individual
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userId"></param>
        /// <param name="isIndividual"></param>
        public async Task AddEditIndividual(string email, int userId, bool isIndividual)
        {
            string methodPath = "user/add_edit_individual";
            HttpRequestMessage response = null;
            var parameters = new
            {
                Email = email,
                UserId = userId,
                IsIndividual = isIndividual

            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpRequestMessage>(methodPath, parameters, true, "POST"));
            response = await request;
        }

        /// <summary>
        /// Add or edit barber
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userId"></param>
        /// <param name="isBusiness"></param>
        public async Task AddEditBusiness(string email, int userId, bool isBusiness)
        {
            string methodPath = "user/add_edit_business";
            HttpRequestMessage response = null;
            var parameters = new
            {
                Email = email,
                UserId = userId,
                IsBusiness = isBusiness

            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpRequestMessage>(methodPath, parameters, true, "POST"));
            response = await request;
        }

        /// <summary>
        /// Does email exist
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> EmailExist(string email)
        {
            DetermineEnvironmentandUsername(email);

            string methodPath = "user/email_exist/";
            bool response = false;
            var parameters = new
            {
                Email = this.Username, //email,
                Password = string.Empty,
                Username = string.Empty
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<bool>(methodPath, parameters, false, "POST"));
            response = await request;

            return response;
        }

        /// <summary>
        /// Does username exist
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<bool> UsernameExist(string username)
        {
            string methodPath = "user/username_exist/";
            bool response = false;
            var parameters = new
            {
                Email = string.Empty,
                Password = string.Empty,
                Username = username
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<bool>(methodPath, parameters, false, "POST"));
            response = await request;

            return response;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<Models.Users.Users> Login(string email, string password)
        {
            DetermineEnvironmentandUsername(email);

            string methodPath = "login/";
            Models.Users.Users response = null;
            var parameters = new
            {
                Email = this.Username,
                Password = password
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<Models.Users.Users>(methodPath, parameters, false, "POST"));
            response = await request;

            return response;
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task Logout(string email)
        {
            string methodPath = "logout/";
            Models.Users.Users response = null;
            var parameters = new
            {
                email = email
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<Models.Users.Users>(methodPath, email, true, "POST"));
            response = await request;
        }

        /// <summary>
        /// Update user phone number
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userId"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public async Task UpdateUserPhoneNumber(string email, int userId, long phoneNumber)
        {
            string methodPath = "user/edit_userphone";
            HttpResponseMessage response = null;
            var parameters = new
            {
                Email = email,
                UserId = userId,
                PhoneNumber = phoneNumber
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpResponseMessage>(methodPath, parameters, true, "POST"));
            response = await request;
        }

        /// <summary>
        /// Edit User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task EditUser(Models.Users.Users user)
        {
            string methodPath = "user/edit_user";
            HttpRequestMessage response = null;
            var parameters = new
            {
                Email = user.Email,
                UserId = user.UserId,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthday = user.Birthday

            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpRequestMessage>(methodPath, parameters, true, "POST"));
            response = await request;
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Models.Users.Users> GetUser(string email, bool fromLogin = true)
        {
            if (fromLogin)
            {
                DetermineEnvironmentandUsername(email);
            }

            string methodPath = "user_by_email/";
            Models.Users.Users response = null;
            var parameters = new
            {
                Email = fromLogin ? this.Username : email,
                Password = string.Empty,
                Username = string.Empty
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<Models.Users.Users>(methodPath, parameters, false, "POST"));
            response = await request;

            return response;
        }

        /// <summary>
        /// Get user by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Models.Users.Users> GetUser(int userId)
        {
            string methodPath = "user/" + userId;
            Models.Users.Users response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<Models.Users.Users>(methodPath, null, false, "GET"));
            response = await request;

            return response;
        }

        /// <summary>
        /// Mark active
        /// </summary>
        /// <param name="userId"></param>
        public async Task MarkActive(int userId)
        {
            string methodPath = "user/mark_active/" + userId;
            HttpResponseMessage response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpResponseMessage>(methodPath, null, true, "POST"));
            response = await request;
        }

        /// <summary>
        /// Mark inactive
        /// </summary>
        /// <param name="userId"></param>
        public async Task MarkInactive(int userId)
        {
            string methodPath = "user/mark_inactive/" + userId;
            HttpResponseMessage response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpResponseMessage>(methodPath, null, true, "POST"));
            response = await request;
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
            return null; //TODO
        }

        /// <summary>
        /// update use password
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        public async Task UpdatePassword(int userId, string password)
        {
            string methodPath = "user/update_password";
            HttpResponseMessage response = null;
            var parameters = new
            {
                UserId = userId,
                Password = password
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpResponseMessage>(methodPath, parameters, false, "POST"));
            response = await request;
        }

        /// <summary>
        /// Get user by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<string> GetStripeKey()
        {
            string methodPath = "user/stripekey/";
            string response = "";
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<string>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        /// <summary>
        /// Get user by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<string> GetStorageConnectionKey()
        {
            string methodPath = "user/storageconnectionkey";
            string response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<string>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }
        #endregion

    }
}
