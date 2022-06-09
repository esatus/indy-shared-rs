using System.Collections.Generic;
using static indy_shared_rs_dotnet.Models.Structures;

namespace indy_shared_rs_dotnet.Models
{
    public class CredentialRevocationInfo
    {
        public uint regDefObjectHandle;
        public uint regDefPvtObjectHandle;
        public uint registryObjectHandle;
        public long regIdx;
        public List<long> regUsed;
        public string tailsPath;
    }
}
