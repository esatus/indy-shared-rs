using indy_shared_rs_dotnet.indy_credx;
using indy_shared_rs_dotnet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet_test
{
    public class TestSetup
    {/**
        public static async Task<Schema> PrepareObjectsForCredDefApiTests()
        {
            List<string> attrNames = new() { "gender", "age", "sex" };
            string issuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
            string schemaName = "gvt";
            string schemaVersion = "1.0";

            Schema schemaObject = await SchemaApi.CreateSchemaAsync(issuerDid, schemaName, schemaVersion, attrNames, 0);
            return await Task.FromResult(schemaObject);
        }

        public static async Task<CredentialDefinition> PrepareObjectsForCredApiTests()
        {
            string issuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
            Schema testSchema = await CreateTestSchema();

            (CredentialDefinition credDef, CredentialDefinitionPrivate credDefPvt, CredentialKeyCorrectnessProof keyProof) =
                await CredentialDefinitionApi.CreateCredentialDefinitionAsync(issuerDid, testSchema, "tag", "CL", 1);
        }

        public static async Task<CredentialDefinition> PrepareObjectsForCredOfferApiTests()
        {
            string issuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
            Schema testSchema = await CreateTestSchema();

            (CredentialDefinition credDef, CredentialDefinitionPrivate credDefPvt, CredentialKeyCorrectnessProof keyProof) =
                await CredentialDefinitionApi.CreateCredentialDefinitionAsync(issuerDid, testSchema, "tag", "CL", 1);
        }

        public static async Task<CredentialDefinition> PrepareObjectsForCredReqApiTests()
        {
            string issuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
            Schema testSchema = await CreateTestSchema();

            (CredentialDefinition credDef, CredentialDefinitionPrivate credDefPvt, CredentialKeyCorrectnessProof keyProof) =
                await CredentialDefinitionApi.CreateCredentialDefinitionAsync(issuerDid, testSchema, "tag", "CL", 1);
        }**/
    }
}
