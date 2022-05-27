using indy_shared_rs_dotnet.models;
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
            uint requestRef = 0;
            uint metadataRef = 0;
            NativeMethods.credx_create_credential_request(
                proverDid,
                credentialDefinition.Handle,
                masterSecret.Handle,
                masterSecretId,
                credentialOffer.Handle,
                ref requestRef,
                ref metadataRef);

            IndyObject requestObject = new(requestRef);
            IndyObject metadataObject = new(metadataRef);

            CredentialRequest request = JsonConvert.DeserializeObject<CredentialRequest>(await requestObject.toJson());
            CredentialRequestMetadata metadata = JsonConvert.DeserializeObject<CredentialRequestMetadata>(await metadataObject.toJson());

            return (request, metadata);
        }
    }
}
