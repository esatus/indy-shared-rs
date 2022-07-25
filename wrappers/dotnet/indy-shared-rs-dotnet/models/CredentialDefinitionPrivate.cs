namespace indy_shared_rs_dotnet.Models
{
    public class CredentialDefinitionPrivate
    {
        public uint Handle { get; set; }
        public string JsonString { get; set; }
        public CredDefPvtValue Value { get; set; }
    }
}