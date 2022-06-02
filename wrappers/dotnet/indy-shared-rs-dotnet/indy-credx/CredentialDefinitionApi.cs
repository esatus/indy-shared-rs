using indy_shared_rs_dotnet.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.indy_credx
{
    public static class CredentialDefinitionApi
    {
        public static async Task<(CredentialDefinition, CredentialDefinitionPrivate, CredentialKeyCorrectnessProof)> CreateCredentialDefinition(
            string originDid, 
            uint schemaHandle,
            string tag,
            string signatureType, 
            byte supportRevocation)
        {
            uint credDefHandle = 0;
            uint credDefPvtHandle = 0;
            uint keyProofHandle = 0;
            //note: only signatureType "CL" supported so far.
            NativeMethods.credx_create_credential_definition(originDid, schemaHandle, tag, signatureType, supportRevocation, 
                                                             ref credDefHandle, ref credDefPvtHandle, ref keyProofHandle);

            CredentialDefinition credDefObject = await CreateCredentialDefinitonObject(credDefHandle);
            CredentialDefinitionPrivate credDefPvtObject = await CreateCredentialDefinitonPrivateObject(credDefPvtHandle);
            CredentialKeyCorrectnessProof keyProofObject = await CreateCredentialKeyProofObject(keyProofHandle);

            return await Task.FromResult((credDefObject, credDefPvtObject, keyProofObject));
         }

        public static async Task<string> GetCredentialDefinitionAttribute(uint objectHandle, string attributeName)
        {
            //note: only "id" and "schema_id" as attributeName supported so far.
            string result = "";
            NativeMethods.credx_credential_definition_get_attribute(objectHandle, attributeName, ref result);
            return await Task.FromResult(result);
        }

        private static async Task<CredentialDefinition> CreateCredentialDefinitonObject(uint objectHandle)
        {
            IndyObject indyObject = new(objectHandle);
            string credDefJson = await indyObject.toJson();
            CredentialDefinition credDefObject = JsonConvert.DeserializeObject<CredentialDefinition>(credDefJson);
            credDefObject.Handle = objectHandle;

            try
            {
                JObject jObj = JObject.Parse(credDefJson);
                credDefObject.Value.Primary.r = new List<KeyProofAttributeValue>();
                foreach (var ele in jObj["value"]["primary"]["r"])
                {
                    //Todo : Find better way to extract strings from ele object -> {"age":"123"}
                    string[] json = ele.ToString().Split(':','"');
                    KeyProofAttributeValue attribute = new(json[1], json[4]);
                    credDefObject.Value.Primary.r.Add(attribute);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Could not find field r.");
                Debug.WriteLine(e);
            }
            return await Task.FromResult(credDefObject);
        }

        private static async Task<CredentialDefinitionPrivate> CreateCredentialDefinitonPrivateObject(uint objectHandle)
        {
            IndyObject indyObject = new(objectHandle);
            CredentialDefinitionPrivate credDefPvtObject = JsonConvert.DeserializeObject<CredentialDefinitionPrivate>(await indyObject.toJson());
            credDefPvtObject.Handle = objectHandle;
            return await Task.FromResult(credDefPvtObject);
        }

        private static async Task<CredentialKeyCorrectnessProof> CreateCredentialKeyProofObject(uint objectHandle)
        {
            IndyObject indyObject = new(objectHandle);
            string keyProofJson = await indyObject.toJson();
            CredentialKeyCorrectnessProof keyProofObject = JsonConvert.DeserializeObject<CredentialKeyCorrectnessProof>(keyProofJson);
            keyProofObject.Handle = objectHandle;

            try
            {
                JObject jObj = JObject.Parse(keyProofJson);
                keyProofObject.xrcap = new List<KeyProofAttributeValue>();
                foreach (var ele in jObj["xr_cap"])
                {
                    KeyProofAttributeValue attribute = new(ele.First.ToString(), ele.Last.ToString());
                    keyProofObject.xrcap.Add(attribute);
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("Could not find field xr_cap.");
                Debug.WriteLine(e);
            }
            return await Task.FromResult(keyProofObject);
        }
    }
}
