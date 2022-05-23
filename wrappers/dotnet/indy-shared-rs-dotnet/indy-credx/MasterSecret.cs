using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.indy_credx
{
    public static class MasterSecret
    {
        public static Task<uint> CreateMasterSecret()
        {
            uint result = 0;
            NativeMethods.credx_create_master_secret(ref result);
            return Task.FromResult(result);
        }
        
    }
}
