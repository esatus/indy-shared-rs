using indy_shared_rs_dotnet.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.Models.Structures;

namespace indy_shared_rs_dotnet.indy_credx
{
    public static class CredentialApi
    {
        public static async Task<(Credential, RevocationRegistry, RevocationDelta)> CreateCredentialAsync(
            CredentialDefinition credDefObject,
            CredentialDefinitionPrivate credDefPvtObject,
            CredentialOffer credOfferObject,
            CredentialRequest credReqObject,
            List<string> attributeNames,
            List<string> attributeRawValues,
            List<string> attributeEncodedValues,
            CredentialRevocationInfo credRevInfo)
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
                ref credRevInfo,
                ref credObjectHandle,
                ref revRegObjectHandle,
                ref revDeltaObjectHandle);

            if (errorCode != 0)
            {
                string error = "";
                NativeMethods.credx_get_current_error(ref error);
                Debug.WriteLine(error);
            }

            Credential credObject = await CreateCredentialObjectAsync(credObjectHandle);
            RevocationRegistry revRegObject = await CreateRevocationRegistryObjectAsync(revRegObjectHandle);
            RevocationDelta revDeltaObject = await CreateRevocationDeltaObjectAsync(revDeltaObjectHandle);

            return await Task.FromResult((credObject, revRegObject, revDeltaObject));
        }

        public static async Task<Credential> ProcessCredentialAsync(
            Credential credential,
            CredentialRequest credentialRequest,
            MasterSecret masterSecret,
            CredentialDefinition credentialDefinition,
            RevocationRegistryDefinition revocationRegistryDefinition)
        {
            uint credentialObjectHandle = 0;
            NativeMethods.credx_process_credential(
                credential.Handle,
                credentialRequest.Handle,
                masterSecret.Handle,
                credentialDefinition.Handle,
                revocationRegistryDefinition.Handle,
                ref credentialObjectHandle);

            Credential credentialObject = await CreateCredentialObjectAsync(credentialObjectHandle);

            return await Task.FromResult(credentialObject);
        }

        public static async Task<string> EncodeCredentialAttributesAsync(List<string> rawAttributes)
        {
            string result = "";
            NativeMethods.credx_encode_credential_attributes(FfiStrList.Create(rawAttributes), ref result);
            return await Task.FromResult(result);
        }

        public static async Task<string> GetCredentialAttributeAsync(Credential credential, string attributeName)
        {
            string result = "";
            //note: only attributeName "schema_id", "cred_def_id", "rev_reg_id", "rev_reg_index" supported so far.
            NativeMethods.credx_credential_get_attribute(credential.Handle, FfiStr.Create(attributeName), ref result);
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

        private static async Task<RevocationDelta> CreateRevocationDeltaObjectAsync(uint objectHandle)
        {
            string revDeltaJson = await ObjectApi.ToJson(objectHandle);
            RevocationDelta revDeltaObject = JsonConvert.DeserializeObject<RevocationDelta>(revDeltaJson, Settings.jsonSettings);
            revDeltaObject.Handle = objectHandle;
            return await Task.FromResult(revDeltaObject);
        }
    }
}
