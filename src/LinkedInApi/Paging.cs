using Newtonsoft.Json;

namespace LinkedIn.Api
{
    public partial class Paging
    {
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public long? Count { get; set; }

        [JsonProperty("links", NullValueHandling = NullValueHandling.Ignore)]
        public object[] Links { get; set; }

        [JsonProperty("start", NullValueHandling = NullValueHandling.Ignore)]
        public long? Start { get; set; }

        [JsonProperty("total", NullValueHandling = NullValueHandling.Ignore)]
        public long? Total { get; set; }
    }

    public partial class Paging
    {
        public static Paging FromJson(string json) => JsonConvert.DeserializeObject<Paging>(json, CustomJsonConverter.Settings);
    }

    public static class PagingSerialize
    {
        public static string ToJson(this Paging self) => JsonConvert.SerializeObject(self, CustomJsonConverter.Settings);
    }
}
