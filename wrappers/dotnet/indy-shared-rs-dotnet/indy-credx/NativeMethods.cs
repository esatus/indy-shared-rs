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
        internal static extern int credx_get_current_error(ref string errorJson);
        #endregion

        #region Mod
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_set_default_logger();
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern string credx_version();
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_buffer_free(ByteBuffer byteBuffer);
        #endregion

        #region PresReq
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_generate_nonce(ref string nonce);
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_presentation_request_from_json(ByteBuffer presentationRequestJson, ref uint presentationRequestObjectHandle);
        #endregion

        #region CredentialDefinition
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_create_credential_definition(FfiStr originDid, uint schemaObjectHandle, FfiStr tag, FfiStr signatureType, byte supportRevocation,
                                                                         ref uint credDefObjectHandle, ref uint credDefPvtObjectHandle, ref uint keyProofObjectHandle);
        
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_credential_definition_get_attribute(uint credDefObjectHandle, FfiStr attributeName, ref string result);

        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_credential_definition_from_json(ByteBuffer credDefJson, ref uint credDefObjectHandle);
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
            FfiCredRevInfo revocation,
            ref uint credObjectHandle,
            ref uint revRegObjectHandle,
            ref uint revDeltaObjectHandle);

        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_encode_credential_attributes(FfiStrList attrRawValues, ref string result);

        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_process_credential(uint credObjectHandle, uint credReqObjectHandle, uint masterSecretObjectHandle, uint credDefObjectHandle, uint revRegDefObjectHandle, ref uint resultObjectHandle);

        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_credential_get_attribute(uint credObjectHandle, FfiStr attributeName, ref string result);
        #endregion

        #region MasterSecret
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_create_master_secret(ref uint masterSecretObjectHandle);
        #endregion

        #region Presentation
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_create_presentation(
            uint presReqObjectHandle,
            FfiCredentialEntryList credentials,
            FfiCredentialProveList credentialsProof,
            FfiStrList selfAttestNames,
            FfiStrList selfAttestValues,
            uint masterSecret,
            FfiUIntList schemas,
            FfiUIntList credDefs,
            ref uint presentationObjectHandle);

        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_presentation_from_json(ByteBuffer presentationJson, ref uint presentationObjectHandle);

        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_verify_presentation(
            uint presObjectHandle,
            uint presReqObjectHandle,
            FfiUIntList schemaObjectHandles,
            FfiUIntList credDefObjectHandles,
            FfiUIntList revRegDefObjectHandles,
            FfiRevocationEntryList revRegEntries, 
            ref byte verifyResult);
        #endregion

        #region Revocation
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_create_revocation_registry(
            FfiStr originDid, 
            uint credDefObjectHandle, 
            FfiStr tag, 
            FfiStr revRegType, 
            FfiStr issuanceType, 
            long maxCredNumber, 
            FfiStr tailsDirPath,
            ref uint regDefObjectHandle,
            ref uint regDefPvtObjectHandle, 
            ref uint regEntryObjectHandle, 
            ref uint regInitDeltaObjectHandle);

        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_revocation_registry_from_json(ByteBuffer revRegJson, ref uint revRegObjectHandle);

        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_revocation_registry_definition_from_json(ByteBuffer revRegJson, ref uint revRegObjectHandle);

        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_update_revocation_registry(
            uint revRegDefObjectHandle,
            uint revRegObjectHandle,
            FfiLongList issued,
            FfiLongList revoked,
            FfiStr tailsPath,
            ref uint revRegUpdatedObjectHandle,
            ref uint revRegDeltaObjectHandle);

        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_revoke_credential(
            uint revRegDefObjectHandle,
            uint revRegObjectHandle,
            long credRevIdx,
            FfiStr tailsPath,
            ref uint revRegUpdatedObjectHandle,
            ref uint revRegDeltaObjectHandle);

        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_revocation_registry_definition_get_attribute(
            uint regDefObjectHandle,
            FfiStr attributeName,
            ref string result);

        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_merge_revocation_registry_deltas(
            uint revRegDelta1ObjectHandle,
            uint revRegDelta2ObjectHandle,
            ref uint revRegDeltaNewObjectHandle);

        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_create_or_update_revocation_state(
            uint revRegDefObjectHandle,
            uint revRegDeltaObjectHandle,
            long revRegIndex,
            long timestamp,
            FfiStr tailsPath,
            uint revStateObjectHandle,
            ref uint revStateNewObjectHandle);

        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_revocation_state_from_json(ByteBuffer credentialRevocationStateJson, ref uint credentialRevocationStateObjectHandle);
        #endregion

        #region Schema
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int credx_create_schema(FfiStr originDid, FfiStr schemaName, FfiStr schemaVersion, FfiStrList attrNames, uint seqNo, ref uint schemaObjectHandle);

        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_schema_get_attribute(uint schemaObjectHandle, FfiStr attributeName, ref string result);

        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_schema_from_json(ByteBuffer schemaJson, ref uint schemaObjectHandle);

        #endregion

        #region ObjectHandle
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_object_get_type_name(uint objectHandle, ref string result);

        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_object_get_json(uint objectHandle, ref ByteBuffer result);
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_object_free(uint objectHandle);
        #endregion
    }
}
