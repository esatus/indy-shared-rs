using indy_shared_rs_dotnet.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.Models.Structures;

namespace indy_shared_rs_dotnet.IndyCredx
{
    public static class RevocationApi
    {
        public static async Task<(RevocationRegistryDefinition, RevocationRegistryDefinitionPrivate, RevocationRegistry, RevocationRegistryDelta)> CreateRevocationRegistryAsync(
            string originDid,
            CredentialDefinition credDefObject,
            string tag,
            RegistryType revRegType,
            IssuerType issuanceType,
            long maxCredNumber,
            string tailsDirPath)
        {
            uint regDefObjectHandle = 0;
            uint regDefPvtObjectHandle = 0;
            uint regEntryObjectHandle = 0;
            uint regInitDeltaObjectHandle = 0;

            int errorCode = NativeMethods.credx_create_revocation_registry(
                FfiStr.Create(originDid),
                credDefObject.Handle,
                FfiStr.Create(tag),
                FfiStr.Create(revRegType.ToString()),
                FfiStr.Create(issuanceType.ToString()),
                maxCredNumber,
                FfiStr.Create(tailsDirPath),
                ref regDefObjectHandle,
                ref regDefPvtObjectHandle,
                ref regEntryObjectHandle,
                ref regInitDeltaObjectHandle);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }
            RevocationRegistryDefinition regDefObject = await CreateRevocationRegistryDefinitionObject(regDefObjectHandle);
            RevocationRegistryDefinitionPrivate regDefPvtObject = await CreateRevocationRegistryDefinitionPrivateObject(regDefPvtObjectHandle);
            RevocationRegistry revRegObject = await CreateRevocationRegistryObject(regEntryObjectHandle);
            RevocationRegistryDelta regInitDeltaObject = await CreateRevocationRegistryDeltaObject(regInitDeltaObjectHandle);

            return await Task.FromResult((regDefObject, regDefPvtObject, revRegObject, regInitDeltaObject));
        }


        public static async Task<RevocationRegistry> CreateRevocationRegistryFromJsonAsync(string revRegJson)
        {
            uint regEntryObjectHandle = 0;
            int errorCode = NativeMethods.credx_revocation_registry_from_json(ByteBuffer.Create(revRegJson), ref regEntryObjectHandle);
            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            RevocationRegistry revRegObject = await CreateRevocationRegistryObject(regEntryObjectHandle);
            return await Task.FromResult(revRegObject);
        }

        public static async Task<RevocationRegistryDefinition> CreateRevocationRegistryDefinitionFromJsonAsync(string revRegDefJson)
        {
            uint revRegDefObjectHandle = 0;
            int errorCode = NativeMethods.credx_revocation_registry_definition_from_json(ByteBuffer.Create(revRegDefJson), ref revRegDefObjectHandle);
            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            RevocationRegistryDefinition revRegDefObject = await CreateRevocationRegistryDefinitionObject(revRegDefObjectHandle);
            return await Task.FromResult(revRegDefObject);
        }

        public static async Task<(RevocationRegistry, RevocationRegistryDelta)> UpdateRevocationRegistryAsync(
            RevocationRegistryDefinition revRegDefObject,
            RevocationRegistry revRegObject,
            List<long> issued,
            List<long> revoked,
            string tailsPath)
        {
            uint revRegObjectHandle = 0;
            uint revRegDeltaObjectHandle = 0;

            int errorCode = NativeMethods.credx_update_revocation_registry(
                revRegDefObject.Handle,
                revRegObject.Handle,
                FfiLongList.Create(issued),
                FfiLongList.Create(revoked),
                FfiStr.Create(tailsPath),
                ref revRegObjectHandle,
                ref revRegDeltaObjectHandle);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }
            RevocationRegistry revRegObjectUpdated = await CreateRevocationRegistryObject(revRegObjectHandle);
            RevocationRegistryDelta revRegDeltaObject = await CreateRevocationRegistryDeltaObject(revRegDeltaObjectHandle);

            return await Task.FromResult((revRegObjectUpdated, revRegDeltaObject));
        }

        public static async Task<(RevocationRegistry, RevocationRegistryDelta)> RevokeCredentialAsync(
            RevocationRegistryDefinition revRegDefObject,
            RevocationRegistry revRegObject,
            long credRevIdx,
            string tailsPath)
        {
            uint revRegObjectHandle = 0;
            uint revRegDeltaObjectHandle = 0;

            int errorCode = NativeMethods.credx_revoke_credential(
                revRegDefObject.Handle,
                revRegObject.Handle,
                credRevIdx,
                FfiStr.Create(tailsPath),
                ref revRegObjectHandle,
                ref revRegDeltaObjectHandle);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }
            RevocationRegistry revRegObjectUpdated = await CreateRevocationRegistryObject(revRegObjectHandle);
            RevocationRegistryDelta revRegDeltaObject = await CreateRevocationRegistryDeltaObject(revRegDeltaObjectHandle);

            return await Task.FromResult((revRegObjectUpdated, revRegDeltaObject));
        }

        public static async Task<RevocationRegistryDelta> MergeRevocationRegistryDeltasAsync(
            RevocationRegistryDelta revRegDeltaObject1,
            RevocationRegistryDelta revRegDeltaObject2)
        {
            uint revRegDeltaObjectHandleNew = 0;

            int errorCode = NativeMethods.credx_merge_revocation_registry_deltas(
                revRegDeltaObject1.Handle,
                revRegDeltaObject2.Handle,
                ref revRegDeltaObjectHandleNew);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            RevocationRegistryDelta revRegDeltaObjectNew = await CreateRevocationRegistryDeltaObject(revRegDeltaObjectHandleNew);

            return await Task.FromResult(revRegDeltaObjectNew);
        }

        public static async Task<CredentialRevocationState> CreateOrUpdateRevocationStateAsync(
            RevocationRegistryDefinition revRegDefObject,
            RevocationRegistryDelta revRegDeltaObject,
            long revRegIndex,
            long timestamp,
            string tailsPath,
            CredentialRevocationState revState)
        {
            uint credRevStateObjectHandle = 0;

            int errorCode = NativeMethods.credx_create_or_update_revocation_state(
                revRegDefObject.Handle,
                revRegDeltaObject.Handle,
                revRegIndex,
                timestamp,
                FfiStr.Create(tailsPath),
                revState.Handle,
                ref credRevStateObjectHandle);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            CredentialRevocationState credRevStateObject = await CreateCredentialRevocationStateObject(credRevStateObjectHandle);

            return await Task.FromResult(credRevStateObject);
        }

        public static async Task<CredentialRevocationState> CreateRevocationStateFromJsonAsync(string revStateJson)
        {
            uint revStateObjectHandle = 0;
            int errorCode = NativeMethods.credx_revocation_state_from_json(ByteBuffer.Create(revStateJson), ref revStateObjectHandle);
            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }
            CredentialRevocationState revStateObject = await CreateCredentialRevocationStateObject(revStateObjectHandle);
            return await Task.FromResult(revStateObject);
        }

        /**
         * Possible attributenames: id, max_cred_num, tails_hash or tails_location
         */
        public static async Task<string> GetRevocationRegistryDefinitionAttributeAsync(RevocationRegistryDefinition revRegDefObject, string attributeName)
        {
            string result = "";
            int errorCode = NativeMethods.credx_revocation_registry_definition_get_attribute(revRegDefObject.Handle, FfiStr.Create(attributeName), ref result);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            return await Task.FromResult(result);
        }

        private static async Task<RevocationRegistryDefinition> CreateRevocationRegistryDefinitionObject(uint objectHandle)
        {
            string regDefJson = await ObjectApi.ToJsonAsync(objectHandle);
            RevocationRegistryDefinition regDefObject = JsonConvert.DeserializeObject<RevocationRegistryDefinition>(regDefJson, Settings.jsonSettings);
            regDefObject.Handle = objectHandle;
            return await Task.FromResult(regDefObject);
        }

        private static async Task<RevocationRegistryDefinitionPrivate> CreateRevocationRegistryDefinitionPrivateObject(uint objectHandle)
        {
            string regDefPvtJson = await ObjectApi.ToJsonAsync(objectHandle);
            RevocationRegistryDefinitionPrivate regDefPvtObject = JsonConvert.DeserializeObject<RevocationRegistryDefinitionPrivate>(regDefPvtJson, Settings.jsonSettings);
            regDefPvtObject.Handle = objectHandle;
            return await Task.FromResult(regDefPvtObject);
        }

        private static async Task<RevocationRegistry> CreateRevocationRegistryObject(uint objectHandle)
        {
            string revRegJson = await ObjectApi.ToJsonAsync(objectHandle);
            RevocationRegistry revRegObject = JsonConvert.DeserializeObject<RevocationRegistry>(revRegJson, Settings.jsonSettings);
            revRegObject.Handle = objectHandle;
            return await Task.FromResult(revRegObject);
        }

        private static async Task<RevocationRegistryDelta> CreateRevocationRegistryDeltaObject(uint objectHandle)
        {
            string revRegDeltaJson = await ObjectApi.ToJsonAsync(objectHandle);
            RevocationRegistryDelta revRegDeltaObject = JsonConvert.DeserializeObject<RevocationRegistryDelta>(revRegDeltaJson, Settings.jsonSettings);
            revRegDeltaObject.Handle = objectHandle;
            return await Task.FromResult(revRegDeltaObject);
        }

        private static async Task<CredentialRevocationState> CreateCredentialRevocationStateObject(uint objectHandle)
        {
            string credRevStateJson = await ObjectApi.ToJsonAsync(objectHandle);
            CredentialRevocationState credRevStateObject = JsonConvert.DeserializeObject<CredentialRevocationState>(credRevStateJson, Settings.jsonSettings);
            credRevStateObject.Handle = objectHandle;
            return await Task.FromResult(credRevStateObject);
        }
    }
}
