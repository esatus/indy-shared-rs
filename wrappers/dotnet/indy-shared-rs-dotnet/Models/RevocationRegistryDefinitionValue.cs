using Newtonsoft.Json;

namespace indy_shared_rs_dotnet.Models
{
    public class RevocationRegistryDefinitionValue
    {
        [JsonProperty("issuanceType")]
        public string IssuanceType { get; set; } = IssuerType.ISSUANCE_BY_DEFAULT.ToString();
        [JsonProperty("maxCredNum")]
        public int MaxCredNum { get; set; } = 1;
        [JsonProperty("publicKeys")]
        public RevocationRegistryDefinitionValuePublicKeys PublicKeys { get; set; }
        [JsonProperty("tailsHash")]
        public string TailsHash { get; set; }
        [JsonProperty("tailsLocation")]
        public string TailsLocation { get; set; }
    }

    public class RevocationRegistryDefinitionValuePublicKeys
    {
        [JsonProperty("accumKey")]
        //public Dictionary<string,string> AccumKey { get; set; }
        public AccumKey AccumKey { get; set; }
    }

    public class AccumKey
    {
        [JsonProperty("z")]
        public string Z { get; set; }
    }
}
