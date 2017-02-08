using System;
using System.Text;

namespace SharpCrop.Provider.Utils
{
    /// <summary>
    /// Simple algorithm to hide API keys and secrets from bots.
    /// Yep, I am paranoid!
    /// </summary>
    public static class Obscure
    {
        /// <summary>
        /// Byte offsetter.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        private static byte[] Code(byte[] bytes, int prefix)
        {
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)(bytes[i] + prefix * Math.Pow(-1, i) * bytes.Length);
            }

            return bytes;
        }

        /// <summary>
        /// Encode a string.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encode(string key)
        {
            if (key == null)
            {
                return null;
            }

            var bytes = Code(Encoding.UTF8.GetBytes(key), 1);

            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Decode an encoded string.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Decode(string key)
        {
            if(key == null)
            {
                return null;
            }

            var bytes = Code(Convert.FromBase64String(key), -1);

            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Decode an encoded base64 string.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Base64Encode(string key)
        {
            if (key == null)
            {
                return null;
            }

            var bytes = Encoding.UTF8.GetBytes(key);

            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Encode a string to base64.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Base64Decode(string key)
        {
            if (key == null)
            {
                return null;
            }

            var bytes = Convert.FromBase64String(key);

            return Encoding.UTF8.GetString(bytes);
        }
    }
}
