using System.Security.Cryptography;
using System.Text;

namespace SilksongDecryptor
{
    public static class Encryption
    {
        private static readonly byte[] _keyArray = Encoding.UTF8.GetBytes("UKu52ePUBwetZ9wNX88o54dnfKRu0T1l");
        private static readonly Aes _aes = Aes.Create();

        static Encryption()
        {
            _aes.Key = _keyArray;
            _aes.Mode = CipherMode.ECB;
            _aes.Padding = PaddingMode.PKCS7;
        }

        public static byte[] Encrypt(byte[] decryptedBytes)
        {
            return _aes.EncryptEcb(decryptedBytes, PaddingMode.PKCS7);
        }

        public static byte[] Decrypt(byte[] encryptedBytes)
        {
            return _aes.DecryptEcb(encryptedBytes, PaddingMode.PKCS7);
        }

        public static string Encrypt(string unencryptedString)
        {
            byte[] array = Encrypt(Encoding.UTF8.GetBytes(unencryptedString));
            return Convert.ToBase64String(array, 0, array.Length);
        }

        public static string Decrypt(string encryptedString)
        {
            byte[] bytes = Decrypt(Convert.FromBase64String(encryptedString));
            return Encoding.UTF8.GetString(bytes);
        }
    }

}
