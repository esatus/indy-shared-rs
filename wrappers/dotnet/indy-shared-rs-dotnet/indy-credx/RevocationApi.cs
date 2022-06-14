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
                Console.WriteLine(error);
                throw new SharedRsException(JsonConvert.DeserializeObject<Dictionary<string, string>>(error)["message"]);
            }
            RevocationRegistryDefinition regDefObject = await CreateRevocationRegistryDefinitionObject(regDefObjectHandle);
            RevocationRegistryDefinitionPrivate regDefPvtObject = await CreateRevocationRegistryDefinitionPrivateObject(regDefPvtObjectHandle);
            RevocationRegistry revRegObject = await CreateRevocationRegistryObject(regEntryObjectHandle);
            RevocationRegistryDelta regInitDeltaObject = await CreateRevocationRegistryDeltaObject(regInitDeltaObjectHandle);

            return await Task.FromResult((regDefObject, regDefPvtObject, revRegObject, regInitDeltaObject));
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
                throw new SharedRsException(JsonConvert.DeserializeObject<Dictionary<string, string>>(error)["message"]);
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
                throw new SharedRsException(JsonConvert.DeserializeObject<Dictionary<string, string>>(error)["message"]);
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
                throw new SharedRsException(JsonConvert.DeserializeObject<Dictionary<string, string>>(error)["message"]);
            }
            
            RevocationRegistryDelta revRegDeltaObjectNew = await CreateRevocationRegistryDeltaObject(revRegDeltaObjectHandleNew);

            return await Task.FromResult(revRegDeltaObjectNew);
        }

        public static async Task<CredentialRevocationState> CreateOrUpdateRevocationState(
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
                throw new SharedRsException(JsonConvert.DeserializeObject<Dictionary<string, string>>(error)["message"]);
            }

            CredentialRevocationState credRevStateObject = await CreateCredentialRevocationStateObject(credRevStateObjectHandle);

            return await Task.FromResult(credRevStateObject);
        }

        public static async Task<string> GetRevocationRegistryDefinitionAttribute(RevocationRegistryDefinition revRegDefObject, string attributeName) 
        {
            string result = "";
            int errorCode = NativeMethods.credx_revocation_registry_definition_get_attribute(revRegDefObject.Handle, FfiStr.Create(attributeName), ref result);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw new SharedRsException(JsonConvert.DeserializeObject<Dictionary<string, string>>(error)["message"]);
            }

            return await Task.FromResult(result);
        }

        private static async Task<RevocationRegistryDefinition> CreateRevocationRegistryDefinitionObject(uint objectHandle)
        {
            string regDefJson = await ObjectApi.ToJson(objectHandle);
            RevocationRegistryDefinition regDefObject = JsonConvert.DeserializeObject<RevocationRegistryDefinition>(regDefJson, Settings.jsonSettings);
            regDefObject.Handle = objectHandle;
            return await Task.FromResult(regDefObject);
        }

        private static async Task<RevocationRegistryDefinitionPrivate> CreateRevocationRegistryDefinitionPrivateObject(uint objectHandle)
        {
            string regDefPvtJson = await ObjectApi.ToJson(objectHandle);
            RevocationRegistryDefinitionPrivate regDefPvtObject = JsonConvert.DeserializeObject<RevocationRegistryDefinitionPrivate>(regDefPvtJson, Settings.jsonSettings);
            regDefPvtObject.Handle = objectHandle;
            return await Task.FromResult(regDefPvtObject);
        }

        private static async Task<RevocationRegistry> CreateRevocationRegistryObject(uint objectHandle)
        {
            string revRegJson = await ObjectApi.ToJson(objectHandle);
            RevocationRegistry revRegObject = JsonConvert.DeserializeObject<RevocationRegistry>(revRegJson, Settings.jsonSettings);
            revRegObject.Handle = objectHandle;
            return await Task.FromResult(revRegObject);
        }

        private static async Task<RevocationRegistryDelta> CreateRevocationRegistryDeltaObject(uint objectHandle)
        {
            string revRegDeltaJson = await ObjectApi.ToJson(objectHandle);
            RevocationRegistryDelta revRegDeltaObject = JsonConvert.DeserializeObject<RevocationRegistryDelta>(revRegDeltaJson, Settings.jsonSettings);
            revRegDeltaObject.Handle = objectHandle;
            return await Task.FromResult(revRegDeltaObject);
        }

        private static async Task<CredentialRevocationState> CreateCredentialRevocationStateObject(uint objectHandle)
        {
            string credRevStateJson = await ObjectApi.ToJson(objectHandle);
            CredentialRevocationState credRevStateObject = JsonConvert.DeserializeObject<CredentialRevocationState>(credRevStateJson, Settings.jsonSettings);
            credRevStateObject.Handle = objectHandle;
            return await Task.FromResult(credRevStateObject);
        }
    }
}
