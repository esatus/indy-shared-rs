﻿using indy_shared_rs_dotnet.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.Models.Structures;

namespace indy_shared_rs_dotnet.IndyCredx
{
    public static class PresentationApi
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="presentationRequest"></param>
        /// <param name="credentialEntries"></param>
        /// <param name="credentialProves"></param>
        /// <param name="selfAttestNames"></param>
        /// <param name="selfAttestValues"></param>
        /// <param name="masterSecret"></param>
        /// <param name="schemas"></param>
        /// <param name="credDefs"></param>
        /// <exception cref="SharedRsException"></exception>
        /// <returns>New presentation object.</returns>
        public static async Task<Presentation> CreatePresentationAsync(
            PresentationRequest presentationRequest,
            List<CredentialEntry> credentialEntries,
            List<CredentialProof> credentialProves,
            List<string> selfAttestNames,
            List<string> selfAttestValues,
            MasterSecret masterSecret,
            List<Schema> schemas,
            List<CredentialDefinition> credDefs)
        {
            uint presentationObjectHandle = 0;
            List<uint> schemaHandles = (from schema in schemas
                                        select schema.Handle).ToList();
            List<uint> credDefHandles = (from credDef in credDefs
                                         select credDef.Handle).ToList();

            int errorCode = NativeMethods.credx_create_presentation(
                presentationRequest.Handle,
                FfiCredentialEntryList.Create(credentialEntries),
                FfiCredentialProveList.Create(credentialProves),
                FfiStrList.Create(selfAttestNames),
                FfiStrList.Create(selfAttestValues),
                masterSecret.Handle,
                FfiUIntList.Create(schemaHandles),
                FfiUIntList.Create(credDefHandles),
                ref presentationObjectHandle);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            Presentation presentationObject = await CreatePresentationObject(presentationObjectHandle);
            return await Task.FromResult(presentationObject);
        }

        /// <summary>
        /// Creates a presentation object from json string.
        /// </summary>
        /// <param name="presentationJson">Json string of presentation object.</param>
        /// <exception cref="IndexOutOfRangeException">Throws when json string is empty.</exception>
        /// <exception cref="SharedRsException">Throws when json string is invalid.</exception>
        /// <returns>New presentation object.</returns>
        public static async Task<Presentation> CreatePresentationFromJsonAsync(string presentationJson)
        {
            uint presentationObjectHandle = 0;
            int errorCode = NativeMethods.credx_presentation_from_json(ByteBuffer.Create(presentationJson), ref presentationObjectHandle);
            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }
            Presentation presentationObject = await CreatePresentationObject(presentationObjectHandle);
            return await Task.FromResult(presentationObject);
        }

        /// <summary>
        /// Verifies that a presentation matches its request.
        /// </summary>
        /// <param name="presentation"></param>
        /// <param name="presentationRequest"></param>
        /// <param name="schemas"></param>
        /// <param name="credentialDefinitions"></param>
        /// <param name="revocationRegistryDefinitions"></param>
        /// <param name="revocationRegistryEntries"></param>
        /// <exception cref="SharedRsException"></exception>
        /// <returns></returns>
        public static async Task<bool> VerifyPresentationAsync(
            Presentation presentation,
            PresentationRequest presentationRequest,
            List<Schema> schemas,
            List<CredentialDefinition> credentialDefinitions,
            List<RevocationRegistryDefinition> revocationRegistryDefinitions,
            List<RevocationRegistryEntry> revocationRegistryEntries)
        {
            byte verify = 0;
            List<uint> schemaHandles =
                (from schema in schemas select schema.Handle).ToList();
            List<uint> credDefHandles =
                (from credDef in credentialDefinitions select credDef.Handle).ToList();
            List<uint> revRegDefHandles =
                (from revRegDef in revocationRegistryDefinitions select revRegDef.Handle).ToList();

            int errorCode = NativeMethods.credx_verify_presentation(
                presentation.Handle,
                presentationRequest.Handle,
                FfiUIntList.Create(schemaHandles),
                FfiUIntList.Create(credDefHandles),
                FfiUIntList.Create(revRegDefHandles),
                FfiRevocationEntryList.Create(revocationRegistryEntries),
                ref verify);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                throw SharedRsException.FromSdkError(error);
            }

            return await Task.FromResult(Convert.ToBoolean(verify));
        }

        private static async Task<Presentation> CreatePresentationObject(uint objectHandle)
        {
            string presentationJson = await ObjectApi.ToJsonAsync(objectHandle);
            Presentation presentationObject = JsonConvert.DeserializeObject<Presentation>(presentationJson, Settings.JsonSettings);

            presentationObject.Handle = objectHandle;
            return await Task.FromResult(presentationObject);
        }
    }
}