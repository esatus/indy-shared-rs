using indy_shared_rs_dotnet.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
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

        public static async Task<PresentationRequest> CreatePresReqFromJsonAsync(string presReqJson)
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

            presentationRequestObject.RequestedAttributes = new Dictionary<string, AttributeInfo>();
            presentationRequestObject.RequestedPredicates = new Dictionary<string, PredicateInfo>();

            JObject presReqJObject = JObject.Parse(presReqJson);
            foreach(JToken element in presReqJObject["requested_attributes"])
            {
                AttributeInfo info;
                try
                {
                    info = JsonConvert.DeserializeObject<AttributeInfo>(element.First.ToString());
                }
                catch
                {
                    continue;
                }
                string key = element.Path.Split('.')[1];
                presentationRequestObject.RequestedAttributes.Add(key.ToString(), info);
            }
            foreach (JToken element in presReqJObject["requested_predicates"])
            {
                PredicateInfo info;
                try
                {
                   
                }
                catch (System.Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    continue;
                }

                //PredicateInfo info;
                //try
                //{
                //    //TODO: If Restrictions, create new AttributeFilter on $or and property on $and.
                //    var restrictions = element.Values();
                //    foreach (JProperty restriction in restrictions)
                //    {
                //        var x = restriction;
                //    }

                //    info = JsonConvert.DeserializeObject<PredicateInfo>(element.First.ToString());
                //}
                //catch(System.Exception ex)
                //{
                //    Debug.WriteLine(ex.Message);
                //    continue;
                //}
                //string key = element.Path.Split('.')[1];
                //presentationRequestObject.RequestedPredicates.Add(key.ToString(), info);
            }

            presentationRequestObject.Handle = objectHandle;
            return await Task.FromResult(presentationRequestObject);
        }
    }
}
