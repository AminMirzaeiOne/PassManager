using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PassManager.Infrastructure.Security
{
    public class CryptoService
    {
        public static byte[] GenerateSalt(int length = 16)
        {
            var salt = new byte[length];
            RandomNumberGenerator.Fill(salt);
            return salt;
        }

        // PBKDF2 password hash (for authentication)
        public static byte[] HashPassword(string password, byte[] salt, int iterations = 100_000, int length = 32)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            return pbkdf2.GetBytes(length);
        }

        // derive AES key from password + keysalt
        public static byte[] DeriveKey(string password, byte[] keySalt, int keyLen = 32, int iterations = 100_000)
        {
            return HashPassword(password, keySalt, iterations, keyLen);
        }

        // AES-GCM encrypt => return base64(nonce + tag + ciphertext)
        public static string EncryptString(byte[] key, string plaintext)
        {
            var plainBytes = Encoding.UTF8.GetBytes(plaintext);
            var nonce = new byte[12]; RandomNumberGenerator.Fill(nonce);
            var cipher = new byte[plainBytes.Length];
            var tag = new byte[16];

            using var aes = new AesGcm(key);
            aes.Encrypt(nonce, plainBytes, cipher, tag);

            var outBytes = new byte[nonce.Length + tag.Length + cipher.Length];
            Buffer.BlockCopy(nonce, 0, outBytes, 0, nonce.Length);
            Buffer.BlockCopy(tag, 0, outBytes, nonce.Length, tag.Length);
            Buffer.BlockCopy(cipher, 0, outBytes, nonce.Length + tag.Length, cipher.Length);

            return Convert.ToBase64String(outBytes);
        }

        public static string DecryptString(byte[] key, string base64Input)
        {
            var all = Convert.FromBase64String(base64Input);
            var nonce = new byte[12];
            var tag = new byte[16];
            var cipher = new byte[all.Length - nonce.Length - tag.Length];

            Buffer.BlockCopy(all, 0, nonce, 0, nonce.Length);
            Buffer.BlockCopy(all, nonce.Length, tag, 0, tag.Length);
            Buffer.BlockCopy(all, nonce.Length + tag.Length, cipher, 0, cipher.Length);

            var plain = new byte[cipher.Length];
            using var aes = new AesGcm(key);
            aes.Decrypt(nonce, cipher, tag, plain);

            return Encoding.UTF8.GetString(plain);
        }
    }
}
