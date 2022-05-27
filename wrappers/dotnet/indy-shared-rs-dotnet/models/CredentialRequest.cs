namespace indy_shared_rs_dotnet.models
{
    public class CredentialRequest
    {
        public DidValue ProverDid { get; set; }
        public string CredentialDefinitionId { get; set; }
        public string BlindedMs { get; set; }
        public string BlindedMsCorrectnessProof { get; set; }
        public string Nonce { get; set; }
        public string MethodName { get; set; }
        public uint Handle { get; set; }
    }
}
