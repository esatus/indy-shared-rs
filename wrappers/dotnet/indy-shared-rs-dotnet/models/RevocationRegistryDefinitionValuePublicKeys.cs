using Newtonsoft.Json;

namespace indy_shared_rs_dotnet.models
{
    public class RevocationRegistryDefinitionValuePublicKeys
    {
        [JsonProperty("accumKey")]
        public AccumKey AccumKey { get; set; }
    }
}
