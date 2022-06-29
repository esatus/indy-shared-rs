using Newtonsoft.Json;

namespace indy_shared_rs_dotnet.Models
{
    public class RevocationRegistryDefinitionPrivate
    {
        public uint Handle { get; set; }

        [JsonProperty("value")]
        public RevocationRegistryDefinitionPrivateValue Value { get; set; }
    }
}
