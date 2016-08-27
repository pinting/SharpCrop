using Newtonsoft.Json;

namespace SharpCrop.OneDrive.Models
{
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
