using System.Reflection;

namespace SharpCrop.Dropbox.Credentials
{
    public static partial class Holder
    {
        public static string Get(string name)
        {
            FieldInfo field = typeof(Holder).GetField(name, BindingFlags.Static | BindingFlags.NonPublic);

            if(field != null)
            {
                return (string)field.GetValue(null);
            }

            return "";
        }

        public static string Key
        {
            get
            {
                return Get("key");
            }
        }

        public static string Secret
        {
            get
            {
                return Get("secret");
            }
        }
    }
}
