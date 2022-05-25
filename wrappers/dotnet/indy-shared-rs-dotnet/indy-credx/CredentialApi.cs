using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.indy_credx
{
<<<<<<<< HEAD:wrappers/dotnet/indy-shared-rs-dotnet/indy-credx/PresentationRequestApi.cs
    public static class PresentationRequestApi
========
    public static class CredentialApi
>>>>>>>> 829b8ee5125266856ce7e2e31affa6e557a79df3:wrappers/dotnet/indy-shared-rs-dotnet/indy-credx/CredentialApi.cs
    {
        public static Task<string> GenerateNonceAsync()
        {
            string result = "";
            NativeMethods.credx_generate_nonce(ref result);
            return Task.FromResult(result);
        }
    }
}