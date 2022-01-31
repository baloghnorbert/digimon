using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Core.Helper
{
    public static class PasswordHasher
    {
        /// <summary>
        /// Crates HASH i SALT based on the UserName i Password parameters
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Create(string password, string email)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                throw new Exception("Can't create password hash because secret key is null or empty!");
            }

            string result = CreateHash(CreateSalt(email), password);

            return result.Reverse().ToString().Replace("==", "");
        }

        /// <summary>
        /// Kreira SALT deo kodiranja
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private static string CreateSalt(string email)
        {
            string saltText = $"hu.tanulmanyiversenyek@{email.Split("@")[0]}";

            Rfc2898DeriveBytes hasher = new Rfc2898DeriveBytes(saltText, Encoding.Default.GetBytes(saltText), 10000);
            string result = Convert.ToBase64String(hasher.GetBytes(25));

            return result.Reverse();
        }

        /// <summary>
        /// Kreira HASH (realni) deo kodiranja na osnovu prethodno kreiranog SALT koda
        /// </summary>
        /// <param name="salt"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static string CreateHash(string salt, string password)
        {
            Rfc2898DeriveBytes Hasher = new Rfc2898DeriveBytes(password, Encoding.Default.GetBytes(salt), 10000);
            string result = Convert.ToBase64String(Hasher.GetBytes(25));

            return result;
        }

        public static string Reverse(this string text)
        {
            if (text == null) return null;
            char[] charArray = text.ToCharArray();
            int len = text.Length - 1;

            for (int i = 0; i < len; i++, len--)
            {
                charArray[i] ^= charArray[len];
                charArray[len] ^= charArray[i];
                charArray[i] ^= charArray[len];
            }

            return new string(charArray);
        }
    }
}
