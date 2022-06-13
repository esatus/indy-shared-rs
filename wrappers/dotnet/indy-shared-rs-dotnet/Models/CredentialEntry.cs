using System;

namespace indy_shared_rs_dotnet.Models
{
    public class CredentialEntry
    {
        public uint CredentialObjectHandle;
        public long Timestamp;
        public uint RevStateObjectHandle;

        public CredentialEntry (Credential credentialObject, RevocationRegistry revocationObject)
        {
            CredentialObjectHandle = credentialObject.Handle;
            Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            if (revocationObject != null)
            {
                RevStateObjectHandle = revocationObject.Handle;
            }
            else RevStateObjectHandle = 0;
        }
    }
}
