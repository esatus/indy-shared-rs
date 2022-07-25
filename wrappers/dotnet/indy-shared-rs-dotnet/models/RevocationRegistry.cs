using Newtonsoft.Json;

namespace indy_shared_rs_dotnet.Models
{
    public class RevocationRegistry
    {
        public uint Handle { get; set; }

        public string JsonString { get; set; }

        [JsonProperty("ver")]
        public string Ver { get; set; }

        [JsonProperty("value")]
        public RevocationRegistryValue Value { get; set; }
    }
}