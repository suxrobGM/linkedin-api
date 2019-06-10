using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkedIn.Api.Organizations
{
    public partial class Organization
    {
        [JsonProperty("$URN", NullValueHandling = NullValueHandling.Ignore)]
        public string Urn { get; set; }

        [JsonProperty("alternativeNames", NullValueHandling = NullValueHandling.Ignore)]
        public object[] AlternativeNames { get; set; }

        [JsonProperty("autoCreated", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AutoCreated { get; set; }

        [JsonProperty("defaultLocale", NullValueHandling = NullValueHandling.Ignore)]
        public Locale DefaultLocale { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public Description Description { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("industries", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Industries { get; set; }

        [JsonProperty("localizedDescription", NullValueHandling = NullValueHandling.Ignore)]
        public string LocalizedDescription { get; set; }

        [JsonProperty("localizedName", NullValueHandling = NullValueHandling.Ignore)]
        public string LocalizedName { get; set; }

        [JsonProperty("localizedSpecialties", NullValueHandling = NullValueHandling.Ignore)]
        public object[] LocalizedSpecialties { get; set; }

        [JsonProperty("localizedWebsite", NullValueHandling = NullValueHandling.Ignore)]
        public Uri LocalizedWebsite { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public Description Name { get; set; }

        [JsonProperty("parentRelationship", NullValueHandling = NullValueHandling.Ignore)]
        public ParentRelationship ParentRelationship { get; set; }

        [JsonProperty("specialties", NullValueHandling = NullValueHandling.Ignore)]
        public object[] Specialties { get; set; }

        [JsonProperty("vanityName", NullValueHandling = NullValueHandling.Ignore)]
        public string VanityName { get; set; }

        [JsonProperty("versionTag", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringConverter))]
        public long? VersionTag { get; set; }

        [JsonProperty("website", NullValueHandling = NullValueHandling.Ignore)]
        public Description Website { get; set; }
    }

    public partial class Locale
    {
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }

        [JsonProperty("language", NullValueHandling = NullValueHandling.Ignore)]
        public string Language { get; set; }
    }

    public partial class Description
    {
        [JsonProperty("localized", NullValueHandling = NullValueHandling.Ignore)]
        public Localized Localized { get; set; }

        [JsonProperty("preferredLocale", NullValueHandling = NullValueHandling.Ignore)]
        public Locale PreferredLocale { get; set; }
    }

    public partial class Localized
    {
        [JsonProperty("en_US", NullValueHandling = NullValueHandling.Ignore)]
        public string EnUs { get; set; }
    }

    public partial class ParentRelationship
    {
        [JsonProperty("parent", NullValueHandling = NullValueHandling.Ignore)]
        public string Parent { get; set; }
    }

    public partial class Organization
    {
        public static Organization FromJson(string json) => JsonConvert.DeserializeObject<Organization>(json, CustomConverter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Organization self) => JsonConvert.SerializeObject(self, CustomConverter.Settings);
    }   

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}
