using indy_shared_rs_dotnet.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.indy_credx
{
    public class Schema
    {
        public static Task<uint> CreateSchema(string originDid, string schemaName, string schemaVersion, string[] attrNames, uint seqNo)
        {
            uint result = 0;
            NativeMethods.credx_create_schema(originDid, schemaName, schemaVersion, attrNames, seqNo, ref result);
            return Task.FromResult(result);
        }
    }
}
