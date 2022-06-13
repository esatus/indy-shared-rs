using indy_shared_rs_dotnet.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.indy_credx
{
    public static class MasterSecretApi
    {
        public static async Task<MasterSecret> CreateMasterSecretAsync()
        {
            uint result = 0;
            int errorCode = NativeMethods.credx_create_master_secret(ref result);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw new SharedRsException(JsonConvert.DeserializeObject<Dictionary<string, string>>(error)["message"]);
            }

            string masterSecretJson = await ObjectApi.ToJson(result);
            MasterSecret msObject = JsonConvert.DeserializeObject<MasterSecret>(masterSecretJson, Settings.jsonSettings);
            msObject.Handle = result;
            return await Task.FromResult(msObject);
        }
        
    }
}
