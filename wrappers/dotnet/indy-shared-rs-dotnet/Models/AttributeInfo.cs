using Newtonsoft.Json;
using System.Collections.Generic;

namespace indy_shared_rs_dotnet.Models
{
    public class AttributeInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("names")]
        public List<string> Names { get; set; }
        //TODO: Implement Query and AbstractQuery
        //public Query Restrictions { get; set; }
        [JsonProperty("non_revoked")]
        public NonRevokedInterval NonRevoked { get; set; }

        public AttributeInfo(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
