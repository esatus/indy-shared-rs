using indy_shared_rs_dotnet;
using System.Runtime.InteropServices;

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
    }
}
