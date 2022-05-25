using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.indy_credx
{
    public static class CredentialApi
    {
        public static Task<string> GenerateNonceAsync()
        {
            string result = "";
            NativeMethods.credx_generate_nonce(ref result);
            return Task.FromResult(result);
        }
    }
}