using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkedIn.Api.SocialActions
{
    public partial class RichMedia
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("thumbnails", NullValueHandling = NullValueHandling.Ignore)]
        public Thumbnails<Media> Medias { get; set; }

        [JsonProperty("location", NullValueHandling = NullValueHandling.Ignore)]
        public string LocationUrn { get; set; }
    }
}
