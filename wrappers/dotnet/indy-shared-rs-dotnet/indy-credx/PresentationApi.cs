using indy_shared_rs_dotnet.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.Models.Structures;

namespace indy_shared_rs_dotnet.indy_credx
{
    public static class PresentationApi
    {
        public static async Task<Presentation> CreatePresentationAsync(
            PresentationRequest presentationRequest,
            List<CredentialEntry> credentialEntries,
            List<CredentialProve> credentialProves,
            List<string> selfAttestNames, 
            List<string> selfAttestValues,
            MasterSecret masterSecret,
            List<Schema> schemas,
            List<CredentialDefinition> credDefs)
        {
            uint presentationObjectHandle = 0;
            List<uint> schemaHandles = (from schema in schemas
                                       select schema.Handle).ToList();
            List<uint> credDefHandles = (from schema in credDefs
                                         select schema.Handle).ToList();

            int errorCode = NativeMethods.credx_create_presentation(
                presentationRequest.Handle,
                FfiCredentialEntryList.Create(credentialEntries),
                FfiCredentialProveList.Create(credentialProves),
                FfiStrList.Create(selfAttestNames),
                FfiStrList.Create(selfAttestValues),
                masterSecret.Handle,
                FfiUIntList.Create(schemaHandles),
                FfiUIntList.Create(credDefHandles),
                ref presentationObjectHandle);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw new SharedRsException(JsonConvert.DeserializeObject<Dictionary<string, string>>(error)["message"]);
            }

            Presentation presentationObject = await CreatePresentationObject(presentationObjectHandle);
            return await Task.FromResult(presentationObject);
        }

        private static async Task<Presentation> CreatePresentationObject(uint objectHandle)
        {
            string presentationJson = await ObjectApi.ToJson(objectHandle);
            Presentation presentationObject = JsonConvert.DeserializeObject<Presentation>(presentationJson, Settings.jsonSettings);

            presentationObject.Handle = objectHandle;
            return await Task.FromResult(presentationObject);
        }
    }
}
