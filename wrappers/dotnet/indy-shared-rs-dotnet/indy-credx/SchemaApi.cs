using System.Collections.Generic;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.Models.Structures;

namespace indy_shared_rs_dotnet.indy_credx
{
    public class SchemaApi
    {
        public static Task<uint> CreateSchema(FfiStr originDid, string schemaName, FfiStr schemaVersion, string[] attrNames, uint seqNo)
        {
            uint result = 0;
            NativeMethods.credx_create_schema(originDid, schemaName, schemaVersion, attrNames, seqNo, ref result);
            return Task.FromResult(result);
            //IndyObject indyObj = new(result);
            //Schema schemaObject = JsonConvert.DeserializeObject<Schema>(await indyObj.toJson());
            //schemaObject.Handle = result;
            //return await Task.FromResult(schemaObject);
        }

        public static Task<string> GetSchemaAttribute(uint objectHandle, FfiStr attributeName)
        {
            string result = "";
            NativeMethods.credx_schema_get_attribute(objectHandle, attributeName, ref result);
            return Task.FromResult(result);
        }
    }
}
