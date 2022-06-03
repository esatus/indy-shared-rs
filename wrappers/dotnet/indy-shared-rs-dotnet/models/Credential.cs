using System.Collections.Generic;

namespace indy_shared_rs_dotnet.Models
{
    public class Credential
    {
        public string SchemaId { get; set; }
        public string CredentialDefinitionId { get; set; }
        public string RevocationRegistryId { get; set; } = null;
        public Dictionary<string, AttributeValue> Values { get; set; }
        public string Signature { get; set; }
        public string SignatureCorrectnessProof { get; set; }
        public RevocationRegistryDefinition RevocationRegistry { get; set; } = null;
        public string Witness { get; set; } = null;
        public uint Handle { get; set; }
    }
}
