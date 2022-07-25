using indy_shared_rs_dotnet.Models;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.Models.Structures;

namespace indy_shared_rs_dotnet.IndyCredx
{
    public static class CredentialRequestApi
    {
        /// <summary>
        /// Creates a new <see cref="CredentialRequest"/> from <see cref="CredentialDefinition"/>.
        /// </summary>
        /// <param name="proverDid">Prover DID.</param>
        /// <param name="credentialDefinition">Credential definition.</param>
        /// <param name="masterSecret">New master secret.</param>
        /// <param name="masterSecretId">Id of master secret.</param>
        /// <param name="credentialOffer">Credential offer.</param>
        /// <exception cref="SharedRsException">Throws if any argument is invalid.</exception>
        /// <returns>New <see cref="CredentialRequest"/> and its <see cref="CredentialRequestMetadata"/>.</returns>
        public static async Task<(CredentialRequest, CredentialRequestMetadata)> CreateCredentialRequestAsync(
            string proverDid,
            CredentialDefinition credentialDefinition,
            MasterSecret masterSecret,
            string masterSecretId,
            CredentialOffer credentialOffer)
        {
            IntPtr requestHandle = new IntPtr();
            IntPtr metadataHandle = new IntPtr();
            int errorCode = NativeMethods.credx_create_credential_request(
                FfiStr.Create(proverDid),
                credentialDefinition.Handle,
                masterSecret.Handle,
                FfiStr.Create(masterSecretId),
                credentialOffer.Handle,
                ref requestHandle,
                ref metadataHandle);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }
            CredentialRequest requestObject = await CreateCredentialRequestObject(requestHandle);
            CredentialRequestMetadata metadataObject = await CreateCredentialRequestMetadataObject(metadataHandle);
            return (requestObject, metadataObject);
        }

        private static async Task<CredentialRequest> CreateCredentialRequestObject(IntPtr objectHandle)
        {
            string credReqJson = await ObjectApi.ToJsonAsync(objectHandle);
            CredentialRequest requestObject = JsonConvert.DeserializeObject<CredentialRequest>(credReqJson, Settings.JsonSettings);
            requestObject.JsonString = credReqJson;
            requestObject.Handle = objectHandle;
            return await Task.FromResult(requestObject);
        }

        private static async Task<CredentialRequestMetadata> CreateCredentialRequestMetadataObject(IntPtr objectHandle)
        {
            string credMetadataJson = await ObjectApi.ToJsonAsync(objectHandle);
            CredentialRequestMetadata requestObject = JsonConvert.DeserializeObject<CredentialRequestMetadata>(credMetadataJson, Settings.JsonSettings);
            requestObject.JsonString = credMetadataJson;
            requestObject.Handle = objectHandle;
            return await Task.FromResult(requestObject);
        }
    }
}