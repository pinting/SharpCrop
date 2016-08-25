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
            var bytes = Code(Convert.FromBase64String(key), -1);

            return Encoding.UTF8.GetString(bytes);
        }
    }
}
