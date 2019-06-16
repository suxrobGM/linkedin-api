using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkedIn.Api.SocialActions
{
    public partial class RichMedia
    {
        public RichMedia()
        {
            MediaThumbnails = new List<Media>();
        }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("thumbnails", NullValueHandling = NullValueHandling.Ignore)]
        public List<Media> MediaThumbnails { get; set; }

        [JsonProperty("location", NullValueHandling = NullValueHandling.Ignore)]
        public string LocationUrn { get; set; }

        public string ToJson() => JsonConvert.SerializeObject(this, CustomJsonConverter.Settings);
        public static RichMedia FromJson(string json) => JsonConvert.DeserializeObject<RichMedia>(json, CustomJsonConverter.Settings);
    }
}
