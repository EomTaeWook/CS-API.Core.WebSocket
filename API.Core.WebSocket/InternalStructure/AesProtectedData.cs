using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace API.Core.WebSocket.InternalStructure
{
    public class AesProtectedData : IProtectedData
    {
        private static readonly UTF8Encoding _encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
        private static byte[] _protectedKey = { 103, 195, 113, 253, 32, 79, 188, 191, 151, 66, 49, 13, 26, 157, 149, 27, 204, 161, 170, 134, 68, 73, 65, 84 };
        public string Protect(string connectionToken)
        {
            return Encrypt(_encoding.GetBytes(connectionToken));
        }
        private string Encrypt(byte[] unprotectedBytes)
        {
            var aesAlg = new RijndaelManaged();
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;
            aesAlg.KeySize = 256;

            aesAlg.Key = _protectedKey;
            aesAlg.IV = _protectedKey.Take(16).ToArray();
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            byte[] protectedBytes = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(unprotectedBytes, 0, unprotectedBytes.Length);
                    cs.Close();
                }
                protectedBytes = ms.ToArray();
                ms.Close();
            }
            return Convert.ToBase64String(protectedBytes);

        }
        public string Unprotect(string connectionToken)
        {
            return Decrypt(Convert.FromBase64String(connectionToken));
        }
        public string Decrypt(byte[] protectedBytes)
        {
            var aesAlg = new RijndaelManaged();
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;
            aesAlg.KeySize = 256;

            aesAlg.Key = _protectedKey;
            aesAlg.IV = _protectedKey.Take(16).ToArray();
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            byte[] unprotectedByte = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                {
                    cs.Write(protectedBytes, 0, protectedBytes.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                unprotectedByte = ms.ToArray();
                ms.Close();
            }
            return _encoding.GetString(unprotectedByte);
        }
    }
}
