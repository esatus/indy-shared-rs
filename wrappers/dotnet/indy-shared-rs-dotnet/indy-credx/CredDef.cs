using indy_shared_rs_dotnet.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet.indy_credx
{
    public class CredDef
    {
        public CredDef(string origin_did, uint schema_handle, string tag, string signature_type, byte support_revocation)
        {
            uint cred_p = 0;
            uint rev_reg_p = 0;
            uint rev_delta_p = 0;
            NativeMethods.credx_create_credential_definition(origin_did, schema_handle, tag, signature_type, support_revocation, 
                                                             ref cred_p, ref rev_reg_p, ref rev_delta_p);
            IndyObject a = new(cred_p);
            IndyObject b = new(rev_reg_p);
            IndyObject c = new(rev_delta_p);

            string typeName_a =  a.TypeName().GetAwaiter().GetResult();
            string model_a =  a.toJson().GetAwaiter().GetResult();



        }
}

}
