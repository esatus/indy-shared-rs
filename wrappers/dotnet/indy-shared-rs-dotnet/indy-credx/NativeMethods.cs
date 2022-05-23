using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace indy_shared_rs_dotnet.indy_credx
{
    internal static class NativeMethods
    {
        #region PresReq
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern string credx_generate_nonce(ref string nonce_p);
        #endregion

        #region CredReq
        //[DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        //internal static extern string credx_create_credential_request(string proverDid, uint credDefObjectHandle, uint masterSecretObjectHandle, string masterSecretIdObjectHandle, uint credOfferObjectHandle, ref uint credReqPObjectHandle, ref uint credReqMetaPObjectHandle);
        #endregion

        #region Credentials
        //[DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        //internal static extern string credx_create_credential();

        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern string credx_encode_credential_attributes(List<string> attr_raw_values, ref string result_p); //(string[] attr_raw_values, ref string result_p);

        //[DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        //internal static extern string credx_process_credential(uint credObjectHandle, uint credReqObjectHandle, uint masterSecretObjectHandle, uint credDefObjectHandle, uint revRegDefObjectHandle, ref uint resultObjectHandle);

        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern string credx_credential_get_attribute(uint ObjectHandle, string name, ref string result_p);
        #endregion
    }
}
