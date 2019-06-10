using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkedIn.Api.Organizations
{
    public partial class OrganizationalAccess
    {
        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }

        [JsonProperty("role", NullValueHandling = NullValueHandling.Ignore)]
        public string Role { get; set; }

        [JsonProperty("roleAssignee", NullValueHandling = NullValueHandling.Ignore)]
        public string RoleAssignee { get; set; }

        [JsonProperty("organizationalTarget", NullValueHandling = NullValueHandling.Ignore)]
        public string OrganizationalTarget { get; set; }
    }

    public partial class OrganizationalAccess
    {
        public static OrganizationalAccess FromJson(string json) => JsonConvert.DeserializeObject<OrganizationalAccess>(json, CustomJsonConverter.Settings);
    }

    public static class OrganizationalAccessSerialize
    {
        public static string ToJson(this OrganizationalAccess self) => JsonConvert.SerializeObject(self, CustomJsonConverter.Settings);
    }
}
