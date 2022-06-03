using indy_shared_rs_dotnet.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.Models.Structures;

namespace indy_shared_rs_dotnet.indy_credx
{
    public class SchemaApi
    {
        public static async Task<Schema> CreateSchemaAsync(string originDid, string schemaName, string schemaVersion, List<string> attrNames, uint seqNo)
        {
            uint result = 0;
            NativeMethods.credx_create_schema(FfiStr.Create(originDid), FfiStr.Create(schemaName), FfiStr.Create(schemaVersion), FfiStrList.Create(attrNames), seqNo, ref result);
            IndyObject indyObj = new(result);
            Schema schemaObject = JsonConvert.DeserializeObject<Schema>(await indyObj.toJson(), Settings.jsonSettings);
            schemaObject.Handle = result;
            return await Task.FromResult(schemaObject);
        }

        public static async Task<string> GetSchemaAttribute(Schema schema, string attributeName)
        {
            string result = "";
            NativeMethods.credx_schema_get_attribute(schema.Handle, FfiStr.Create(attributeName), ref result);
            return await Task.FromResult(result);
        }
    }
}
