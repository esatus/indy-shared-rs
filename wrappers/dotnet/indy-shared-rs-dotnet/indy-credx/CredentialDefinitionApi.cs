using indy_shared_rs_dotnet.models;

namespace indy_shared_rs_dotnet.indy_credx
{
    public class CredentialDefinitionApi
    {
        public CredentialDefinitionApi(string originDid, uint schemaHandle, string tag, string signatureType, byte supportRevocation)
        {
            uint credDefHandle = 0;
            uint credDefPvtHandle = 0;
            uint keyProofHandle = 0;
            NativeMethods.credx_create_credential_definition(originDid, schemaHandle, tag, signatureType, supportRevocation, 
                                                             ref credDefHandle, ref credDefPvtHandle, ref keyProofHandle);
            IndyObject indyObjCredDef = new(credDefHandle);
            IndyObject indyObjCredDefPvt = new(credDefPvtHandle);
            IndyObject indyObjKeyProof = new(keyProofHandle);

           //CredDef typeName_a = indyObjCredDef.TypeName().GetAwaiter().GetResult();
            //string model_a =  a.toJson().GetAwaiter().GetResult();



        }
}

}
