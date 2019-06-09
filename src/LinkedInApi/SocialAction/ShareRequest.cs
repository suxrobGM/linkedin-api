using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace LinkedIn.Api.SocialAction
{
    public partial class ShareRequest
    {
        [JsonProperty("content")]
        public Content Content { get; set; }

        [JsonProperty("distribution")]
        public Distribution Distribution { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("text")]
        public Text Text { get; set; }
    }

    public partial class Content
    {
        [JsonProperty("contentEntities")]
        public ContentEntity[] ContentEntities { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public partial class ContentEntity
    {
        [JsonProperty("entityLocation")]
        public Uri EntityLocation { get; set; }

        [JsonProperty("thumbnails")]
        public Thumbnail[] Thumbnails { get; set; }
    }

    public partial class Thumbnail
    {
        [JsonProperty("resolvedUrl")]
        public Uri ResolvedUrl { get; set; }
    }

    public partial class Distribution
    {
        [JsonProperty("linkedInDistributionTarget")]
        public LinkedInDistributionTarget LinkedInDistributionTarget { get; set; }
    }

    public partial class LinkedInDistributionTarget
    {
    }

    public partial class Text
    {
        [JsonProperty("text")]
        public string TextContent { get; set; }
    }

    public partial class ShareRequest
    {
        public static ShareRequest FromJson(string json) => JsonConvert.DeserializeObject<ShareRequest>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this ShareRequest self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
