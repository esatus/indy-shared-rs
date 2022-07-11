using indy_shared_rs_dotnet.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.IndyCredx
{
    public static class MasterSecretApi
    {
        /// <summary>
        /// Creates a new <see cref="MasterSecret"/>.
        /// </summary>
        /// <exception cref="SharedRsException">Throws when <see cref="MasterSecret"/> can't be created.</exception>
        /// <returns>New <see cref="MasterSecret"/>.</returns>
        public static async Task<MasterSecret> CreateMasterSecretAsync()
        {
            uint result = 0;
            int errorCode = NativeMethods.credx_create_master_secret(ref result);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            string masterSecretJson = await ObjectApi.ToJsonAsync(result);
            MasterSecret msObject = JsonConvert.DeserializeObject<MasterSecret>(masterSecretJson, Settings.JsonSettings);
            msObject.Handle = result;
            return await Task.FromResult(msObject);
        }

    }
}
