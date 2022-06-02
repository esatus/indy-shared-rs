using Newtonsoft.Json;
using System.Collections.Generic;

namespace indy_shared_rs_dotnet.Models
{
    public class CredentialKeyCorrectnessProof
    {
        [JsonIgnore]
        public uint Handle { get; set; }
        public string c { get; set; }

        [JsonProperty("xz_cap")]
        public string xzcap { get; set; }

        [JsonIgnore]
        public List<KeyProofAttributeValue> xrcap { get; set; }
    }

    public class KeyProofAttributeValue
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public KeyProofAttributeValue(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
