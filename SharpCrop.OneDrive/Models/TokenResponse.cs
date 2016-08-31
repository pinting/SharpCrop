using Newtonsoft.Json;

namespace SharpCrop.OneDrive.Models
{
    /// <summary>
    /// OneDrive internal response object which holds the AccessToken. This is serialized
    /// into the application settings as one Token string.
    /// </summary>
    public class TokenResponse
    {
        [JsonProperty(PropertyName = "token_type")]
        public string TokenType;

        [JsonProperty(PropertyName = "expires_in")]
        public string ExpiresIn;

        [JsonProperty(PropertyName = "scope")]
        public string Scope;

        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken;

        [JsonProperty(PropertyName = "user_id")]
        public string UserId;
    }
}
