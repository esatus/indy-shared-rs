using Newtonsoft.Json;

namespace indy_shared_rs_dotnet.Models
{
    public class CredentialRequestMetadata
    {
        [JsonProperty("master_secret_blinding_data")]
        public MasterSecretBlindingData MsBlindingData { get; set; }
        public string Nonce { get; set; }

        [JsonProperty("master_secret_name")]
        public string MsName { get; set; }
        public uint Handle { get; set; }
    }
}