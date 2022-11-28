using indy_shared_rs_dotnet.IndyCredx;
using System;
using static indy_shared_rs_dotnet.Models.Structures;

namespace indy_shared_rs_dotnet.Models
{
    public class CredentialEntry
    {
        public IntPtr CredentialObjectHandle;
        public long Timestamp;
        public IntPtr RevStateObjectHandle;

        /** Timestamp and revocationStateObject are optional parameters. Either timestamp and revocation state must be presented, or neither.
         * ´param name="credentialObject" : Credential object
         *  param name="timestamp" : Value of -1 corresponds to None value
         *  param name="revocationStateObject" : CredentialRevocationState object
         **/
        public static CredentialEntry CreateCredentialEntry(Credential credentialObject, long timestamp = -1, CredentialRevocationState revocationStateObject = null)
        {
            CredentialEntry entry = new CredentialEntry();
            entry.CredentialObjectHandle = credentialObject.Handle;
            if (timestamp == 0 || revocationStateObject == null)
            {
                entry.Timestamp = -1;
                entry.RevStateObjectHandle = new IntPtr();
            }
            if (revocationStateObject != null)
            {
                entry.Timestamp = timestamp;
                entry.RevStateObjectHandle = revocationStateObject.Handle;
            }
            return entry;
        }

        public static CredentialEntry CreateCredentialEntryJson(string credentialJson, long timestamp = -1, string revocationStateJson = null)
        {
            CredentialEntry entry = new CredentialEntry();
            IntPtr credObjectHandle = new IntPtr();
            IntPtr revStateObjectHandle = new IntPtr();

            _ = NativeMethods.credx_credential_from_json(ByteBuffer.Create(credentialJson), ref credObjectHandle);
            entry.CredentialObjectHandle = credObjectHandle;

            if (timestamp == 0 || revocationStateJson == null)
            {
                entry.Timestamp = -1;
                entry.RevStateObjectHandle = new IntPtr();
            }
            if (revocationStateJson != null)
            {
                entry.Timestamp = timestamp;
                _ = NativeMethods.credx_revocation_state_from_json(ByteBuffer.Create(revocationStateJson), ref revStateObjectHandle);
                entry.RevStateObjectHandle = revStateObjectHandle;
            }
            return entry;
        }
    }
}