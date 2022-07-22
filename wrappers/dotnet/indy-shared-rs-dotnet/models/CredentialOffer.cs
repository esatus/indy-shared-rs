using Newtonsoft.Json;
using System;

namespace indy_shared_rs_dotnet.Models
{
    public class CredentialOffer
    {
        [JsonProperty("schema_id")]
        public string SchemaId { get; set; }

        [JsonProperty("cred_def_id")]
        public string CredentialDefinitionId { get; set; }

        [JsonProperty("key_correctness_proof")]
        public CredentialKeyCorrectnessProof KeyCorrectnessProof { get; set; }
        public string Nonce { get; set; }

        [JsonIgnore]
        public IntPtr Handle { get; set; }
    }
}