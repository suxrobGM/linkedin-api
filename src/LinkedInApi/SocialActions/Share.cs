using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LinkedIn.Api.SocialActions
{
    public partial class Share
    {
        /// <summary>
        /// Unique ID for the share.
        /// Guaranteed in Response: Yes.
        /// Required for Request: No.
        /// </summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        /// <summary>
        /// URN of the activity associated with this share. This value is not present for video shares and historic shares.
        /// Guaranteed in Response: No.
        /// Required for Request: No.
        /// </summary>
        [JsonProperty("activity", NullValueHandling = NullValueHandling.Ignore)]
        public string ActivityUrn { get; set; }

        /// <summary>
        /// Agent is the Sponsored Ad Account that created the Direct Sponsored Content Share on behalf of an organization. This permission has to be delegated. Returns a sponsored Account URN for Direct Sponsored Content shares.
        /// Guaranteed in Response: No.
        /// Required for Request: Yes if only used for direct sponsored content organization share.
        /// </summary>
        [JsonProperty("agent", NullValueHandling = NullValueHandling.Ignore)]
        public string AgentUrn { get; set; }

        /// <summary>
        /// Referenced content such as articles and images.
        /// Guaranteed in Response: No.
        /// Required for Request: Required if text is empty.
        /// </summary>
        [JsonProperty("content", NullValueHandling = NullValueHandling.Ignore)]
        public ShareContent Content { get; set; }

        /// <summary>
        /// Time of creation.
        /// Guaranteed in Response: Yes.
        /// Required for Request: No.
        /// </summary>
        [JsonProperty("created", NullValueHandling = NullValueHandling.Ignore)]
        public AuditStamp Created { get; set; }

        /// <summary>
        /// Distribution target for the share.
        /// Guaranteed in Response: Yes.
        /// Required for Request: Required to set the share as publicly visible. For sponsored content where the targeting is defined when it is sponsored, distribution should be null.
        /// </summary>
        [JsonProperty("distribution", NullValueHandling = NullValueHandling.Ignore)]
        public Distribution Distribution { get; set; }

        /// <summary>
        /// A flag that indicates if this share was edited by a member.
        /// Guaranteed in Response: Yes.
        /// Required for Request: No.
        /// </summary>
        [JsonProperty("edited", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Edited { get; set; }

        /// <summary>
        /// Time of last modification.
        /// Guaranteed in Response: Yes.
        /// Required for Request: No.
        /// </summary>
        [JsonProperty("lastModified", NullValueHandling = NullValueHandling.Ignore)]
        public AuditStamp LastModified { get; set; }

        /// <summary>
        /// Share being reshared.
        /// Guaranteed in Response: No.
        /// Required for Request: Required when resharing. Not allowed otherwise.
        /// </summary>
        [JsonProperty("resharedShare", NullValueHandling = NullValueHandling.Ignore)]
        public string ResharedShareUrn { get; set; }

        /// <summary>
        /// If this share is a reshare, then this is the URN of the original/root share that was reshared.
        /// Guaranteed in Response: No.
        /// Required for Request: Required when resharing. Not allowed otherwise.
        /// </summary>
        [JsonProperty("originalShare", NullValueHandling = NullValueHandling.Ignore)]
        public string OriginalShareUrn { get; set; }

        /// <summary>
        /// URN of the owner of the share.
        /// Guaranteed in Response: Yes.
        /// Required for Request: No.
        /// </summary>
        [JsonProperty("owner", NullValueHandling = NullValueHandling.Ignore)]
        public string OwnerUrn { get; set; }

        /// <summary>
        /// Share subject.
        /// Guaranteed in Response: Yes.
        /// Required for Request: Required for direct sponsored shares.
        /// </summary>
        [JsonProperty("subject", NullValueHandling = NullValueHandling.Ignore)]
        public string Subject { get; set; }

        /// <summary>
        /// Text entered by the member for this share, which may contain annotations.
        /// Guaranteed in Response: Yes.
        /// Required for Request: Required if content is empty.
        /// </summary>
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public ShareText Text { get; set; }
        
        public string ToJson() => JsonConvert.SerializeObject(this, CustomJsonConverter.Settings);
        public static Share FromJson(string json) => JsonConvert.DeserializeObject<Share>(json, CustomJsonConverter.Settings);
    }

    /// <summary>
    /// Share content represents external articles and media referenced in a share. This includes rich media and articles from around the web.
    /// </summary>
    public partial class ShareContent
    {
        public ShareContent()
        {
            ContentEntities = new List<ContentEntity>();
        }

        /// <summary>
        /// Details of content being shared.
        /// Required for Request: No.
        /// </summary>
        [JsonProperty("contentEntities", NullValueHandling = NullValueHandling.Ignore)]
        public List<ContentEntity> ContentEntities { get; set; }

        /// <summary>
        /// Content title. Maximum of 400 characters; recommended length is < 70 characters.
        /// Required for Request: No.
        /// </summary>
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        /// <summary>
        /// Content description. This field is displayed to a small percentage of members on the mobile web version of the site. It is not displayed on the desktop site or native mobile apps. Maximum of 256 characters.
        /// Required for Request: No.
        /// </summary>
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        /// <summary>
        /// The type of media represented by contentEntities. Must correspond to the URN types in contentEntities.
        /// Use static class of <c>ShareMediaCategory</c> to set value.
        /// Required for Request: Optional.
        /// </summary>
        /// <example>share.Content.ShareMediaCategory = ShareMediaCategory.ARTICLE</example>
        [JsonProperty("shareMediaCategory", NullValueHandling = NullValueHandling.Ignore)]
        public string ShareMediaCategory { get; set; }
    }

    public partial class ContentEntity
    {       
        /// <summary>
        /// URN of the content being shared. Typical URN format is urn:li:richMediaSummary:{id}.
        /// Required for Request: Required for rich media shares. Not allowed otherwise.
        /// </summary>
        [JsonProperty("entity", NullValueHandling = NullValueHandling.Ignore)]
        public string EntityUrn { get; set; }

        /// <summary>
        /// URL of the content being shared.
        /// Required for Request: Required for sharing external articles. Not allowed otherwise.
        /// </summary>
        [JsonProperty("entityLocation", NullValueHandling = NullValueHandling.Ignore)]
        public Uri EntityLocation { get; set; }

        [JsonProperty("thumbnails", NullValueHandling = NullValueHandling.Ignore)]
        public List<ShareThumbnails> Thumbnails { get; set; }
    }

    public partial class ShareThumbnails
{
        [JsonProperty("authors", NullValueHandling = NullValueHandling.Ignore)]
        public object[] Authors { get; set; }

        [JsonProperty("imageSpecificContent", NullValueHandling = NullValueHandling.Ignore)]
        public ImageSpecificContent ImageSpecificContent { get; set; }

        [JsonProperty("publishers", NullValueHandling = NullValueHandling.Ignore)]
        public object[] Publishers { get; set; }

        /// <summary>
        /// URL to a thumbnail image to display for the content. Maximum of 2MB thumbnail size.
        /// Required for Request: No. Only allowed for article shares.
        /// </summary>
        [JsonProperty("resolvedUrl", NullValueHandling = NullValueHandling.Ignore)]
        public Uri ResolvedUrl { get; set; }
    }

    public partial class ImageSpecificContent
    {
    }

    public partial class AuditStamp
    {
        [JsonProperty("actor", NullValueHandling = NullValueHandling.Ignore)]
        public string ActorUrn { get; set; }

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

    public partial class ShareText
    {
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Content { get; set; }
    }         
}
