using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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

        public string ToRequestJson()
        {           
            var requestShare = new Share()
            {
                Content = this.Content,
                Distribution = this.Distribution,
                Owner = this.Owner,
                Subject = this.Subject,
                Text = this.Text
            };
            return requestShare.ToJson();         
        }
    }

    public partial class Content
    {
        public Content()
        {
            ContentEntities = new List<ContentEntity>();
        }

        [JsonProperty("contentEntities", NullValueHandling = NullValueHandling.Ignore)]
        public List<ContentEntity> ContentEntities { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }
    }

    public partial class ContentEntity
    {
        public ContentEntity()
        {
            Thumbnails = new List<Thumbnail>();
        }

        [JsonProperty("entityLocation", NullValueHandling = NullValueHandling.Ignore)]
        public Uri EntityLocation { get; set; }

        [JsonProperty("thumbnails", NullValueHandling = NullValueHandling.Ignore)]
        public List<Thumbnail> Thumbnails { get; set; }
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
        public static Share FromJson(string json) => JsonConvert.DeserializeObject<Share>(json, CustomJsonConverter.Settings);
    }

    public static class ShareSerialize
    {
        public static string ToJson(this Share self) => JsonConvert.SerializeObject(self, CustomJsonConverter.Settings);
    }    
}
