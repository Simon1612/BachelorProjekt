using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace DentalResearchApp.Code.Impl
{
    public static class Hash
    {
        public static string Create(string password, byte[] salt)
        {
            // derive a 256-bit subkey (use HMACSHA256 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
