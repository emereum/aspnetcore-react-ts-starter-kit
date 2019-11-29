using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace TemplateProductName.Domain.Services
{
    public class PasswordService : IPasswordService
    {
        /// <summary>
        /// See: https://www.owasp.org/index.php/Using_Rfc2898DeriveBytes_for_PBKDF2
        /// </summary>
        /// <param name="plaintextPassword"></param>
        /// <returns></returns>
        public HashedPassword HashPassword(string plaintextPassword)
        {
            // Generate the hash, with an automatic 32 byte salt
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(plaintextPassword, 32);
            rfc2898DeriveBytes.IterationCount = 10000;
            var hash = rfc2898DeriveBytes.GetBytes(20);
            var salt = rfc2898DeriveBytes.Salt;
            //Return the salt and the hash
            return new HashedPassword(Convert.ToBase64String(hash), Convert.ToBase64String(salt));
        }

        public bool VerifyPassword(HashedPassword expected, string plaintextPassword)
        {
            // Generate the hash, with the expected salt
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(plaintextPassword, Convert.FromBase64String(expected.Salt));
            rfc2898DeriveBytes.IterationCount = 10000;
            var hash = rfc2898DeriveBytes.GetBytes(20);
            return Convert.ToBase64String(hash) == expected.Hash;
        }
    }
}
