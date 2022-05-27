using System.Collections.Generic;

namespace indy_shared_rs_dotnet.models
{
    public class Credential
    {
        public string SchemaId { get; set; }
        public string CredentialDefinitionId { get; set; }
        public string RevocationRegistryId { get; set; } = null;
        public Dictionary<string, AttributeValue> Values { get; set; }
        public string Signature { get; set; }
        public string SignatureCorrectnessProof { get; set; }
        public RevocationRegistry RevocationRegistry { get; set; } = null;
        public string Witness { get; set; } = null;
    }

    public class AttributeValue
    {
        public string raw { get; set; }
        public string encoded { get; set; }
    }
}
