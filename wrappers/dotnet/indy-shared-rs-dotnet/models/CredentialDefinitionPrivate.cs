using Newtonsoft.Json;

namespace indy_shared_rs_dotnet.Models
{
    public class CredentialDefinitionPrivate
    {
        public uint Handle { get; set; }
        public CredDefPvtValue Value { get; set; }
    }

    public class CredDefPvtValue
    {
        [JsonProperty("p_key")]
        public PKey PKey { get; set; }
        [JsonProperty("r_key")]
        public RKey RKey { get; set; }
    }

    public class PKey
    {
        public string p { get; set; }
        public string q { get; set; }
    }

    public class RKey
    {
        public string x { get; set; }
        public string sk { get; set; }
    }
}
