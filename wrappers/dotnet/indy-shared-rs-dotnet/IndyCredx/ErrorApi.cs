using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.IndyCredx
{
    public static class ErrorApi
    {
        /// <summary>
        /// Returns json string of the last thrown native error.
        /// </summary>
        /// <returns>Error json in form of {"code":[int],"message":[string]}.</returns>
        public static Task<string> GetCurrentErrorAsync()
        {
            string result = "";
            NativeMethods.credx_get_current_error(ref result);
            return Task.FromResult(result);
        }
    }
}
