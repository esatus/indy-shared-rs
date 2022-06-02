﻿using indy_shared_rs_dotnet.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.indy_credx
{
    public static class CredentialOfferApi
    {
        public static async Task<CredentialOffer> CreateCredentialOffer(
            string schemaId,
            CredentialDefinition credDefObject,
            CredentialKeyCorrectnessProof keyProofObject)
        {
            uint credOfferObjectHandle = 0;
            NativeMethods.credx_create_credential_offer(schemaId, credDefObject.Handle, keyProofObject.Handle, ref credOfferObjectHandle);
            CredentialOffer credOfferObject = await CreateCredentialOfferObject(credOfferObjectHandle);
            return await Task.FromResult(credOfferObject);
        }

        private static async Task<CredentialOffer> CreateCredentialOfferObject(uint objectHandle)
        {
            IndyObject indyObject = new(objectHandle);
            string credOfferJson = await indyObject.toJson();
            CredentialOffer credOfferObject = JsonConvert.DeserializeObject<CredentialOffer>(credOfferJson);
            credOfferObject.Handle = objectHandle;

            try
            {
                JObject jObj = JObject.Parse(credOfferJson);
                credOfferObject.KeyCorrectnessProof.xrcap = new List<KeyProofAttributeValue>();
                foreach (var ele in jObj["key_correctness_proof"]["xr_cap"])
                {
                    KeyProofAttributeValue attribute = new(ele.First.ToString(), ele.Last.ToString());
                    credOfferObject.KeyCorrectnessProof.xrcap.Add(attribute);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Could not find field xr_cap.");
                Debug.WriteLine(e);
            }

            return await Task.FromResult(credOfferObject);
        }
    }
}