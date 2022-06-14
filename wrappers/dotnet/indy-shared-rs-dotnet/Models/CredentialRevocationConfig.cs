using System.Collections.Generic;

namespace indy_shared_rs_dotnet.Models
{
    public class CredentialRevocationConfig
    {
        public uint revRegDefObjectHandle;
        public uint revRegDefPvtObjectHandle;
        public uint revRegObjectHandle;
        public long regIdx;
        public List<long> regUsed;
        public string tailsPath;
    }
}
