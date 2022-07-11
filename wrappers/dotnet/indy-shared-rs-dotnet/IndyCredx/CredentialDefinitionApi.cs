using indy_shared_rs_dotnet.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.Models.Structures;

namespace indy_shared_rs_dotnet.IndyCredx
{
    public static class CredentialDefinitionApi
    {
        /// <summary>
        /// Creates a new <see cref="CredentialDefinition"/> from schema and other parameters (only signatureType "CL" supported so far).
        /// </summary>
        /// <param name="originDid">Issuer DID.</param>
        /// <param name="schemaObject">Corresponding schema.</param>
        /// <param name="tag">Tag name.</param>
        /// <param name="signatureType">Type of the sginature.</param>
        /// <param name="supportRevocation">Flag if revocation is supported or not.</param>
        /// <exception cref="SharedRsException">Throws if any provided parameters are invalid.</exception>
        /// <returns>The new <see cref="CredentialDefinition"/>, <see cref="CredentialDefinitionPrivate"/> and <see cref="CredentialKeyCorrectnessProof"/>.</returns>
        public static async Task<(CredentialDefinition, CredentialDefinitionPrivate, CredentialKeyCorrectnessProof)> CreateCredentialDefinitionAsync(
            string originDid,
            Schema schemaObject,
            string tag,
            SignatureType signatureType,
            byte supportRevocation)
        {
            uint credDefHandle = 0;
            uint credDefPvtHandle = 0;
            uint keyProofHandle = 0;
            int errorCode = NativeMethods.credx_create_credential_definition(
                FfiStr.Create(originDid),
                schemaObject.Handle,
                FfiStr.Create(tag),
                FfiStr.Create(signatureType.ToString()),
                supportRevocation,
                ref credDefHandle,
                ref credDefPvtHandle,
                ref keyProofHandle);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            CredentialDefinition credDefObject = await CreateCredentialDefinitonObject(credDefHandle);
            CredentialDefinitionPrivate credDefPvtObject = await CreateCredentialDefinitonPrivateObject(credDefPvtHandle);
            CredentialKeyCorrectnessProof keyProofObject = await CreateCredentialKeyProofObject(keyProofHandle);
            return await Task.FromResult((credDefObject, credDefPvtObject, keyProofObject));
        }

        /// <summary>
        /// Returns the value of a <see cref="CredentialDefinition"/> attribute (only the attribute names "id" and "schema_id" are supported so far).
        /// </summary>
        /// <param name="credDefObject">Definition to get the value from.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <exception cref="SharedRsException">Throws if <paramref name="attributeName"/> or <paramref name="credDefObject"/> are invalid.</exception>
        /// <returns>The value of the requested <paramref name="attributeName"/> from the provided <paramref name="credDefObject"/>.</returns>
        public static async Task<string> GetCredentialDefinitionAttributeAsync(CredentialDefinition credDefObject, string attributeName)
        {
            string result = "";
            int errorCode = NativeMethods.credx_credential_definition_get_attribute(credDefObject.Handle, FfiStr.Create(attributeName), ref result);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }
            return await Task.FromResult(result);
        }

        /// <summary>
        /// Creates a <see cref="CredentialDefinition"/> object from json <see cref="System.String"/>.
        /// </summary>
        /// <param name="credDefJson">Json string encoding a credential definition object.</param>
        /// <exception cref="SharedRsException">Throws when <paramref name="credDefJson"/> is invalid.</exception>
        /// <returns>The new <see cref="CredentialDefinition"/> object.</returns>
        public static async Task<CredentialDefinition> CreateCredentialDefinitionFromJsonAsync(string credDefJson)
        {
            uint credDefHandle = 0;
            int errorCode = NativeMethods.credx_credential_definition_from_json(ByteBuffer.Create(credDefJson), ref credDefHandle);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            CredentialDefinition result = await CreateCredentialDefinitonObject(credDefHandle);
            return await Task.FromResult(result);
        }

        private static async Task<CredentialDefinition> CreateCredentialDefinitonObject(uint objectHandle)
        {
            string credDefJson = await ObjectApi.ToJsonAsync(objectHandle);
            CredentialDefinition credDefObject = JsonConvert.DeserializeObject<CredentialDefinition>(credDefJson, Settings.JsonSettings);
            credDefObject.Handle = objectHandle;

            try
            {
                JObject jObj = JObject.Parse(credDefJson);
                credDefObject.Value.Primary.R = new List<KeyProofAttributeValue>();
                foreach (JToken ele in jObj["value"]["primary"]["r"])
                {
                    string[] attrFields = ele.ToString().Split(':');
                    KeyProofAttributeValue attribute = new(JsonConvert.DeserializeObject<string>(attrFields[0]), JsonConvert.DeserializeObject<string>(attrFields[1]));
                    credDefObject.Value.Primary.R.Add(attribute);
                }
            }
            catch(Exception e)
            {
                throw new ArgumentException("Could not find field r.", e);
            }
            return await Task.FromResult(credDefObject);
        }

        private static async Task<CredentialDefinitionPrivate> CreateCredentialDefinitonPrivateObject(uint objectHandle)
        {
            string credDefPvtJson = await ObjectApi.ToJsonAsync(objectHandle);
            CredentialDefinitionPrivate credDefPvtObject = JsonConvert.DeserializeObject<CredentialDefinitionPrivate>(credDefPvtJson, Settings.JsonSettings);
            credDefPvtObject.Handle = objectHandle;
            return await Task.FromResult(credDefPvtObject);
        }

        private static async Task<CredentialKeyCorrectnessProof> CreateCredentialKeyProofObject(uint objectHandle)
        {
            string keyProofJson = await ObjectApi.ToJsonAsync(objectHandle);
            CredentialKeyCorrectnessProof keyProofObject = JsonConvert.DeserializeObject<CredentialKeyCorrectnessProof>(keyProofJson, Settings.JsonSettings);
            keyProofObject.Handle = objectHandle;

            try
            {
                JObject jObj = JObject.Parse(keyProofJson);
                keyProofObject.XrCap = new List<KeyProofAttributeValue>();
                foreach (JToken ele in jObj["xr_cap"])
                {
                    KeyProofAttributeValue attribute = new(ele.First.ToString(), ele.Last.ToString());
                    keyProofObject.XrCap.Add(attribute);
                }
            }
            catch(Exception e)
            {
                throw new ArgumentException("Could not find field xr_cap.", e);
            }
            return await Task.FromResult(keyProofObject);
        }
    }
}
