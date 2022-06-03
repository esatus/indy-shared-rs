using indy_shared_rs_dotnet.Models;
using Newtonsoft.Json;
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
            NativeMethods.credx_create_credential_request(
                proverDid,
                credentialDefinition.Handle,
                masterSecret.Handle,
                masterSecretId,
                credentialOffer.Handle,
                ref requestHandle,
                ref metadataHandle);

            CredentialRequest requestObject = await CreateCredentialRequestObject(requestHandle);
            CredentialRequestMetadata metadataObject = await CreateCredentialRequestMetadataObject(metadataHandle);
            return (requestObject, metadataObject);
        }

        private static async Task<CredentialRequest> CreateCredentialRequestObject(uint objectHandle)
        {
            IndyObject indyObject = new(objectHandle);
            string credReqJson = await indyObject.toJson();
            CredentialRequest requestObject = JsonConvert.DeserializeObject<CredentialRequest>(credReqJson, Settings.jsonSettings);
            requestObject.Handle = objectHandle;
            return await Task.FromResult(requestObject);
        }

        private static async Task<CredentialRequestMetadata> CreateCredentialRequestMetadataObject(uint objectHandle)
        {
            IndyObject indyObject = new(objectHandle);
            string credReqJson = await indyObject.toJson();
            CredentialRequestMetadata requestObject = JsonConvert.DeserializeObject<CredentialRequestMetadata>(credReqJson, Settings.jsonSettings);
            requestObject.Handle = objectHandle;
            return await Task.FromResult(requestObject);
        }
    }
}
