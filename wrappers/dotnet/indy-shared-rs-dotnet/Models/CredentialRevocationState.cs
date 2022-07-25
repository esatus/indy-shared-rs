using Newtonsoft.Json;

namespace indy_shared_rs_dotnet.Models
{
    public class CredentialRevocationState
    {
        public uint Handle { get; set; }

        public string JsonString { get; set; }

        [JsonProperty("witness")]
        public Witness Witness { get; set; }

        [JsonProperty("rev_reg")]
        public RevocationRegistryValue CryptoRevocationRegistry { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }
}