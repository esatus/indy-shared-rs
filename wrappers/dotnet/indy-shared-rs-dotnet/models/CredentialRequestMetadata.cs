namespace indy_shared_rs_dotnet.models
{
    public class CredentialRequestMetadata
    {
        public string MsBlindingData { get; set; }
        public string Nonce { get; set; }
        public string MsName { get; set; }
        public uint Handle { get; set; }
    }
}
