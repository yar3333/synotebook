using System;
using System.IO;
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

                AES.Key = SHA256Managed.Create().ComputeHash(passwordBytes);
                AES.IV = MD5.Create().ComputeHash(passwordBytes);
                AES.Mode = CipherMode.CBC;
                AES.Padding = PaddingMode.PKCS7;

                using (var mem = new MemoryStream())
                {
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

                AES.Key = SHA256Managed.Create().ComputeHash(passwordBytes);
                AES.IV = MD5.Create().ComputeHash(passwordBytes);
                AES.Mode = CipherMode.CBC;
                AES.Padding = PaddingMode.PKCS7;

                using (var mem = new MemoryStream())
                {
                    var bytesToDecrypt = Convert.FromBase64String(messageToDecrypt);

                    var crypto = new CryptoStream(mem, AES.CreateDecryptor(), CryptoStreamMode.Write);
                    crypto.Write(bytesToDecrypt, 0, bytesToDecrypt.Length);
                    crypto.FlushFinalBlock();

                    return Encoding.UTF8.GetString(mem.ToArray());
                }
            }
        }
    }
}
