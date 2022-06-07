using indy_shared_rs_dotnet.Models;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static indy_shared_rs_dotnet.Models.Structures;

namespace indy_shared_rs_dotnet.indy_credx
{
    internal static class NativeMethods
    {
        #region Error
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_get_current_error(ref string error_json_p);
        #endregion

        #region Mod
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_set_default_logger();

        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern string credx_version();
        #endregion

        #region PresReq
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_generate_nonce(ref string nonce_p);
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_presentation_request_from_json(ByteBuffer presentationRequestJson, ref uint presentationRequestObjectHandle);
        #endregion

        #region CredentialDefinition
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_create_credential_definition(FfiStr originDid, uint schemaObjectHandle, FfiStr tag, FfiStr signatureType, byte supportRevocation,
                                                                         ref uint credDefObjectHandle, ref uint credDefPvtObjectHandle, ref uint keyProofObjectHandle);
        
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_credential_definition_get_attribute(uint handle, FfiStr name, ref string result_p);
        #endregion
        
        #region CredentialOffer 
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_create_credential_offer(FfiStr schemaId, uint credDefObjectHandle, uint keyProofObjectHandle, ref uint credOfferHandle);
        #endregion

        #region CredentialRequest
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_create_credential_request(FfiStr proverDid, uint credDefObjectHandle, uint masterSecretObjectHandle, FfiStr masterSecretId, uint credOfferObjectHandle, ref uint credReqObjectHandle, ref uint credReqMetaObjectHandle);
        #endregion

        #region Credential
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_create_credential(
            uint credDefObjectHandle,
            uint credDefPrivateObjectHandle,
            uint credOfferObjectHandle,
            uint credRequestObjectHandle,
            FfiStrList attrNames,
            FfiStrList attrRawValues,
            FfiStrList attrEncValues,
            ref CredentialRevocationInfo revocation,
            ref uint credObjectHandle,
            ref uint revRegObjectHandle,
            ref uint revDeltaObjectHandle);

        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_encode_credential_attributes(FfiStrList attrRawValues, ref string result);

        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_process_credential(uint credObjectHandle, uint credReqObjectHandle, uint masterSecretObjectHandle, uint credDefObjectHandle, uint revRegDefObjectHandle, ref uint resultObjectHandle);

        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_credential_get_attribute(uint ObjectHandle, FfiStr name, ref string result);
        #endregion

        #region MasterSecret
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_create_master_secret(ref uint objectHandle);
        #endregion

        #region Schema
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int credx_create_schema(FfiStr origin_did, FfiStr schema_name, FfiStr schema_version, FfiStrList attr_names, uint seq_no, ref uint schema_p);
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_schema_get_attribute(uint handle, FfiStr name, ref string result_p);

        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_schema_from_json(ByteBuffer schemaJson, ref uint schemaHandle);

        #endregion

        #region ObjectHandle
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_object_get_type_name(uint handle, ref string result);

        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_object_get_json(uint handle, ref ByteBuffer result);
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_object_free(uint handle);
        #endregion
    }
}
