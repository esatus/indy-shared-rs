using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.indy_credx
{
    public static class ModApi
    {
        public static async Task SetDefaultLogger()
        {
            int errorCode = NativeMethods.credx_set_default_logger();
            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }
        }

        public static Task<string> GetVersionAsync()
        {
            string result = NativeMethods.credx_version();
            return Task.FromResult(result);
        }
    }
}
