using indy_shared_rs_dotnet.Models;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.Models.Structures;

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
            IntPtr result = new IntPtr();
            int errorCode = NativeMethods.credx_create_master_secret(ref result);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            string masterSecretJson = await ObjectApi.ToJsonAsync(result);
            MasterSecret msObject = JsonConvert.DeserializeObject<MasterSecret>(masterSecretJson, Settings.JsonSettings);
            msObject.JsonString = masterSecretJson;
            msObject.Handle = result;
            return await Task.FromResult(msObject);
        }
        /// <summary>
        /// Creates a new <see cref="MasterSecret"/>.
        /// </summary>
        /// <exception cref="SharedRsException">Throws when <see cref="MasterSecret"/> can't be created.</exception>
        /// <returns>New <see cref="MasterSecret"/> as JSON string.</returns>
        public static async Task<string> CreateMasterSecretJsonAsync()
        {
            IntPtr result = new IntPtr();
            int errorCode = NativeMethods.credx_create_master_secret(ref result);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            string masterSecretJson = await ObjectApi.ToJsonAsync(result);

            return await Task.FromResult(masterSecretJson);
        }

        private static async Task<MasterSecret> CreateMasterSecretObject(IntPtr objectHandle)
        {
            string masterSecretJson = await ObjectApi.ToJsonAsync(objectHandle);
            MasterSecret requestObject = JsonConvert.DeserializeObject<MasterSecret>(masterSecretJson, Settings.JsonSettings);
            requestObject.JsonString = masterSecretJson;
            requestObject.Handle = objectHandle;
            return await Task.FromResult(requestObject);
        }
        private static async Task<MasterSecret> CreateMasterSecretFromJsonAsync(string masterSecretJson)
        {
            IntPtr masterSecretHandle = new IntPtr();
            int errorCode = NativeMethods.credx_master_secret_from_json(ByteBuffer.Create(masterSecretJson), ref masterSecretHandle);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            MasterSecret result = await CreateMasterSecretObject(masterSecretHandle);
            return await Task.FromResult(result);
        }

    }
}