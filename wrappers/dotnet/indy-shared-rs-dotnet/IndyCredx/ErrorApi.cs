using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.IndyCredx
{
    public static class ErrorApi
    {
        public static Task<string> GetCurrentErrorAsync()
        {
            string result = "";
            NativeMethods.credx_get_current_error(ref result);
            return Task.FromResult(result);
        }
    }
}
