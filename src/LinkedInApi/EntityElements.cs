using Newtonsoft.Json;

namespace LinkedIn.Api
{
    public partial class EntityElements<T> where T: class
    {
        [JsonProperty("paging", NullValueHandling = NullValueHandling.Ignore)]
        public Paging Paging { get; set; }

        [JsonProperty("elements", NullValueHandling = NullValueHandling.Ignore)]
        public T[] Elements { get; set; }

        public static EntityElements<T> FromJson(string json) => JsonConvert.DeserializeObject<EntityElements<T>>(json, CustomJsonConverter.Settings);

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, CustomJsonConverter.Settings);
        }
    }          
}
