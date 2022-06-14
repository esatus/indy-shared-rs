using Newtonsoft.Json;
using System.Collections.Generic;

namespace indy_shared_rs_dotnet.Models
{
    public class Credential
    {
        [JsonProperty("schema_id")]
        public string SchemaId { get; set; }

        [JsonProperty("cred_def_id")]
        public string CredentialDefinitionId { get; set; }

        [JsonProperty("rev_reg_id")]
        public string RevocationRegistryId { get; set; } = null;
        public Dictionary<string, AttributeValue> Values { get; set; }

        [JsonProperty("signature")]
        public CredentialSignature Signature { get; set; }

        [JsonProperty("signature_correctness_proof")]
        public SignatureCorrectnessProofValue SignatureCorrectnessProof { get; set; }

        [JsonProperty("rev_reg")]
        public RevocationRegistryValue RevocationRegistry { get; set; } = null;
        
        [JsonProperty("witness")]
        public Witness Witness { get; set; } = null;
        public uint Handle { get; set; }
    }
    public class CredentialSignature
    {
        [JsonProperty("p_credential")]
        public PrimaryCredentialSignature PCredential { get; set; }
        [JsonProperty("r_credential")]
        public NonRevocationCredentialSignature RCredential { get; set; }
    }

    public class PrimaryCredentialSignature
    {
        [JsonProperty("m_2")]
        public string M2 { get; set; }

        [JsonProperty("a")]
        public string A { get; set; }

        [JsonProperty("e")]
        public string E { get; set; }

        [JsonProperty("v")]
        public string V { get; set; }
    }

    public class NonRevocationCredentialSignature
    {
        [JsonProperty("sigma")]
        public string Sigma { get; set; }

        [JsonProperty("c")]
        public string C { get; set; }

        [JsonProperty("vr_prime_prime")]
        public string VrPrimePrime { get; set; }

        [JsonProperty("witness_signature")]
        public WitnessSignature WitnessSignature { get; set; }

        [JsonProperty("g_i")]
        public string Gi { get; set; }

        [JsonProperty("i")]
        public uint I { get; set; }

        [JsonProperty("m2")]
        public string M2 { get; set; }
    }

    public class Witness
    {
        [JsonProperty("omega")]
        public string omega { get; set; }
    }

    public class WitnessSignature
    {
        [JsonProperty("sigma_i")]
        public string SigmaI { get; set; }

        [JsonProperty("u_i")]
        public string Ui { get; set; }

        [JsonProperty("g_i")]
        public string Gi { get; set; }
    }
}
