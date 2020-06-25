using System;
using System.Collections.Generic;
using System.Text;

namespace Tabs.Mobile.Shared.Helpers
{
    public class PasswordHash
    {

        #region Methods

        /// <summary>
        ///  Hash password and return it
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string DoHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
        }

        /// <summary>
        /// Verify Password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static bool CheckPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        /// <summary>
        /// Generate Verification Code
        /// </summary>
        /// <returns></returns>
        public static string GenerateCodeNumber()
        {
            Random generator = new Random();
            return generator.Next(0, 1000000).ToString("D6");
        }


        #endregion

    }
}
