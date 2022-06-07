using indy_shared_rs_dotnet.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.Models.Structures;

namespace indy_shared_rs_dotnet.indy_credx
{
    public static class PresentationRequestApi
    {
        public static Task<string> GenerateNonceAsync()
        {
            string result = "";
            int errorCode = NativeMethods.credx_generate_nonce(ref result);
            if (errorCode != 0)
            {
                string error = "";
                NativeMethods.credx_get_current_error(ref error);
                Debug.WriteLine(error);
            }
            return Task.FromResult(result);
        }

        public static Task<string> credx_presentation_request_from_json(string presReqJson)
        {
            uint presReqObjectHandle = 0;
            int errorCode = NativeMethods.credx_presentation_request_from_json(ByteBuffer.Create(presReqJson) ,ref presReqObjectHandle);
            if (errorCode != 0)
            {
                string error = "";
                NativeMethods.credx_get_current_error(ref error);
                Debug.WriteLine(error);
            }
            //PresentationRequest presReq = await CreatePresReqObject();
            //return Task.FromResult(result);
            return null;
        }
    }
}
