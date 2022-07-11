using indy_shared_rs_dotnet.Models;
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
        /// Creates a new <see cref="Presentation"/> object from parameters.
        /// </summary>
        /// <param name="presentationRequest">Presentation request.</param>
        /// <param name="credentialEntries">Credential entries.</param>
        /// <param name="credentialProofs">Credential proofs.</param>
        /// <param name="selfAttestNames">Names of self attested attributes.</param>
        /// <param name="selfAttestValues">Values of self attested attributes.</param>
        /// <param name="masterSecret">Master secret.</param>
        /// <param name="schemas">Corresponding schemas.</param>
        /// <param name="credDefs">Credential definitions.</param>
        /// <exception cref="SharedRsException">Throws when any parameters are invalid.</exception>
        /// <returns>New <see cref="Presentation"/> object.</returns>
        public static async Task<Presentation> CreatePresentationAsync(
            PresentationRequest presentationRequest,
            List<CredentialEntry> credentialEntries,
            List<CredentialProof> credentialProofs,
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
                FfiCredentialProveList.Create(credentialProofs),
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
        /// Creates a <see cref="Presentation"/> object from json <see cref="System.String"/>.
        /// </summary>
        /// <param name="presentationJson">Json string of presentation object.</param>
        /// <exception cref="IndexOutOfRangeException">Throws when <paramref name="presentationJson"/> is empty.</exception>
        /// <exception cref="SharedRsException">Throws if <paramref name="presentationJson"/> is an invalid json object.</exception>
        /// <returns>New <see cref="Presentation"/> object.</returns>
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
        /// <param name="presentation">Presentation to verify.</param>
        /// <param name="presentationRequest">Request to verify the <paramref name="presentation"/> object with.</param>
        /// <param name="schemas">Corresponding schemas.</param>
        /// <param name="credentialDefinitions"></param>
        /// <param name="revocationRegistryDefinitions"></param>
        /// <param name="revocationRegistryEntries"></param>
        /// <exception cref="SharedRsException">Throws if any parameter is invalid.</exception>
        /// <returns>True if provided <see cref="Presentation"/> can be verified, false if not.</returns>
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