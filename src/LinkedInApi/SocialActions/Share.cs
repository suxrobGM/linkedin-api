using Newtonsoft.Json;
using System;

namespace LinkedIn.Api.SocialActions
{
    public partial class Share
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("activity", NullValueHandling = NullValueHandling.Ignore)]
        public string Activity { get; set; }

        [JsonProperty("content", NullValueHandling = NullValueHandling.Ignore)]
        public Content Content { get; set; }

        [JsonProperty("created", NullValueHandling = NullValueHandling.Ignore)]
        public Created Created { get; set; }

        [JsonProperty("distribution", NullValueHandling = NullValueHandling.Ignore)]
        public Distribution Distribution { get; set; }

        [JsonProperty("edited", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Edited { get; set; }       

        [JsonProperty("lastModified", NullValueHandling = NullValueHandling.Ignore)]
        public Created LastModified { get; set; }

        [JsonProperty("owner", NullValueHandling = NullValueHandling.Ignore)]
        public string Owner { get; set; }

        [JsonProperty("subject", NullValueHandling = NullValueHandling.Ignore)]
        public string Subject { get; set; }

        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public Text Text { get; set; }
    }

    public partial class Content
    {
        [JsonProperty("contentEntities", NullValueHandling = NullValueHandling.Ignore)]
        public ContentEntity[] ContentEntities { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }
    }

    public partial class ContentEntity
    {
        [JsonProperty("entityLocation", NullValueHandling = NullValueHandling.Ignore)]
        public Uri EntityLocation { get; set; }

        [JsonProperty("thumbnails", NullValueHandling = NullValueHandling.Ignore)]
        public Thumbnail[] Thumbnails { get; set; }
    }

    public partial class Thumbnail
    {
        [JsonProperty("authors", NullValueHandling = NullValueHandling.Ignore)]
        public object[] Authors { get; set; }

        [JsonProperty("imageSpecificContent", NullValueHandling = NullValueHandling.Ignore)]
        public ImageSpecificContent ImageSpecificContent { get; set; }

        [JsonProperty("publishers", NullValueHandling = NullValueHandling.Ignore)]
        public object[] Publishers { get; set; }

        [JsonProperty("resolvedUrl", NullValueHandling = NullValueHandling.Ignore)]
        public Uri ResolvedUrl { get; set; }
    }

    public partial class ImageSpecificContent
    {
    }

    public partial class Created
    {
        [JsonProperty("actor", NullValueHandling = NullValueHandling.Ignore)]
        public string Actor { get; set; }

        [JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
        public long? Time { get; set; }
    }

    public partial class Distribution
    {
        [JsonProperty("linkedInDistributionTarget", NullValueHandling = NullValueHandling.Ignore)]
        public LinkedInDistributionTarget LinkedInDistributionTarget { get; set; }
    }

    public partial class LinkedInDistributionTarget
    {
        [JsonProperty("visibleToGuest", NullValueHandling = NullValueHandling.Ignore)]
        public bool? VisibleToGuest { get; set; }
    }

    public partial class Text
    {
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Content { get; set; }
    }

    public partial class Share
    {
        public static Share FromJson(string json) => JsonConvert.DeserializeObject<Share>(json, CustomConverter.Settings);
    }

    public static class ShareSerialize
    {
        public static string ToJson(this Share self) => JsonConvert.SerializeObject(self, CustomConverter.Settings);
    }    
}
