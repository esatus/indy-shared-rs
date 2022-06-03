using FluentAssertions;
using indy_shared_rs_dotnet.indy_credx;
using indy_shared_rs_dotnet.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.Models.Structures;

namespace indy_shared_rs_dotnet_test.indy_credx
{
    public class CredentialRequestApiTests
    {
        [Test, TestCase(TestName = "CreateCredentialRequestAsync with all Arguments set returns a request and metadata.")]
        public async Task CreateCredentialRequestAsyncWorks()
        {
            //Arrange
            List<string> attrNames = new() { "gender", "age", "sex" };
            string issuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
            string proverDid = "VsKV7grR1BUE29mG2Fm2kX";
            string schemaName = "gvt";
            string schemaVersion = "1.0";

            MasterSecret masterSecretObject = await MasterSecretApi.CreateMasterSecretAsync();
            Schema schemaObject = await SchemaApi.CreateSchemaAsync(issuerDid, schemaName, schemaVersion, attrNames, 0);

            (CredentialDefinition credDefObject, _ , CredentialKeyCorrectnessProof keyProofObject ) =
                await CredentialDefinitionApi.CreateCredentialDefinitionAsync(issuerDid, schemaObject, "tag", "CL", 1);
            string schemaId = await CredentialDefinitionApi.GetCredentialDefinitionAttribute(credDefObject, "schema_id");
            CredentialOffer credOfferObject = await CredentialOfferApi.CreateCredentialOfferAsync(schemaId, credDefObject, keyProofObject);

            //Act
            (CredentialRequest request, CredentialRequestMetadata metaData) = await CredentialRequestApi.CreateCredentialRequestAsync(proverDid, credDefObject, masterSecretObject, "testMasterSecretName", credOfferObject);

            //Assert
            request.Should().NotBeNull();
            request.Should().BeOfType(typeof(CredentialRequest));
            metaData.Should().NotBeNull();
            metaData.Should().BeOfType(typeof(CredentialRequestMetadata));
        }
    }
}
