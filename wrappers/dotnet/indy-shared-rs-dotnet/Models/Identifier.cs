using System;

namespace indy_shared_rs_dotnet.Models
{
    public class Identifier
    {
        public string SchemaId { get; set; }
        public string CredentialDefinitionId { get; set; }
        public string RevocationRegistryId { get; set; }
        public uint Timestamp { get; set; } 
    }
}
