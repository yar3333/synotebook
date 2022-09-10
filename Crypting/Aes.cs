using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SyNotebook.Crypting
{
    public static class Aes
    {
        public static string Encrypt(string messageToEncrypt, string password)
        {
            using (var AES = new AesCryptoServiceProvider())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);

                AES.Key = SHA256.Create().ComputeHash(passwordBytes);
                AES.GenerateIV();
                AES.Mode = CipherMode.CBC;
                AES.Padding = PaddingMode.PKCS7;

                using (var mem = new MemoryStream())
                {
                    mem.Write(AES.IV, 0, AES.IV.Length);

                    var bytesToEncrypt = Encoding.UTF8.GetBytes(messageToEncrypt);
                        
                    var crypto = new CryptoStream(mem, AES.CreateEncryptor(), CryptoStreamMode.Write);
                    crypto.Write(bytesToEncrypt, 0, bytesToEncrypt.Length);
                    crypto.FlushFinalBlock();
                        
                    return Convert.ToBase64String(mem.ToArray());
                }
            }
        }

        public static string Decrypt(string messageToDecrypt, string password)
        {
            using (var AES = new AesCryptoServiceProvider())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);

                AES.Key = SHA256.Create().ComputeHash(passwordBytes);
                AES.Mode = CipherMode.CBC;
                AES.Padding = PaddingMode.PKCS7;

                var bytesToDecrypt = Convert.FromBase64String(messageToDecrypt);
                AES.IV = bytesToDecrypt.Take(AES.IV.Length).ToArray();

                using (var mem = new MemoryStream())
                {
                    var crypto = new CryptoStream(mem, AES.CreateDecryptor(), CryptoStreamMode.Write);
                    crypto.Write(bytesToDecrypt, AES.IV.Length, bytesToDecrypt.Length - AES.IV.Length);
                    crypto.FlushFinalBlock();

                    return Encoding.UTF8.GetString(mem.ToArray());
                }
            }
        }
    }
}
