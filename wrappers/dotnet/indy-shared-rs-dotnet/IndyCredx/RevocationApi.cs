﻿using indy_shared_rs_dotnet.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.Models.Structures;

namespace indy_shared_rs_dotnet.IndyCredx
{
    public static class RevocationApi
    {
        /// <summary>
        /// Creates a new <see cref="RevocationRegistry"/> object and its corresponding informative objects.
        /// </summary>
        /// <param name="originDid">Did of issuer.</param>
        /// <param name="credDefObject">Credential definition.</param>
        /// <param name="tag">Tag.</param>
        /// <param name="revRegType">Type of revocation registry.</param>
        /// <param name="issuanceType">Type of issuance.</param>
        /// <param name="maxCredNumber">Maximum number of credential entries.</param>
        /// <param name="tailsDirPath">Path to tails file.</param>
        /// <exception cref="SharedRsException">Throws if any parameter is invalid.</exception>
        /// <returns>A new <see cref="RevocationRegistry"/>, its <see cref="RevocationRegistryDefinition"/>, <see cref="RevocationRegistryDefinitionPrivate"/> and <see cref="RevocationRegistryDelta"/> objects.</returns>
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

        /// <summary>
        /// Creates a new <see cref="RevocationRegistry"/> object from json <see cref="System.String"/>.
        /// </summary>
        /// <param name="revRegJson">Json <see cref="System.String"/> representing a <see cref="RevocationRegistry"/> object.</param>
        /// <exception cref="SharedRsException">Throws when provided <paramref name="revRegJson"/> is invalid.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Throws when provided <paramref name="revRegJson"/> is empty.</exception>
        /// <returns>A new <see cref="RevocationRegistry"/> object.</returns>
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

        /// <summary>
        /// Creates a new <see cref="RevocationRegistryDefinition"/> object from json <see cref="System.String"/>.
        /// </summary>
        /// <param name="revRegDefJson">Json <see cref="System.String"/> representing a <see cref="RevocationRegistryDefinition"/> object.</param>
        /// <exception cref="SharedRsException">Throws when provided <paramref name="revRegDefJson"/> is invalid.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Throws when provided <paramref name="revRegDefJson"/> is empty.</exception>
        /// <returns>The deserialized <see cref="RevocationRegistryDefinition"/> object that was serialized in json.</returns>
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

        /// <summary>
        /// Updates a provided <see cref="RevocationRegistry"/> object.
        /// </summary>
        /// <param name="revRegDefObject">Revocation registry definition.</param>
        /// <param name="revRegObject">Revocation registry.</param>
        /// <param name="issued">Issued entries.</param>
        /// <param name="revoked">Revoked entries.</param>
        /// <param name="tailsPath">Path of tails file.</param>
        /// <exception cref="SharedRsException">Throws if any parameter is invalid.</exception>
        /// <returns>An updated <see cref="RevocationRegistry"/> and its <see cref="RevocationRegistryDelta"/>.</returns>
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

        /// <summary>
        /// Revokes a <see cref="Credential"/> on the revocation registry.
        /// </summary>
        /// <param name="revRegDefObject">Revocation registry definition.</param>
        /// <param name="revRegObject">Corresponding revocation registry.</param>
        /// <param name="credRevIdx">Index of the <see cref="Credential"/> in the revocation registry.</param>
        /// <param name="tailsPath">Path to tails file.</param>
        /// <exception cref="SharedRsException">Throws if any parameter is invalid.</exception>
        /// <returns>A new <see cref="RevocationRegistry"/> and <see cref="RevocationRegistryDelta"/> object with the refered <see cref="Credential"/> revoked.</returns>
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

        /// <summary>
        /// Merges two <see cref="RevocationRegistryDelta"/> objects into one.
        /// </summary>
        /// <param name="revRegDeltaObject1">First delta.</param>
        /// <param name="revRegDeltaObject2">Second delta.</param>
        /// <exception cref="SharedRsException">Throws if <paramref name="revRegDeltaObject1"/> or <paramref name="revRegDeltaObject2"/> are invalid.</exception>
        /// <returns>The merged <see cref="RevocationRegistryDelta"/>.</returns>
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

        /// <summary>
        /// Updates the provided <see cref="CredentialRevocationState"/> or creates a new one.
        /// </summary>
        /// <param name="revRegDefObject">The revocation registry definition.</param>
        /// <param name="revRegDeltaObject">The revocation registry delta.</param>
        /// <param name="revRegIndex">The revocation registry index.</param>
        /// <param name="timestamp">Unix timestamp.</param>
        /// <param name="tailsPath">Path to the tails file.</param>
        /// <param name="revState">Revocation state to update.</param>
        /// <exception cref="SharedRsException">Throws if any parameter is invalid.</exception>
        /// <returns>A new <see cref="CredentialRevocationState"/> object.</returns>
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

        /// <summary>
        /// Creates a new <see cref="CredentialRevocationState"/> object from json <see cref="System.String"/>.
        /// </summary>
        /// <param name="revStateJson">Json <see cref="System.String"/> representing a revocation object.</param>
        /// <exception cref="SharedRsException">Throws when provided <paramref name="revStateJson"/> is invalid.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Throws when provided <paramref name="revStateJson"/> is empty.</exception>
        /// <returns>A new <see cref="CredentialRevocationState"/> object.</returns>
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

        /// <summary>
        /// Get the value of an <see cref="RevocationRegistryDefinition"/> attribute (Supported attribute names so far: id, max_cred_num, tails_hash or tails_location).
        /// </summary>
        /// <param name="revRegDefObject">Revocation registry definition from which the attribute is requested.</param>
        /// <param name="attributeName">Name of the requested attribute.</param>
        /// <exception cref="SharedRsException">Throws when provided <paramref name="attributeName"/> or <paramref name="revRegDefObject"/> are invalid.</exception>
        /// <returns>The value of the requested <paramref name="attributeName"/> from the provided <paramref name="revRegDefObject"/>.</returns>
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
            RevocationRegistryDefinition regDefObject = JsonConvert.DeserializeObject<RevocationRegistryDefinition>(regDefJson, Settings.JsonSettings);
            regDefObject.Handle = objectHandle;
            return await Task.FromResult(regDefObject);
        }

        private static async Task<RevocationRegistryDefinitionPrivate> CreateRevocationRegistryDefinitionPrivateObject(uint objectHandle)
        {
            string regDefPvtJson = await ObjectApi.ToJsonAsync(objectHandle);
            RevocationRegistryDefinitionPrivate regDefPvtObject = JsonConvert.DeserializeObject<RevocationRegistryDefinitionPrivate>(regDefPvtJson, Settings.JsonSettings);
            regDefPvtObject.Handle = objectHandle;
            return await Task.FromResult(regDefPvtObject);
        }

        private static async Task<RevocationRegistry> CreateRevocationRegistryObject(uint objectHandle)
        {
            string revRegJson = await ObjectApi.ToJsonAsync(objectHandle);
            RevocationRegistry revRegObject = JsonConvert.DeserializeObject<RevocationRegistry>(revRegJson, Settings.JsonSettings);
            revRegObject.Handle = objectHandle;
            return await Task.FromResult(revRegObject);
        }

        private static async Task<RevocationRegistryDelta> CreateRevocationRegistryDeltaObject(uint objectHandle)
        {
            string revRegDeltaJson = await ObjectApi.ToJsonAsync(objectHandle);
            RevocationRegistryDelta revRegDeltaObject = JsonConvert.DeserializeObject<RevocationRegistryDelta>(revRegDeltaJson, Settings.JsonSettings);
            revRegDeltaObject.Handle = objectHandle;
            return await Task.FromResult(revRegDeltaObject);
        }

        private static async Task<CredentialRevocationState> CreateCredentialRevocationStateObject(uint objectHandle)
        {
            string credRevStateJson = await ObjectApi.ToJsonAsync(objectHandle);
            CredentialRevocationState credRevStateObject = JsonConvert.DeserializeObject<CredentialRevocationState>(credRevStateJson, Settings.JsonSettings);
            credRevStateObject.Handle = objectHandle;
            return await Task.FromResult(credRevStateObject);
        }
    }
}