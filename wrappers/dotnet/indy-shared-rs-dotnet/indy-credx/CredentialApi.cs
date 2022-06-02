﻿using indy_shared_rs_dotnet.Models;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.indy_credx
{
    public static class CredentialApi
    {
        //public static async Task<(Credential, RevocationRegistry, RevocationDelta)> CreateCredentialAsync()
        //{
        //    string result = NativeMethods.credx_create_credential();
        //    return await Task.FromResult(result);
        //}

        public static async Task<string> EncodeCredentialAttributesAsync(List<string> rawAttributes)
        {
            string result = "";
            string[] attributeArray = rawAttributes.ToArray();
            
            NativeMethods.credx_encode_credential_attributes(attributeArray, ref result);
            return await Task.FromResult(result);
        }

        //public static async Task ProcessCredentialAsync(IndyObject credential, 
        //    CredentialRequest credentialRequest, 
        //    MasterSecret masterSecret, 
        //    CredentialDefinition credentialDefinition,
        //    IndyObject revRegDef)
        //{
        //    IndyObject result;
        //    NativeMethods.credx_process_credential(credential.Handle, 
        //        credentialRequest.Handle,
        //        masterSecret.Handle,
        //        credentialDefinition.Handle, 
        //        revRegDef.Handle, 
        //        ref result.Handle);
        //}

        //public static async Task<string> GetCredentialAttributeAsync(IndyObject credential, string name)
        //{
        //    string result = "";
        //    NativeMethods.credx_credential_get_attribute(credential.Handle, name, ref result);
        //    return await Task.FromResult(result);
        //}
    }
}