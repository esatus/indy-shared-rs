using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.indy_credx
{
    public static class Mod
    {
        public static void SetDefaultLogger()
        {
            NativeMethods.credx_set_default_logger();
        }

        public static Task<string> GetVersionAsync()
        {
            string result = NativeMethods.credx_version();
            return Task.FromResult(result);
        }
    }
}
