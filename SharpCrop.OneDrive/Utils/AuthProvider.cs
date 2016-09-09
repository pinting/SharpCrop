using Microsoft.Graph;
using Newtonsoft.Json;
using SharpCrop.OneDrive.Models;
using SharpCrop.Provider.Utils;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable UseStringInterpolation

namespace SharpCrop.OneDrive.Utils
{
    /// <summary>
    /// Custom written OneDrive authentication provider which signs every outgoing request with
    /// the AccessToken. The user needs to copy the API code into the application manually - so
    /// this is compatible with Mono.
    /// </summary>
    public class AuthProvider : IAuthenticationProvider
    {
        public TokenResponse Session;

        /// <summary>
        /// Construct the authorization URL.
        /// </summary>
        public string Url => string.Format(
            "https://login.live.com/oauth20_authorize.srf?client_id={0}&scope={1}&response_type=code&redirect_uri={2}",
            Obscure.Decode(Constants.AppKey),
            string.Join("+", Constants.Scopes),
            Constants.RedirectUrl);

        /// <summary>
        /// Process a code and obtain a TokenResponse.
        /// </summary>
        /// <param name="code"></param>
        public void ProcessCode(string code)
        {
            // The redirect_uri redirects the user to the project Github Page where the OneDrive.html
            // writes the API code to the document.body from the URL. This is needed, because OneDrive
            // does not support a token request without a callback URL - like Google Drive and Dropbox.
            var request = WebRequest.Create("https://login.live.com/oauth20_token.srf");
            var array = Encoding.UTF8.GetBytes(string.Format(
                "client_id={0}&client_secret={1}&redirect_uri={2}&code={3}&grant_type=authorization_code",
                Obscure.Decode(Constants.AppKey),
                Obscure.Decode(Constants.AppSecret),
                Constants.RedirectUrl,
                code));

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = array.Length;
            request.Method = "POST";

            var stream = request.GetRequestStream();

            stream.Write(array, 0, array.Length);
            stream.Close();

            var response = request.GetResponse();

            stream = response.GetResponseStream();

            if (stream != null)
            {
                var reader = new StreamReader(stream);
                var responseData = reader.ReadToEnd();

                Session = JsonConvert.DeserializeObject<TokenResponse>(responseData);

                reader.Close();
                stream.Close();
            }
            
            response.Close();
        }

        /// <summary>
        /// Sign a request with the AccessToken.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            if(Session != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(Session.TokenType, Session.AccessToken);
            }

            return Task.Delay(0);
        }
    }
}
