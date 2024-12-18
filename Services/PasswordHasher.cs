using AuthentificationExample.Server.Abstractions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace AuthentificationExample.Server.Services
{
    public class PasswordHasher() : IPasswordHasher
    {
        public bool Verify(string unhashedPassword, string hashedPassword, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);

            string computedHashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: unhashedPassword,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32
                )
            );

            return computedHashedPassword == hashedPassword;
        }
    }
}
