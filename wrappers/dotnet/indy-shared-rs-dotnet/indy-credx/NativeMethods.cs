using indy_shared_rs_dotnet.models;
using System.Runtime.InteropServices;
using static indy_shared_rs_dotnet.models.Structures;

namespace indy_shared_rs_dotnet.indy_credx
{
    internal static class NativeMethods
    {
        #region Error
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern string credx_get_current_error(ref string error_json_p);
        #endregion

        #region Mod
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern void credx_set_default_logger();

        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern string credx_version();
        #endregion

        #region PresReq
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern string credx_generate_nonce(ref string nonce_p);
        #endregion

        #region CredDef
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_create_credential_definition(string origin_did, uint schema_handle, string tag, string signature_type, byte support_revocation,
                                                                      ref uint cred_def_p_handle, ref uint cred_def_pvt_p_handle, ref uint key_proof_p_handle);
        #endregion

        #region CredentialRequest
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern string credx_create_credential_request(string proverDid, uint credDefObjectHandle, uint masterSecretObjectHandle, string masterSecretId, uint credOfferObjectHandle, ref uint credReqObjectHandle, ref uint credReqMetaObjectHandle);
        #endregion

        #region Credential
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern string credx_create_credential(
            uint credDefObjectHandle,
            uint credDefPrivateObjectHandle,
            uint credOfferObjectHandle,
            uint credRequestObjectHandle,
            string[] attrNames,
            string[] attrRawValues,
            string[] attrEncValues,
            ref CredentialRevocationInfo revocation,
            ref uint credObjectHandle,
            ref uint revRegObjectHandle,
            ref uint revDeltaObjectHandle);

        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern string credx_encode_credential_attributes(string[] attrRawValues, ref string result);

        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern string credx_process_credential(uint credObjectHandle, uint credReqObjectHandle, uint masterSecretObjectHandle, uint credDefObjectHandle, uint revRegDefObjectHandle, ref uint resultObjectHandle);

        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern string credx_credential_get_attribute(uint ObjectHandle, string name, ref string result);
        #endregion

        #region MasterSecret
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_create_master_secret(ref uint objectHandle);
        #endregion

        #region Schema
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int credx_create_schema(string origin_did, string schema_name, string schema_version, string[] attr_names, uint seq_no, ref uint schema_p);

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
