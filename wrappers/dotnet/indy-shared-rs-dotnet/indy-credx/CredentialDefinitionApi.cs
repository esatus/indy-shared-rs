using indy_shared_rs_dotnet.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.Models.Structures;

namespace indy_shared_rs_dotnet.indy_credx
{
    public static class CredentialDefinitionApi
    {
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
            //note: only signatureType "CL" supported so far.
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

        public static async Task<string> GetCredentialDefinitionAttributeAsync(CredentialDefinition credDefObject, string attributeName)
        {
            //note: only "id" and "schema_id" as attributeName supported so far.
            string result = "";
            int errorCode = NativeMethods.credx_credential_definition_get_attribute(credDefObject.Handle, FfiStr.Create(attributeName), ref result);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }
            return await Task.FromResult(result);
        }

        private static async Task<CredentialDefinition> CreateCredentialDefinitonObject(uint objectHandle)
        {
            string credDefJson = await ObjectApi.ToJson(objectHandle);
            CredentialDefinition credDefObject = JsonConvert.DeserializeObject<CredentialDefinition>(credDefJson, Settings.jsonSettings);
            credDefObject.Handle = objectHandle;

            try
            {
                JObject jObj = JObject.Parse(credDefJson);
                credDefObject.Value.Primary.r = new List<KeyProofAttributeValue>();
                foreach (JToken ele in jObj["value"]["primary"]["r"])
                {
                    string[] attrFields = ele.ToString().Split(':');
                    KeyProofAttributeValue attribute = new(JsonConvert.DeserializeObject<string>(attrFields[0]), JsonConvert.DeserializeObject<string>(attrFields[1]));
                    credDefObject.Value.Primary.r.Add(attribute);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not find field r.");
                Console.WriteLine(e);
            }
            return await Task.FromResult(credDefObject);
        }

        private static async Task<CredentialDefinitionPrivate> CreateCredentialDefinitonPrivateObject(uint objectHandle)
        {
            string credDefPvtJson = await ObjectApi.ToJson(objectHandle);
            CredentialDefinitionPrivate credDefPvtObject = JsonConvert.DeserializeObject<CredentialDefinitionPrivate>(credDefPvtJson, Settings.jsonSettings);
            credDefPvtObject.Handle = objectHandle;
            return await Task.FromResult(credDefPvtObject);
        }

        private static async Task<CredentialKeyCorrectnessProof> CreateCredentialKeyProofObject(uint objectHandle)
        {
            string keyProofJson = await ObjectApi.ToJson(objectHandle);
            CredentialKeyCorrectnessProof keyProofObject = JsonConvert.DeserializeObject<CredentialKeyCorrectnessProof>(keyProofJson, Settings.jsonSettings);
            keyProofObject.Handle = objectHandle;

            try
            {
                JObject jObj = JObject.Parse(keyProofJson);
                keyProofObject.xrcap = new List<KeyProofAttributeValue>();
                foreach (JToken ele in jObj["xr_cap"])
                {
                    KeyProofAttributeValue attribute = new(ele.First.ToString(), ele.Last.ToString());
                    keyProofObject.xrcap.Add(attribute);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not find field xr_cap.");
                Console.WriteLine(e);
            }
            return await Task.FromResult(keyProofObject);
        }
    }
}
