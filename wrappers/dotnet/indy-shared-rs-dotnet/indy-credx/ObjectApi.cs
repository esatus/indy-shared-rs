using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.Models.Structures;

namespace indy_shared_rs_dotnet.indy_credx
{
    public static class ObjectApi
    {
        public static async Task<string> GetTypeName(uint objectHandle)
        {
            return await ObjectGetTypeName(objectHandle);
        }

        public static async Task FreeObject(uint objectHandle)
        {
            int errorCode = NativeMethods.credx_object_free(objectHandle);
            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                Debug.WriteLine(error);
            }
        }

        public static async unsafe Task<string> ToJson(uint objectHandle)
        {
            ByteBuffer byteBuffer = ObjectGetJson(objectHandle).GetAwaiter().GetResult();
            string decoded = DecodeToString(byteBuffer).GetAwaiter().GetResult();
            return Task.FromResult(decoded).GetAwaiter().GetResult();
        }

        private static async Task<string> ObjectGetTypeName(uint handle)
        {
            string result = "";
            int errorCode = NativeMethods.credx_object_get_type_name(handle, ref result);
            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                Debug.WriteLine(error);
            }
            return await Task.FromResult(result);

        }
        private static async unsafe Task<ByteBuffer> ObjectGetJson(uint handle)
        {
            ByteBuffer result = new()
            {
                len = 0,
                value = null
            };

            int errorCode = NativeMethods.credx_object_get_json(handle, ref result);
            if (errorCode != 0)
            {
                string error = ErrorApi.GetCurrentErrorAsync().GetAwaiter().GetResult();
                Debug.WriteLine(error);
            }
            return Task.FromResult(result).GetAwaiter().GetResult();
        }

        private static async unsafe Task<string> DecodeToString(ByteBuffer byteBuffer)
        {
            char[] charArray = new char[byteBuffer.len];
            UTF8Encoding utf8Decoder = new UTF8Encoding(true, true);

            fixed (char* char_ptr = &charArray[0])
            {
                utf8Decoder.GetChars(byteBuffer.value, (int)byteBuffer.len, char_ptr, (int)byteBuffer.len);
            }
            return Task.FromResult(new string(charArray)).GetAwaiter().GetResult();
        }
    }
}
