using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace KadoshRepository.Security
{
    internal static class PasswordEncodeUtil
    {
        public static (string hash, byte[] salt, int iterations) GetPasswordHashed(string password)
        {
            // Generate 128-bit salt
            byte[] salt = new byte[128 / 8];
            RandomNumberGenerator.Fill(salt);

            int iterations = RandomNumberGenerator.GetInt32(10000, 100000);

            return (GetPasswordHashed(password, salt, iterations), salt, iterations);
        }

        public static string GetPasswordHashed(string password, byte[] salt, int iterations)
        {
            // Hash password
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: iterations,
                numBytesRequested: 256 / 8)
            );

            return hashedPassword;
        }
    }
}
