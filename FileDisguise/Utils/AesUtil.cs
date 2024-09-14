using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FileDisguise.Utils
{
    public class AesUtil
    {
        public static byte[] Encrypt(byte[] plainBytes)
        {
            return EncryptBytes(plainBytes, MainWindow.Key, MainWindow.IV);
        }

        public static byte[] Decrypt(byte[] cipherBytes)
        {
            return DecryptBytes(cipherBytes, MainWindow.Key, MainWindow.IV);
        }

        public static byte[] EncryptBytes(byte[] plainBytes, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainBytes == null || plainBytes.Length <= 0)
                throw new ArgumentNullException("plainBytes");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            using (var aes = Aes.Create())
            {
                using (ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV))
                {
                    return encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                }
            }
        }

        public static byte[] DecryptBytes(byte[] cipherBytes, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherBytes == null || cipherBytes.Length <= 0)
                throw new ArgumentNullException("cipherBytes");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            using (var aes = Aes.Create())
            {
                using (ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV))
                {
                    return decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                }
            }
        }

    }
}
