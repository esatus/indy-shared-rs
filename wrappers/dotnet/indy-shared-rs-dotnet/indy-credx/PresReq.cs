using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.indy_credx
{
    public static class PresReq
    {
        public static Task<string> GenerateNonceAsync()
        {
            string result = "";
            NativeMethods.credx_generate_nonce(ref result);
            return Task.FromResult(result);
        }
    }
}
