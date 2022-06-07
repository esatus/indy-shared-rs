using indy_shared_rs_dotnet.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.indy_credx
{
    public static class CredentialRequestApi
    {
        public static async Task<(CredentialRequest, CredentialRequestMetadata)> CreateCredentialRequestAsync(
            string proverDid,
            CredentialDefinition credentialDefinition,
            MasterSecret masterSecret,
            string masterSecretId,
            CredentialOffer credentialOffer)
        {
            uint requestHandle = 0;
            uint metadataHandle = 0;
            int errorCode =  NativeMethods.credx_create_credential_request(
                proverDid,
                credentialDefinition.Handle,
                masterSecret.Handle,
                masterSecretId,
                credentialOffer.Handle,
                ref requestHandle,
                ref metadataHandle);

            if (errorCode != 0)
            {
                string error = "";
                NativeMethods.credx_get_current_error(ref error);
                Debug.WriteLine(error);
            }
            CredentialRequest requestObject = await CreateCredentialRequestObject(requestHandle);
            CredentialRequestMetadata metadataObject = await CreateCredentialRequestMetadataObject(metadataHandle);
            return (requestObject, metadataObject);
        }

        private static async Task<CredentialRequest> CreateCredentialRequestObject(uint objectHandle)
        {
            string credReqJson = await ObjectApi.ToJson(objectHandle);
            CredentialRequest requestObject = JsonConvert.DeserializeObject<CredentialRequest>(credReqJson, Settings.jsonSettings);
            requestObject.Handle = objectHandle;
            return await Task.FromResult(requestObject);
        }

        private static async Task<CredentialRequestMetadata> CreateCredentialRequestMetadataObject(uint objectHandle)
        {
            string credMetadataJson = await ObjectApi.ToJson(objectHandle);
            CredentialRequestMetadata requestObject = JsonConvert.DeserializeObject<CredentialRequestMetadata>(credMetadataJson, Settings.jsonSettings);
            requestObject.Handle = objectHandle;
            return await Task.FromResult(requestObject);
        }
    }
}
