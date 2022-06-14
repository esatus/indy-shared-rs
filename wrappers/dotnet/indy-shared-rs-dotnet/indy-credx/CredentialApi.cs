using indy_shared_rs_dotnet.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.Models.Structures;

namespace indy_shared_rs_dotnet.indy_credx
{
    public static class CredentialApi
    {
        public static async Task<(Credential, RevocationRegistry, RevocationRegistryDelta)> CreateCredentialAsync(
            CredentialDefinition credDefObject,
            CredentialDefinitionPrivate credDefPvtObject,
            CredentialOffer credOfferObject,
            CredentialRequest credReqObject,
            List<string> attributeNames,
            List<string> attributeRawValues,
            List<string> attributeEncodedValues,
            CredentialRevocationConfig credRevInfo)
        {
            uint credObjectHandle = 0;
            uint revRegObjectHandle = 0;
            uint revDeltaObjectHandle = 0;
            int errorCode = NativeMethods.credx_create_credential(
                credDefObject.Handle,
                credDefPvtObject.Handle,
                credOfferObject.Handle,
                credReqObject.Handle,
                FfiStrList.Create(attributeNames),
                FfiStrList.Create(attributeRawValues),
                FfiStrList.Create(attributeEncodedValues),
                FfiCredRevInfo.Create(credRevInfo),
                ref credObjectHandle,
                ref revRegObjectHandle,
                ref revDeltaObjectHandle);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw new SharedRsException(JsonConvert.DeserializeObject<Dictionary<string, string>>(error)["message"]);
            }

            Credential credObject = await CreateCredentialObjectAsync(credObjectHandle);

            RevocationRegistry revRegObject = null;
            if (revRegObjectHandle != 0)
            {
                revRegObject = await CreateRevocationRegistryObjectAsync(revRegObjectHandle);
            }

            RevocationRegistryDelta revDeltaObject = null;
            if (revDeltaObjectHandle != 0)
            {
                revDeltaObject = await CreateRevocationRegistryDeltaObjectAsync(revDeltaObjectHandle);
            }

            return await Task.FromResult((credObject, revRegObject, revDeltaObject));
        }

        public static async Task<Credential> ProcessCredentialAsync(
            Credential credential,
            CredentialRequestMetadata credentialRequestMetadata,
            MasterSecret masterSecret,
            CredentialDefinition credentialDefinition,
            RevocationRegistryDefinition revocationRegistryDefinition)
        {
            uint credentialObjectHandle = 0;
            int errorCode = NativeMethods.credx_process_credential(
                credential.Handle,
                credentialRequestMetadata.Handle,
                masterSecret.Handle,
                credentialDefinition.Handle,
                revocationRegistryDefinition.Handle,
                ref credentialObjectHandle);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw new SharedRsException(JsonConvert.DeserializeObject<Dictionary<string, string>>(error)["message"]);
            }

            Credential credentialObject = await CreateCredentialObjectAsync(credentialObjectHandle);

            return await Task.FromResult(credentialObject);
        }

        public static async Task<List<string>> EncodeCredentialAttributesAsync(List<string> rawAttributes)
        {
            string result = "";
            int errorCode = NativeMethods.credx_encode_credential_attributes(FfiStrList.Create(rawAttributes), ref result);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw new SharedRsException(JsonConvert.DeserializeObject<Dictionary<string, string>>(error)["message"]);
            }
            string[] abc = result.Split(",");
            return await Task.FromResult(abc.ToList());
        }

        public static async Task<string> GetCredentialAttributeAsync(Credential credential, string attributeName)
        {
            string result = "";
            //note: only attributeName "schema_id", "cred_def_id", "rev_reg_id", "rev_reg_index" supported so far.
            int errorCode = NativeMethods.credx_credential_get_attribute(credential.Handle, FfiStr.Create(attributeName), ref result);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw new SharedRsException(JsonConvert.DeserializeObject<Dictionary<string, string>>(error)["message"]);
            }

            return await Task.FromResult(result);
        }

        private static async Task<Credential> CreateCredentialObjectAsync(uint objectHandle)
        {
            string credJson = await ObjectApi.ToJson(objectHandle);
            Credential credObject = JsonConvert.DeserializeObject<Credential>(credJson, Settings.jsonSettings);
            credObject.Handle = objectHandle;
            return await Task.FromResult(credObject);
        }

        private static async Task<RevocationRegistry> CreateRevocationRegistryObjectAsync(uint objectHandle)
        {
            string revRegJson = await ObjectApi.ToJson(objectHandle);
            RevocationRegistry revRegObject = JsonConvert.DeserializeObject<RevocationRegistry>(revRegJson, Settings.jsonSettings);
            revRegObject.Handle = objectHandle;
            return await Task.FromResult(revRegObject);
        }

        private static async Task<RevocationRegistryDelta> CreateRevocationRegistryDeltaObjectAsync(uint objectHandle)
        {
            string revDeltaJson = await ObjectApi.ToJson(objectHandle);
            RevocationRegistryDelta revDeltaObject = JsonConvert.DeserializeObject<RevocationRegistryDelta>(revDeltaJson, Settings.jsonSettings);
            revDeltaObject.Handle = objectHandle;
            return await Task.FromResult(revDeltaObject);
        }
    }
}
