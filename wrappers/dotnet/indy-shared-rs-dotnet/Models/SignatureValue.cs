using Newtonsoft.Json;

namespace indy_shared_rs_dotnet.Models
{
    public class SignatureValue
    {
        [JsonProperty("m_2")]
        public string m2 { get; set; }

        [JsonProperty("a")]
        public string a { get; set; }

        [JsonProperty("e")]
        public string e { get; set; }

        [JsonProperty("v")]
        public string v { get; set; }
    }
}
