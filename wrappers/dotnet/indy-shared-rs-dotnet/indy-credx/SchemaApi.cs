using indy_shared_rs_dotnet.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.Models.Structures;

namespace indy_shared_rs_dotnet.indy_credx
{
    public class SchemaApi
    {
        public static async Task<Schema> CreateSchemaAsync(string originDid, string schemaName, string schemaVersion, List<string> attrNames, uint seqNo)
        {
            uint schemaObjectHandle = 0;
            int errorCode = NativeMethods.credx_create_schema(FfiStr.Create(originDid), FfiStr.Create(schemaName), FfiStr.Create(schemaVersion), FfiStrList.Create(attrNames), seqNo, ref schemaObjectHandle);
            
            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                Debug.WriteLine(error);
            }

            Schema schemaObject = await CreateSchemaObject(schemaObjectHandle);
            return await Task.FromResult(schemaObject);
        }

        public static async Task<Schema> CreateSchemaFromJsonAsync(string schemaJson)
        {
            uint schemaObjectHandle = 0;
            int errorCode = NativeMethods.credx_schema_from_json(ByteBuffer.Create(schemaJson), ref schemaObjectHandle);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                Debug.WriteLine(error);
            }

            Schema schemaObject = await CreateSchemaObject(schemaObjectHandle);
            return await Task.FromResult(schemaObject);
        }

        public static async Task<string> GetSchemaAttribute(Schema schema, string attributeName)
        {
            string result = "";
            //note: only "id" as attributeName supported so far.
            int errorCode = NativeMethods.credx_schema_get_attribute(schema.Handle, FfiStr.Create(attributeName), ref result);

            if (errorCode != 0)
            {
                string error = await ErrorApi.GetCurrentErrorAsync();
                Debug.WriteLine(error);
            }

            return await Task.FromResult(result);
        }

        private static async Task<Schema> CreateSchemaObject(uint objectHandle)
        {
            string schemaJson = await ObjectApi.ToJson(objectHandle);
            Schema schemaObject = JsonConvert.DeserializeObject<Schema>(schemaJson, Settings.jsonSettings);
            schemaObject.Handle = objectHandle;
            return await Task.FromResult(schemaObject);
        }
    }
}
