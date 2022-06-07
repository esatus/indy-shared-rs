using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace indy_shared_rs_dotnet.Models
{
    public class CredentialRequest
    {
        [JsonProperty("prover_did")]
        public string ProverDid { get; set; }

        [JsonProperty("cred_def_id")]
        public string CredentialDefinitionId { get; set; }

        [JsonProperty("blinded_ms")]
        public BlindedMs BlindedMs { get; set; }

        [JsonProperty("blinded_ms_correctness_proof")]
        public BlindingMsCorrectnessProof BlindedMsCorrectnessProof { get; set; }
        public string Nonce { get; set; }
        public string MethodName { get; set; }
        public uint Handle { get; set; }
    }

    public class BlindingMsCorrectnessProof
    {
        public string C { get; set; }

        [JsonProperty("v_dash_cap")]
        public string VDashCap { get; set; }

        [JsonProperty("m_caps")]
        public MCaps MCaps { get; set; }

        [JsonProperty("r_caps")]
        public JObject RCaps { get; set; }
    }

    public class BlindedMs
    {
        public string U { get; set; }

        public string Ur { get; set; }

        [JsonProperty("hidden_attributes")]
        public List<string> HiddenAttributes { get; set; }

        [JsonProperty("committed_attributes")]
        public JObject ComittedAttributes { get; set; }
    }

    public class MCaps
    {
        [JsonProperty("master_secret")]
        public string MasterSecret { get; set; }
    }
}
