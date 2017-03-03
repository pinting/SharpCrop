using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace SharpCrop.Utils
{
    public static class UpdateHelper
    {
        /// <summary>
        /// Check for updates.
        /// </summary>
        /// <returns>URL of the new version or null.</returns>
        public static string GetLatest()
        {
            var request = (HttpWebRequest)WebRequest.Create(Constants.LatestVersion);

            request.UserAgent = "SharpCrop";

            var response = (HttpWebResponse)request.GetResponse();
            var stream = response.GetResponseStream();

            if (stream == null)
            {
                return null;
            }

            var reader = new StreamReader(stream);

            try
            {
                dynamic parsed = JObject.Parse(reader.ReadToEnd());

                var tagName = ((string)parsed.tag_name.ToString()).Replace(".", "");
                var version = (int)(int.Parse(tagName) * Math.Pow(10, Constants.VersionLength - tagName.Length));

                if (version > Constants.Version)
                {
                    return parsed.html_url.ToString();
                }
            }
            catch
            {
                // Ignored
            }

            return null;
        }
    }
}
