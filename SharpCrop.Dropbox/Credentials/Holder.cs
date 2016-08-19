using System.Reflection;

namespace SharpCrop.Dropbox.Credentials
{
    public static partial class Holder
    {
        /// <summary>
        /// This is needed, because I do not want to expose my key-secret in plaintext into the public.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string Get(string name)
        {
            FieldInfo field = typeof(Holder).GetField(name, BindingFlags.Static | BindingFlags.NonPublic);

            if(field != null)
            {
                return (string)field.GetValue(null);
            }

            return "";
        }

        /// <summary>
        /// Dropbox API key.
        /// </summary>
        public static string Key
        {
            get
            {
                return Get("key");
            }
        }

        /// <summary>
        /// Dropbox API secret.
        /// </summary>
        public static string Secret
        {
            get
            {
                return Get("secret");
            }
        }
    }
}
