using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkedIn.Api
{
    public class AccessToken
    {
        [JsonProperty("access_token", NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; }

        [JsonProperty("expires_in", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime ExpiresIn { get; set; }

        public string ToJson() => JsonConvert.SerializeObject(this, CustomJsonConverter.Settings);
        public static AccessToken FromJson(string json) => JsonConvert.DeserializeObject<AccessToken>(json, CustomJsonConverter.Settings);
        public static implicit operator AccessToken(string token) => new AccessToken() { Token = token };
    }
}
