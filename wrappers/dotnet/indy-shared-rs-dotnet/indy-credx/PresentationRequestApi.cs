using indy_shared_rs_dotnet.Models;
using Newtonsoft.Json;
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

        public static async Task<PresentationRequest> credx_presentation_request_from_json(string presReqJson)
        {
            uint presReqObjectHandle = 0;
            int errorCode = NativeMethods.credx_presentation_request_from_json(ByteBuffer.Create(presReqJson) ,ref presReqObjectHandle);
            if (errorCode != 0)
            {
                string error = "";
                NativeMethods.credx_get_current_error(ref error);
                Debug.WriteLine(error);
            }
            PresentationRequest presReq = await CreatePresentationRequestObject(presReqObjectHandle);
            return await Task.FromResult(presReq);
        }
        private static async Task<PresentationRequest> CreatePresentationRequestObject(uint objectHandle)
        {
            string presReqJson = await ObjectApi.ToJson(objectHandle);
            PresentationRequest presentationRequestObject = JsonConvert.DeserializeObject<PresentationRequest>(presReqJson, Settings.jsonSettings);
            presentationRequestObject.Handle = objectHandle;
            return await Task.FromResult(presentationRequestObject);
        }
    }
}
