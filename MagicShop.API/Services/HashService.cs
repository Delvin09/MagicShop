using MagicShop.API.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace MagicShop.API.Services
{
    public class HashService : IHashService
    {
        public (byte[] hash, byte[] salt) GetHash(string value, byte[]? salt = null)
        {
            using HMACSHA512 hmac = salt == null
                ? new HMACSHA512()
                : new HMACSHA512(salt);

            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
            return (hash, hmac.Key);
        }
    }
}
