using Newtonsoft.Json;

namespace indy_shared_rs_dotnet.Models
{
    public class SignatureCorrectnessProofValue
    {
        [JsonProperty("se")]
        public string se { get; set; }

        [JsonProperty("c")]
        public string c { get; set; }
    }
}
