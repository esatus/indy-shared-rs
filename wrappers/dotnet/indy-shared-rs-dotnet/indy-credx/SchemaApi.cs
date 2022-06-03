using indy_shared_rs_dotnet.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.Models.Structures;

namespace indy_shared_rs_dotnet.indy_credx
{
    public class SchemaApi
    {
        public static async Task<Schema> CreateSchemaAsync(FfiStr originDid, FfiStr schemaName, FfiStr schemaVersion, FfiStrList attrNames, uint seqNo)
        {
            uint result = 0;
            NativeMethods.credx_create_schema(originDid, schemaName, schemaVersion, attrNames, seqNo, ref result);
            IndyObject indyObj = new(result);
            Schema schemaObject = JsonConvert.DeserializeObject<Schema>(await indyObj.toJson(), Settings.jsonSettings);
            schemaObject.Handle = result;
            return await Task.FromResult(schemaObject);
        }

        public static async Task<string> GetSchemaAttribute(uint objectHandle, FfiStr attributeName)
        {
            string result = "";
            NativeMethods.credx_schema_get_attribute(objectHandle, attributeName, ref result);
            return await Task.FromResult(result);
        }
    }
}
