using Newtonsoft.Json;
using System.Collections.Generic;

namespace indy_shared_rs_dotnet.Models
{
    public class CredentialDefinitionData
    {
        public Primary Primary { get; set; }
        public Revocation Revocation { get; set; }
    }

    public class Primary
    {
        public string n { get; set; }
        public string s { get; set; }

        [JsonIgnore]
        public List<KeyProofAttributeValue> r { get; set; }
        public string rctxt { get; set; }
        public string z { get; set; }
    }

    public class Revocation
    {
        public string g { get; set; }

        [JsonProperty("g_dash")]
        public string gdash { get; set; }
        public string h { get; set; }
        public string h0 { get; set; }
        public string h1 { get; set; }
        public string h2 { get; set; }
        public string htilde { get; set; }

        [JsonProperty("h_cap")]
        public string hcap { get; set; }
        public string u { get; set; }
        public string pk { get; set; }
        public string y { get; set; }
    }
}
