using indy_shared_rs_dotnet.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.Models.Structures;

namespace indy_shared_rs_dotnet.indy_credx
{
    public static class CredentialOfferApi
    {
        public static async Task<CredentialOffer> CreateCredentialOfferAsync(
            string schemaId,
            CredentialDefinition credDefObject,
            CredentialKeyCorrectnessProof keyProofObject)
        {
            uint credOfferObjectHandle = 0;
            int errorCode = NativeMethods.credx_create_credential_offer(FfiStr.Create(schemaId), credDefObject.Handle, keyProofObject.Handle, ref credOfferObjectHandle);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw new SharedRsException(JsonConvert.DeserializeObject<Dictionary<string, string>>(error)["message"]);
            }

            CredentialOffer credOfferObject = await CreateCredentialOfferObject(credOfferObjectHandle);
            return await Task.FromResult(credOfferObject);
        }

        private static async Task<CredentialOffer> CreateCredentialOfferObject(uint objectHandle)
        {
            string credOfferJson = await ObjectApi.ToJson(objectHandle);
            CredentialOffer credOfferObject = JsonConvert.DeserializeObject<CredentialOffer>(credOfferJson, Settings.jsonSettings);
            credOfferObject.Handle = objectHandle;

            try
            {
                JObject jObj = JObject.Parse(credOfferJson);
                credOfferObject.KeyCorrectnessProof.xrcap = new List<KeyProofAttributeValue>();
                foreach (JToken ele in jObj["key_correctness_proof"]["xr_cap"])
                {
                    KeyProofAttributeValue attribute = new(ele.First.ToString(), ele.Last.ToString());
                    credOfferObject.KeyCorrectnessProof.xrcap.Add(attribute);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not find field xr_cap.");
                Console.WriteLine(e);
            }

            return await Task.FromResult(credOfferObject);
        }
    }
}
