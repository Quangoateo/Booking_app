using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BookingApp.Helpers
{
    public static class MyUtility
    {
        private static readonly string _Salt = "K6KVX-6NWTF-2JPJ3-Q29H6-H8RC6";
        public static string ToEncryptPassword(this string password)
        {
            if (string.IsNullOrEmpty(password)) return "";
            var ASCIIENC = new ASCIIEncoding();
            string strreturn;
            strreturn = string.Empty;
            var bytesourcetxt = ASCIIENC.GetBytes(password);
            var SHA1Hash = new SHA1CryptoServiceProvider();
            byte[] bytehash = SHA1Hash.ComputeHash(bytesourcetxt);
            foreach (byte b in bytehash)
                strreturn += b.ToString("X2");
            return strreturn;
        }

        public static string ToEncrypt(this string password, string salt = "")
        {
            if (string.IsNullOrEmpty(salt))
                salt = _Salt;
            var data = Encoding.UTF8.GetBytes(password);

            using (var md5 = new MD5CryptoServiceProvider())
            {
                var keys = md5.ComputeHash(Encoding.UTF8.GetBytes(salt));
                using (var tripDes = new TripleDESCryptoServiceProvider { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    var transform = tripDes.CreateEncryptor();
                    var results = transform.TransformFinalBlock(data, 0, data.Length);
                    return Convert.ToBase64String(results, 0, results.Length);
                }
            }
        }
        public static bool IsBase64(this string base64String)
        {
            if (base64String.Replace(" ", "").Length % 4 != 0)
            {
                return false;
            }

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch
            {
                // Handle the exception
            }
            return false;
        }
        public static string ToDecrypt(this string password, string salt = "")
        {
            if (string.IsNullOrEmpty(salt))
                salt = _Salt;
            var data = Convert.FromBase64String(password);
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var keys = md5.ComputeHash(Encoding.UTF8.GetBytes(salt));

                using (var tripDes = new TripleDESCryptoServiceProvider()
                {
                    Key = keys,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                })
                {
                    var transform = tripDes.CreateDecryptor();
                    var results = transform.TransformFinalBlock(data, 0, data.Length);
                    return Encoding.UTF8.GetString(results);
                }
            }
        }
        public static string ToSha256(this string inputString)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString + _Salt);
            byte[] hash = sha256.ComputeHash(bytes);
            return hash.GetStringFromHash();
        }

        public static string ToSha512(this string inputString)
        {
            SHA512 sha512 = SHA512.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString + _Salt);
            byte[] hash = sha512.ComputeHash(bytes);
            return hash.GetStringFromHash();
        }
        public static bool VerifyHashedPassword(this string pass, string confirmPass)
        {
            byte[] array1 = Encoding.UTF8.GetBytes(pass);
            byte[] array2 = Encoding.UTF8.GetBytes(confirmPass);
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }
            return true;
        }
        private static string GetStringFromHash(this byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

    }
}
