using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace indy_shared_rs_dotnet.Models
{
    public class AttributeFilter
    {
        [JsonProperty("schema_id", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("schema_id")]
        public string SchemaId { get; set; }

        [JsonProperty("schema_issuer_did", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("schema_issuer_did")]
        public string SchemaIssuerDid { get; set; }

        [JsonProperty("schema_name", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("schema_name")]
        public string SchemaName { get; set; }

        [JsonProperty("schema_version", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("schema_version")]
        public string SchemaVersion { get; set; }

        [JsonProperty("issuer_did", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("issuer_did")]
        public string IssuerDid { get; set; }

        [JsonProperty("cred_def_id", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("cred_def_id")]
        public string CredentialDefinitionId { get; set; }

        public AttributeValue AttributeValue { get; set; }
    }
}