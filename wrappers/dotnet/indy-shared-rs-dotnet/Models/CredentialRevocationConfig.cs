using System.Collections.Generic;

namespace indy_shared_rs_dotnet.Models
{
    public class CredentialRevocationConfig
    {
        public uint RevRegDefObjectHandle;
        public uint RevRegDefPvtObjectHandle;
        public uint RevRegObjectHandle;
        public long RegIdx;
        public List<long> RegUsed;
        public string TailsPath;
    }
}