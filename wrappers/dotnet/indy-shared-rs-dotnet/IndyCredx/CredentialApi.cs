using indy_shared_rs_dotnet.Models;
using Newtonsoft.Json;
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
                throw SharedRsException.FromSdkError(error);
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
                throw SharedRsException.FromSdkError(error);
            }

            Credential credentialObject = await CreateCredentialObjectAsync(credentialObjectHandle);

            return await Task.FromResult(credentialObject);
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

        private static async Task<Credential> CreateCredentialObjectAsync(uint objectHandle)
        {
            string credJson = await ObjectApi.ToJsonAsync(objectHandle);
            Credential credObject = JsonConvert.DeserializeObject<Credential>(credJson, Settings.JsonSettings);
            credObject.Handle = objectHandle;
            return await Task.FromResult(credObject);
        }

        private static async Task<RevocationRegistry> CreateRevocationRegistryObjectAsync(uint objectHandle)
        {
            string revRegJson = await ObjectApi.ToJsonAsync(objectHandle);
            RevocationRegistry revRegObject = JsonConvert.DeserializeObject<RevocationRegistry>(revRegJson, Settings.JsonSettings);
            revRegObject.Handle = objectHandle;
            return await Task.FromResult(revRegObject);
        }

        private static async Task<RevocationRegistryDelta> CreateRevocationRegistryDeltaObjectAsync(uint objectHandle)
        {
            string revDeltaJson = await ObjectApi.ToJsonAsync(objectHandle);
            RevocationRegistryDelta revDeltaObject = JsonConvert.DeserializeObject<RevocationRegistryDelta>(revDeltaJson, Settings.JsonSettings);
            revDeltaObject.Handle = objectHandle;
            return await Task.FromResult(revDeltaObject);
        }
    }
}
