using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkedIn.Api
{
    /// <summary>
    /// Access tokens stay valid until the number of seconds indicated in the expires_in field in the API response. 
    /// You can go through the OAuth flow on multiple clients (browsers or devices) and simultaneously hold multiple valid access tokens as long as the same scope is requested. 
    /// If you request a different scope than the previously granted scope, all the previous access tokens are invalidated.
    /// </summary>
    public class AccessToken
    {
        /// <summary>
        /// The access token for the application. This value must be kept secure as specified in the API Terms of Use
        /// </summary>
        [JsonProperty("access_token", NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; }

        /// <summary>
        /// The number of seconds remaining until the token expires. Currently, all access tokens are issued with a 60 day lifespan.
        /// </summary>
        [JsonProperty("expires_in", NullValueHandling = NullValueHandling.Ignore)]
        public long ExpiresIn { get; set; }

        public DateTime GetExpiryDate() => DateTime.Now.AddSeconds(ExpiresIn);
        public string ToJson() => JsonConvert.SerializeObject(this, CustomJsonConverter.Settings);
        public static AccessToken FromJson(string json) => JsonConvert.DeserializeObject<AccessToken>(json, CustomJsonConverter.Settings);
        public static implicit operator AccessToken(string token) => new AccessToken() { Token = token };
    }
}
