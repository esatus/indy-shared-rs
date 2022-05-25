using indy_shared_rs_dotnet.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.indy_credx
{
    public class Schema
    {
        public Schema(string origin_did, string schema_name, string schema_version, string[] attr_names, uint seq_no)
        {
            uint schema_p = 0;
            NativeMethods.credx_create_schema(ref origin_did, schema_name, schema_version, attr_names, seq_no, ref schema_p);

            //IndyObject a = new(schema_p);
           // string typeName_a = a.TypeName().GetAwaiter().GetResult();
            //string model_a = a.toJson().GetAwaiter().GetResult();
        }
    }
}
