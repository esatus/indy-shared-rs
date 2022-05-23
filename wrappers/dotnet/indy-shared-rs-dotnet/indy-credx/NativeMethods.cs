using indy_shared_rs_dotnet;
using System.Runtime.InteropServices;

namespace indy_shared_rs_dotnet.indy_credx
{
    internal static class NativeMethods
    {
        #region PresReq
        [DllImport(Consts.CREDX_LIB_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern string credx_generate_nonce(ref string nonce_p);
        #endregion
    }
}
