using indy_shared_rs_dotnet.models;
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
        public Query Restrictions { get; set; }
        public NonRevokedInterval NonRevoked { get; set; }

        public AttributeInfo()
        {
            Restrictions = new Query();
        }
    }
}
