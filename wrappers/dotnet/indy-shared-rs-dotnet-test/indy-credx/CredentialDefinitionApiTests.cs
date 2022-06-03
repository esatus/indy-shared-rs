using FluentAssertions;
using indy_shared_rs_dotnet.indy_credx;
using indy_shared_rs_dotnet.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.Models.Structures;

namespace indy_shared_rs_dotnet_test.indy_credx
{
    public class CredentialDefinitionApiTests
    {
        [Test]
        [TestCase(TestName = "CreateCredentialDefinition works.")]
        public async Task CreateCredentialDefinitionWorks()
        {
            List<string> attrNames = new() { "gender", "age", "sex" };
            string issuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
            string schemaName = "gvt";
            string schemaVersion = "1.0";

            Schema schemaObject = await SchemaApi.CreateSchemaAsync(issuerDid, schemaName, schemaVersion, attrNames, 0);
            (CredentialDefinition credDef, CredentialDefinitionPrivate credDefPvt, CredentialKeyCorrectnessProof keyProof) =
                await CredentialDefinitionApi.CreateCredentialDefinitionAsync(issuerDid, schemaObject, "tag", "CL", 1);

            string schemaId = await CredentialDefinitionApi.GetCredentialDefinitionAttribute(credDef, "schema_id");
            CredentialOffer testObject = await CredentialOfferApi.CreateCredentialOfferAsync(schemaId, credDef, keyProof);

            //Assert
            credDef.Should().BeOfType(typeof(CredentialDefinition));
            credDefPvt.Should().BeOfType(typeof(CredentialDefinitionPrivate));
            keyProof.Should().BeOfType(typeof(CredentialKeyCorrectnessProof));
        }
    }
}
