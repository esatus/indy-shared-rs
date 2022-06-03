using FluentAssertions;
using indy_shared_rs_dotnet.indy_credx;
using indy_shared_rs_dotnet.Models;
using NUnit.Framework;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.Models.Structures;

namespace indy_shared_rs_dotnet_test.indy_credx
{
    public class CredentialOfferApiTests
    {
        [Test]
        [TestCase(TestName = "CreateCredentialOffer works.")]
        public async Task CreateCredentialOfferWorks()
        {
            string[] attrNames = { "gender", "age", "sex" };
            string did = "NcYxiDXkpYi6ov5FcYDi1e";
            string schemaName = "gvt";
            string schemaVersion = "1.0";
            FfiStrList FfiAttrNames = FfiStrList.Create(attrNames);
            FfiStr FfiDid = FfiStr.Create(did);
            FfiStr FfiSchemaName = FfiStr.Create(schemaName);
            FfiStr FfiSchemaVersion = FfiStr.Create(schemaVersion);

            uint handle = await SchemaApi.CreateSchema(FfiDid, schemaName, FfiSchemaVersion, FfiAttrNames, 0);
            (CredentialDefinition credDef, CredentialDefinitionPrivate credDefPvt, CredentialKeyCorrectnessProof keyProof) =
                await CredentialDefinitionApi.CreateCredentialDefinition(did, handle, "tag", "CL", 1);

            string schemaId = await CredentialDefinitionApi.GetCredentialDefinitionAttribute(credDef.Handle,"schema_id");
            CredentialOffer testObject = await CredentialOfferApi.CreateCredentialOffer(schemaId, credDef, keyProof);

            //Assert
            testObject.Should().BeOfType(typeof(CredentialOffer));
        }
    }
}
