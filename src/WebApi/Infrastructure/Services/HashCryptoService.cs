using System.Security.Cryptography;
using System.Text;
using LaserPointer.WebApi.Application.Common.Interfaces;
using LaserPointer.WebApi.Application.Common.Models;
using LaserPointer.WebApi.Infrastructure.Helpers;

namespace LaserPointer.WebApi.Infrastructure.Services
{
    public class HashCryptoService : IHashCryptoService
    {
        private readonly GlobalSettings _globalSettings;

        public HashCryptoService(GlobalSettings globalSettings)
        {
            _globalSettings = globalSettings;
        }

        public string DecryptPlainTextValue(byte[] cipherText)
        {
            byte[] secret = Encoding.UTF8.GetBytes(_globalSettings.AesSecret);
            
            var sha = new SHA256Managed();
            byte[] key = sha.ComputeHash(secret);

            return AesHelper.DecryptStringFromBytes_Aes(cipherText, key);
        }

        public byte[] EncryptPlainTextValue(string plain)
        {
            byte[] secret = Encoding.UTF8.GetBytes(_globalSettings.AesSecret);
            
            var sha = new SHA256Managed();
            byte[] key = sha.ComputeHash(secret);

            return AesHelper.EncryptStringToBytes_Aes(plain, key);
        }
    }
}
