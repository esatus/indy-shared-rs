﻿using indy_shared_rs_dotnet.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.Models.Structures;

namespace indy_shared_rs_dotnet.IndyCredx
{
    public static class CredentialOfferApi
    {
        /// <summary>
        /// Create a new <see cref="CredentialOffer"/> from <see cref="CredentialDefinition"/>.
        /// </summary>
        /// <param name="schemaId">Id of the corresponding schema.</param>
        /// <param name="credDefObject">Credential definition.</param>
        /// <param name="keyProofObject">Key correctness proof.</param>
        /// <exception cref="SharedRsException">Throws if any parameter is invalid.</exception>
        /// <returns>A new <see cref="CredentialOffer"/>.</returns>
        public static async Task<CredentialOffer> CreateCredentialOfferAsync(
            string schemaId,
            CredentialDefinition credDefObject,
            CredentialKeyCorrectnessProof keyProofObject)
        {
            IntPtr credOfferObjectHandle = new IntPtr();
            int errorCode = NativeMethods.credx_create_credential_offer(FfiStr.Create(schemaId), credDefObject.Handle, keyProofObject.Handle, ref credOfferObjectHandle);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            CredentialOffer credOfferObject = await CreateCredentialOfferObject(credOfferObjectHandle);
            return await Task.FromResult(credOfferObject);
        }

        /// <summary>
        /// Create a new <see cref="CredentialOffer"/> from <see cref="CredentialDefinition"/>.
        /// </summary>
        /// <param name="schemaId">Id of the corresponding schema.</param>
        /// <param name="credDefObjectJson">Credential definition as JSON string.</param>
        /// <param name="keyProofObjectJson">Key correctness proof as JSON string.</param>
        /// <exception cref="SharedRsException">Throws if any parameter is invalid.</exception>
        /// <returns>A new <see cref="CredentialOffer"/> as JSON string.</returns>
        public static async Task<string> CreateCredentialOfferAsync(
            string schemaId,
            string credDefObjectJson,
            string keyProofObjectJson)
        {
            CredentialDefinition credDefObject = await CreateCredentialDefinitionFromJsonAsync(credDefObjectJson);
            CredentialKeyCorrectnessProof keyProofObject = await CreateKeyCorrectnessProofFromJsonAsync(keyProofObjectJson);

            IntPtr credOfferObjectHandle = new IntPtr();
            int errorCode = NativeMethods.credx_create_credential_offer(FfiStr.Create(schemaId), credDefObject.Handle, keyProofObject.Handle, ref credOfferObjectHandle);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            CredentialOffer credOfferObject = await CreateCredentialOfferObject(credOfferObjectHandle);
            return await Task.FromResult(credOfferObject.JsonString);
        }

        #region private methods
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

        private static async Task<CredentialOffer> CreateCredentialOfferFromJsonAsync(string credDefJson)
        {
            IntPtr credOfferHandle = new IntPtr();
            int errorCode = NativeMethods.credx_credential_offer_from_json(ByteBuffer.Create(credDefJson), ref credOfferHandle);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            CredentialOffer result = await CreateCredentialOfferObject(credOfferHandle);
            return await Task.FromResult(result);
        }
        private static async Task<CredentialKeyCorrectnessProof> CreateCredentialKeyProofObject(IntPtr objectHandle)
        {
            string keyProofJson = await ObjectApi.ToJsonAsync(objectHandle);
            CredentialKeyCorrectnessProof keyProofObject = JsonConvert.DeserializeObject<CredentialKeyCorrectnessProof>(keyProofJson, Settings.JsonSettings);
            keyProofObject.JsonString = keyProofJson;
            keyProofObject.Handle = objectHandle;

            try
            {
                JObject jObj = JObject.Parse(keyProofJson);
                keyProofObject.XrCap = new List<KeyProofAttributeValue>();
                foreach (JToken ele in jObj["xr_cap"])
                {
                    KeyProofAttributeValue attribute = new KeyProofAttributeValue(ele.First.ToString(), ele.Last.ToString());
                    keyProofObject.XrCap.Add(attribute);
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException("Could not find field xr_cap.", e);
            }
            return await Task.FromResult(keyProofObject);
        }
        private static async Task<CredentialKeyCorrectnessProof> CreateKeyCorrectnessProofFromJsonAsync(string keyCorrectnessProofJson)
        {
            IntPtr keyCorrectnessProofHandle = new IntPtr();
            int errorCode = NativeMethods.credx_key_correctness_proof_from_json(ByteBuffer.Create(keyCorrectnessProofJson), ref keyCorrectnessProofHandle);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            CredentialKeyCorrectnessProof result = await CreateCredentialKeyProofObject(keyCorrectnessProofHandle);
            return await Task.FromResult(result);
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

        #endregion
    }
}