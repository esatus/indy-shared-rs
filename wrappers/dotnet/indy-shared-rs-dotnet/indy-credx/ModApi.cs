using System.Diagnostics;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.indy_credx
{
    public static class ModApi
    {
        public static void SetDefaultLogger()
        {
            int errorCode = NativeMethods.credx_set_default_logger();
            if (errorCode != 0)
            {
                string error = "";
                NativeMethods.credx_get_current_error(ref error);
                Debug.WriteLine(error);
            }
        }

        public static Task<string> GetVersionAsync()
        {
            string result = NativeMethods.credx_version();
            return Task.FromResult(result);
        }
    }
}
