using indy_shared_rs_dotnet.models;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.indy_credx
{
    public static class MasterSecretApi
    {
        public static async Task<MasterSecret> CreateMasterSecretAsync()
        {
            uint result = 0;
            NativeMethods.credx_create_master_secret(ref result);
            IndyObject indyObj = new(result);
            MasterSecret msObject = JsonConvert.DeserializeObject<MasterSecret>(await indyObj.toJson());
            msObject.Handle = result;
            return await Task.FromResult(msObject);
        }
        
    }
}
