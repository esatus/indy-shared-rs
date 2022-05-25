namespace indy_shared_rs_dotnet.models
{
    public class CredentialOffer
    {
        public string SchemaId { get; set; }
        public string CredentialDefinitionId { get; set; }
        public string KeyCorrectnessProof { get; set; }
        public string Nonce { get; set; }
        public uint Handle { get; set; }
    }
}
