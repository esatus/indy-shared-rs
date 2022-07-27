using indy_shared_rs_dotnet.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.Models.Structures;

namespace indy_shared_rs_dotnet.IndyCredx
{
    public static class CredentialApi
    {
        /// <summary>
        /// Creates a new tuple of <see cref="Credential"/>, <see cref="RevocationRegistry"/> and <see cref="RevocationRegistryDelta"/> objects.
        /// </summary>
        /// <param name="credDefObject">Definition of the credential.</param>
        /// <param name="credDefPvtObject">Private key params of the credential.</param>
        /// <param name="credOfferObject">Credential offer.</param>
        /// <param name="credReqObject">Credential request.</param>
        /// <param name="attributeNames">Attribute names.</param>
        /// <param name="attributeRawValues">Raw values of the attributes.</param>
        /// <param name="attributeEncodedValues">Encoded values of the attributes.</param>
        /// <param name="credRevInfo">Revocation configuration.</param>
        /// <exception cref="SharedRsException">Throws if any parameters are invalid.</exception>
        /// <returns>A new <see cref="Credential"/>, <see cref="RevocationRegistry"/> and <see cref="RevocationRegistryDelta"/>.</returns>
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
            IntPtr credObjectHandle = new IntPtr();
            IntPtr revRegObjectHandle = new IntPtr();
            IntPtr revDeltaObjectHandle = new IntPtr();
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
                throw SharedRsException.FromSdkError(error);
            }

            Credential credObject = await CreateCredentialObjectAsync(credObjectHandle);

            RevocationRegistry revRegObject = null;
            if (!revRegObjectHandle.Equals(new IntPtr()))
            {
                revRegObject = await CreateRevocationRegistryObjectAsync(revRegObjectHandle);
            }

            RevocationRegistryDelta revDeltaObject = null;
            if (!revDeltaObjectHandle.Equals( new IntPtr()))
            {
                revDeltaObject = await CreateRevocationRegistryDeltaObjectAsync(revDeltaObjectHandle);
            }

            return await Task.FromResult((credObject, revRegObject, revDeltaObject));
        }

        /// <summary>
        /// Creates a new tuple of <see cref="Credential"/>, <see cref="RevocationRegistry"/> and <see cref="RevocationRegistryDelta"/> objects.
        /// </summary>
        /// <param name="credDefObjectJson">Definition of the credential.</param>
        /// <param name="credDefPvtObjectJson">Private key params of the credential.</param>
        /// <param name="credOfferObjectJson">Credential offer.</param>
        /// <param name="credReqObjectJson">Credential request.</param>
        /// <param name="attributeNames">Attribute names.</param>
        /// <param name="attributeRawValues">Raw values of the attributes.</param>
        /// <param name="attributeEncodedValues">Encoded values of the attributes.</param>
        /// <param name="credRevInfo">Revocation configuration.</param>
        /// <exception cref="SharedRsException">Throws if any parameters are invalid.</exception>
        /// <returns>A new <see cref="Credential"/>, <see cref="RevocationRegistry"/> and <see cref="RevocationRegistryDelta"/>.</returns>
        public static async Task<(string, string, string)> CreateCredentialAsync(
            string credDefObjectJson,
            string credDefPvtObjectJson,
            string credOfferObjectJson,
            string credReqObjectJson,
            List<string> attributeNames,
            List<string> attributeRawValues,
            List<string> attributeEncodedValues,
            CredentialRevocationConfig credRevInfo)  // TODO ??? replace with json?
        {
            IntPtr credDefObjectHandle = new IntPtr();
            IntPtr credDefPvtObjectHandle = new IntPtr();
            IntPtr credOfferObjectHandle = new IntPtr();
            IntPtr credReqObjectHandle = new IntPtr();
            _ = NativeMethods.credx_credential_definition_from_json(ByteBuffer.Create(credDefObjectJson), ref credDefObjectHandle);
            _ = NativeMethods.credx_credential_definition_private_from_json(ByteBuffer.Create(credDefPvtObjectJson), ref credDefPvtObjectHandle);
            _ = NativeMethods.credx_credential_offer_from_json(ByteBuffer.Create(credOfferObjectJson), ref credOfferObjectHandle);
            _ = NativeMethods.credx_credential_definition_from_json(ByteBuffer.Create(credReqObjectJson), ref credReqObjectHandle);

            IntPtr credObjectHandle = new IntPtr();
            IntPtr revRegObjectHandle = new IntPtr();
            IntPtr revDeltaObjectHandle = new IntPtr();
            int errorCode = NativeMethods.credx_create_credential(
                credDefObjectHandle,
                credDefPvtObjectHandle,
                credOfferObjectHandle,
                credReqObjectHandle,
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
                throw SharedRsException.FromSdkError(error);
            }

            string credJson = await ObjectApi.ToJsonAsync(credObjectHandle);

            string revRegJson = null;
            if (!revRegObjectHandle.Equals(new IntPtr()))
            {
                revRegJson = await ObjectApi.ToJsonAsync(revRegObjectHandle);
            }

            string revDeltaJson = null;
            if (!revDeltaObjectHandle.Equals(new IntPtr()))
            {
                revDeltaJson = await ObjectApi.ToJsonAsync(revDeltaObjectHandle);
            }

            return await Task.FromResult((credJson, revRegJson, revDeltaJson));
        }

        /// <summary>
        /// Processes a given <see cref="Credential"/>.
        /// </summary>
        /// <param name="credential">Credential to be processed.</param>
        /// <param name="credentialRequestMetadata">Metadata of the credential request.</param>
        /// <param name="masterSecret">Used master secret.</param>
        /// <param name="credentialDefinition">Credential definition of the processed credential.</param>
        /// <param name="revocationRegistryDefinition">Revocation registry definition for the processed credential.</param>
        /// <exception cref="SharedRsException">Throws if any parameters are invalid.</exception>
        /// <returns>A copy of the processed <see cref="Credential"/>.</returns>
        public static async Task<Credential> ProcessCredentialAsync(
            Credential credential,
            CredentialRequestMetadata credentialRequestMetadata,
            MasterSecret masterSecret,
            CredentialDefinition credentialDefinition,
            RevocationRegistryDefinition revocationRegistryDefinition)
        {
            IntPtr credentialObjectHandle = new IntPtr();
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
                throw SharedRsException.FromSdkError(error);
            }

            Credential credentialObject = await CreateCredentialObjectAsync(credentialObjectHandle);

            return await Task.FromResult(credentialObject);
        }

        /// <summary>
        /// Processes a given <see cref="Credential"/>.
        /// </summary>
        /// <param name="credentialJson">Credential to be processed as JSON string.</param>
        /// <param name="credentialRequestMetadataJson">Metadata of the credential request as JSON string.</param>
        /// <param name="masterSecretJson">Used master secret as JSON string.</param>
        /// <param name="credentialDefinitionJson">Credential definition of the processed credential as JSON string.</param>
        /// <param name="revocationRegistryDefinitionJson">Revocation registry definition for the processed credential as JSON string.</param>
        /// <exception cref="SharedRsException">Throws if any parameters are invalid.</exception>
        /// <returns>A copy of the processed <see cref="Credential"/> as JSON string.</returns>
        public static async Task<string> ProcessCredentialAsync(
            string credentialJson,
            string credentialRequestMetadataJson,
            string masterSecretJson,
            string credentialDefinitionJson,
            string revocationRegistryDefinition)
        {
            IntPtr credObjectHandle = new IntPtr();
            IntPtr credReqMetadataObjectHandle = new IntPtr();
            IntPtr masterSecretObjectHandle = new IntPtr();
            IntPtr credDefObjectHandle = new IntPtr();
            IntPtr revRegDefObjectHandle = new IntPtr();
            _ = NativeMethods.credx_credential_from_json(ByteBuffer.Create(credentialJson), ref credObjectHandle);
            _ = NativeMethods.credx_credential_request_metadata_from_json(ByteBuffer.Create(credentialRequestMetadataJson), ref credReqMetadataObjectHandle);
            _ = NativeMethods.credx_master_secret_from_json(ByteBuffer.Create(masterSecretJson), ref masterSecretObjectHandle);
            _ = NativeMethods.credx_credential_definition_from_json(ByteBuffer.Create(credentialDefinitionJson), ref credDefObjectHandle);
            _ = NativeMethods.credx_revocation_registry_definition_from_json(ByteBuffer.Create(revocationRegistryDefinition), ref revRegDefObjectHandle);

            IntPtr credentialObjectHandle = new IntPtr();
            int errorCode = NativeMethods.credx_process_credential(
                credObjectHandle,
                credReqMetadataObjectHandle,
                masterSecretObjectHandle,
                credDefObjectHandle,
                revRegDefObjectHandle,
                ref credentialObjectHandle);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            string credentialObjectJson = await ObjectApi.ToJsonAsync(credentialObjectHandle);

            return await Task.FromResult(credentialObjectJson);
        }

        /// <summary>
        /// Encodes raw attributes to be used in a <see cref="Credential"/>.
        /// </summary>
        /// <param name="rawAttributes">Attributes to be encoded.</param>
        /// <exception cref="SharedRsException">Throws when <paramref name="rawAttributes"/> are invalid.</exception>
        /// <exception cref="System.InvalidOperationException">Throws when <paramref name="rawAttributes"/> are empty.</exception>
        /// <returns>Returns the given <paramref name="rawAttributes"/> as encoded attributes.</returns>
        public static async Task<List<string>> EncodeCredentialAttributesAsync(List<string> rawAttributes)
        {
            string result = "";
            int errorCode = NativeMethods.credx_encode_credential_attributes(FfiStrList.Create(rawAttributes), ref result);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }
            string[] abc = result.Split(",");
            return await Task.FromResult(abc.ToList());
        }

        /// <summary>
        /// Returns the value of a requested <see cref="Credential"/> attribute (Currently supported attribute names: "schema_id", "cred_def_id", "rev_reg_id", "rev_reg_index").
        /// </summary>
        /// <param name="credential">The credential object from which the attribute value is requested.</param>
        /// <param name="attributeName">The name of the attribute that is requested.</param>
        /// <exception cref="SharedRsException">Throws when attribute name is invalid.</exception>
        /// <returns>The value of requested <paramref name="attributeName"/> from the provided <paramref name="credential"/>.</returns>
        public static async Task<string> GetCredentialAttributeAsync(Credential credential, string attributeName)
        {
            string result = "";
            int errorCode = NativeMethods.credx_credential_get_attribute(credential.Handle, FfiStr.Create(attributeName), ref result);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            return await Task.FromResult(result);
        }

        /// <summary>
        /// Returns the value of a requested <see cref="Credential"/> attribute (Currently supported attribute names: "schema_id", "cred_def_id", "rev_reg_id", "rev_reg_index").
        /// </summary>
        /// <param name="credentialJson">The credential object as JSON string from which the attribute value is requested.</param>
        /// <param name="attributeName">The name of the attribute that is requested.</param>
        /// <exception cref="SharedRsException">Throws when attribute name is invalid.</exception>
        /// <returns>The value of requested <paramref name="attributeName"/> from the provided <paramref name="credential"/>.</returns>
        public static async Task<string> GetCredentialAttributeAsync(string credentialJson, string attributeName)
        {
            IntPtr credObjectHandle = new IntPtr();
            _ = NativeMethods.credx_credential_from_json(ByteBuffer.Create(credentialJson), ref credObjectHandle);
            string result = "";
            int errorCode = NativeMethods.credx_credential_get_attribute(credObjectHandle, FfiStr.Create(attributeName), ref result);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            return await Task.FromResult(result);
        }

        #region private methods
        /// <summary>
        /// Creates a new <see cref="Credential"/> object from json <see cref="string"/>.
        /// </summary>
        /// <param name="schemaJson">Json <see cref="string"/> representing a <see cref="Credential"/> object.</param>
        /// <exception cref="SharedRsException">Throws when provided <paramref name="schemaJson"/> is invalid.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Throws when <paramref name="schemaJson"/> is empty.</exception>
        /// <returns>A new <see cref="Credential"/> object.</returns>
        private static async Task<Credential> CreateCredentialFromJsonAsync(string credJson)
        {
            IntPtr credentialObjectHandle = new IntPtr();
            int errorCode = NativeMethods.credx_credential_from_json(ByteBuffer.Create(credJson), ref credentialObjectHandle);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            Credential credentialObject = await CreateCredentialObjectAsync(credentialObjectHandle);
            return await Task.FromResult(credentialObject);
        }
        private static async Task<Credential> CreateCredentialObjectAsync(IntPtr objectHandle)
        {
            string credJson = await ObjectApi.ToJsonAsync(objectHandle);
            Credential credObject = JsonConvert.DeserializeObject<Credential>(credJson, Settings.JsonSettings);
            credObject.JsonString = credJson;
            credObject.Handle = objectHandle;
            return await Task.FromResult(credObject);
        }
        private static async Task<RevocationRegistry> CreateRevocationRegistryObjectAsync(IntPtr objectHandle)
        {
            string revRegJson = await ObjectApi.ToJsonAsync(objectHandle);
            RevocationRegistry revRegObject = JsonConvert.DeserializeObject<RevocationRegistry>(revRegJson, Settings.JsonSettings);
            revRegObject.JsonString = revRegJson;
            revRegObject.Handle = objectHandle;
            return await Task.FromResult(revRegObject);
        }
        private static async Task<RevocationRegistryDelta> CreateRevocationRegistryDeltaObjectAsync(IntPtr objectHandle)
        {
            string revDeltaJson = await ObjectApi.ToJsonAsync(objectHandle);
            RevocationRegistryDelta revDeltaObject = JsonConvert.DeserializeObject<RevocationRegistryDelta>(revDeltaJson, Settings.JsonSettings);
            revDeltaObject.JsonString = revDeltaJson;
            revDeltaObject.Handle = objectHandle;
            return await Task.FromResult(revDeltaObject);
        }
        private static async Task<CredentialDefinition> CreateCredentialDefinitionFromJsonAsync(string credDefJson)
        {
            IntPtr credDefHandle = new IntPtr();
            int errorCode = NativeMethods.credx_credential_definition_from_json(ByteBuffer.Create(credDefJson), ref credDefHandle);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            CredentialDefinition result = await CreateCredentialDefinitonObject(credDefHandle);
            return await Task.FromResult(result);
        }
        private static async Task<CredentialDefinition> CreateCredentialDefinitonObject(IntPtr objectHandle)
        {
            string credDefJson = await ObjectApi.ToJsonAsync(objectHandle);
            CredentialDefinition credDefObject = JsonConvert.DeserializeObject<CredentialDefinition>(credDefJson, Settings.JsonSettings);
            credDefObject.JsonString = credDefJson;
            credDefObject.Handle = objectHandle;

            try
            {
                JObject jObj = JObject.Parse(credDefJson);
                credDefObject.Value.Primary.R = new List<KeyProofAttributeValue>();
                foreach (JToken ele in jObj["value"]["primary"]["r"])
                {
                    string[] attrFields = ele.ToString().Split(':');
                    KeyProofAttributeValue attribute = new KeyProofAttributeValue(JsonConvert.DeserializeObject<string>(attrFields[0]), JsonConvert.DeserializeObject<string>(attrFields[1]));
                    credDefObject.Value.Primary.R.Add(attribute);
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException("Could not find field r.", e);
            }
            return await Task.FromResult(credDefObject);
        }
        private static async Task<CredentialOffer> CreateCredentialOfferFromJsonAsync(string credOfferJson)
        {
            IntPtr credOfferHandle = new IntPtr();
            int errorCode = NativeMethods.credx_credential_offer_from_json(ByteBuffer.Create(credOfferJson), ref credOfferHandle);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            CredentialOffer result = await CreateCredentialOfferObject(credOfferHandle);
            return await Task.FromResult(result);
        }
        private static async Task<CredentialOffer> CreateCredentialOfferObject(IntPtr objectHandle)
        {
            string credOfferJson = await ObjectApi.ToJsonAsync(objectHandle);
            CredentialOffer credOfferObject = JsonConvert.DeserializeObject<CredentialOffer>(credOfferJson, Settings.JsonSettings);
            credOfferObject.JsonString = credOfferJson;
            credOfferObject.Handle = objectHandle;

            try
            {
                JObject jObj = JObject.Parse(credOfferJson);
                credOfferObject.KeyCorrectnessProof.XrCap = new List<KeyProofAttributeValue>();
                foreach (JToken ele in jObj["key_correctness_proof"]["xr_cap"])
                {
                    KeyProofAttributeValue attribute = new KeyProofAttributeValue(ele.First.ToString(), ele.Last.ToString());
                    credOfferObject.KeyCorrectnessProof.XrCap.Add(attribute);
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException("Could not find field xr_cap.", e);
            }

            return await Task.FromResult(credOfferObject);
        }
        private static async Task<CredentialDefinitionPrivate> CreateCredentialDefinitionPrivateFromJsonAsync(string credDefPrivJson)
        {
            IntPtr credDefPrivHandle = new IntPtr();
            int errorCode = NativeMethods.credx_credential_definition_private_from_json(ByteBuffer.Create(credDefPrivJson), ref credDefPrivHandle);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            CredentialDefinitionPrivate result = await CreateCredentialDefinitonPrivateObject(credDefPrivHandle);
            return await Task.FromResult(result);
        }
        private static async Task<CredentialDefinitionPrivate> CreateCredentialDefinitonPrivateObject(IntPtr objectHandle)
        {
            string credDefPvtJson = await ObjectApi.ToJsonAsync(objectHandle);
            CredentialDefinitionPrivate credDefPvtObject = JsonConvert.DeserializeObject<CredentialDefinitionPrivate>(credDefPvtJson, Settings.JsonSettings);
            credDefPvtObject.JsonString = credDefPvtJson;
            credDefPvtObject.Handle = objectHandle;
            return await Task.FromResult(credDefPvtObject);
        }
        private static async Task<CredentialRequest> CreateCredentialRequestFromJsonAsync(string credReqJson)
        {
            IntPtr credReqHandle = new IntPtr();
            int errorCode = NativeMethods.credx_credential_request_from_json(ByteBuffer.Create(credReqJson), ref credReqHandle);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            CredentialRequest result = await CreateCredentialRequestObject(credReqHandle);
            return await Task.FromResult(result);
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
        private static async Task<CredentialRequestMetadata> CreateCredentialRequestMetadataFromJsonAsync(string credReqMetaJson)
        {
            IntPtr credReqHandle = new IntPtr();
            int errorCode = NativeMethods.credx_credential_request_metadata_from_json(ByteBuffer.Create(credReqMetaJson), ref credReqHandle);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            CredentialRequestMetadata result = await CreateCredentialRequestMetadataObject(credReqHandle);
            return await Task.FromResult(result);
        }
        private static async Task<MasterSecret> CreateMasterSecretObject(IntPtr objectHandle)
        {
            string masterSecretJson = await ObjectApi.ToJsonAsync(objectHandle);
            MasterSecret requestObject = JsonConvert.DeserializeObject<MasterSecret>(masterSecretJson, Settings.JsonSettings);
            requestObject.JsonString = masterSecretJson;
            requestObject.Handle = objectHandle;
            return await Task.FromResult(requestObject);
        }
        private static async Task<MasterSecret> CreateMasterSecretFromJsonAsync(string masterSecretJson)
        {
            IntPtr masterSecretHandle = new IntPtr();
            int errorCode = NativeMethods.credx_master_secret_from_json(ByteBuffer.Create(masterSecretJson), ref masterSecretHandle);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            MasterSecret result = await CreateMasterSecretObject(masterSecretHandle);
            return await Task.FromResult(result);
        }
        #endregion
    }
}