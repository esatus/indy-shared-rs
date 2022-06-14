using FluentAssertions;
using indy_shared_rs_dotnet;
using indy_shared_rs_dotnet.indy_credx;
using indy_shared_rs_dotnet.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet_test.indy_credx
{
    public class CredentialDefinitionApiTests
    {
        #region Tests for CreateCredentialDefinitionAsync
        [Test, TestCase(TestName = "CreateCredentialDefinition() returns a CredentialDefintion, CredentialDefinitionPrivate and CredentialKeyCorrectnessProof object.")]
        public async Task CreateCredentialDefinitionWorks()
        {
            //Arrange
            List<string> attrNames = new() { "gender", "age", "sex" };
            string issuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
            string schemaName = "gvt";
            string schemaVersion = "1.0";

            Schema schemaObject = await SchemaApi.CreateSchemaAsync(issuerDid, schemaName, schemaVersion, attrNames, 0);

            //Act
            (CredentialDefinition credDef, CredentialDefinitionPrivate credDefPvt, CredentialKeyCorrectnessProof keyProof) =
                await CredentialDefinitionApi.CreateCredentialDefinitionAsync(issuerDid, schemaObject, "tag", Consts.SIGNATURE_TYPE, 1);

            //Assert
            credDef.Should().BeOfType(typeof(CredentialDefinition));
            credDefPvt.Should().BeOfType(typeof(CredentialDefinitionPrivate));
            keyProof.Should().BeOfType(typeof(CredentialKeyCorrectnessProof));
        }

        private static IEnumerable<TestCaseData> CreateCredentialDefinitionCases()
        {
            yield return new TestCaseData(null, null, null, null)
                .SetName("CreateCredentialDefinition() throws SharedRsException if all arguments are null.");
            yield return new TestCaseData(null, "tag", Consts.SIGNATURE_TYPE, (byte)1)
                .SetName("CreateCredentialDefinition() throws SharedRsException if issuerDid is null.");
            yield return new TestCaseData("NcYxiDXkpYi6ov5FcYDi1e", null, Consts.SIGNATURE_TYPE, (byte)1)
                .SetName("CreateCredentialDefinition() throws SharedRsException if tag is null.");
            yield return new TestCaseData("NcYxiDXkpYi6ov5FcYDi1e", "tag", null, (byte)1)
                .SetName("CreateCredentialDefinition() throws SharedRsException if signatureType is null.");
        }

        [Test, TestCaseSource(nameof(CreateCredentialDefinitionCases))]
        public async Task CreateCredentialDefinitionThrowsException(string issuerDid, string tag, string signatureType, byte supportRevocation)
        {
            //Arrange
            List<string> attrNames = new() { "gender", "age", "sex" };
            string schemaIssuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
            string schemaName = "gvt";
            string schemaVersion = "1.0";
            Schema schemaObject = await SchemaApi.CreateSchemaAsync(schemaIssuerDid, schemaName, schemaVersion, attrNames, 0);

            //Act
            Func<Task> act = async () => await CredentialDefinitionApi.CreateCredentialDefinitionAsync(issuerDid, schemaObject, tag, signatureType, supportRevocation);

            //Assert
            await act.Should().ThrowAsync<SharedRsException>();
        }
        #endregion

        #region Tests for GetCredentialDefinitionAttributeAsync
        private static IEnumerable<TestCaseData> GetCredentialDefinitionAttributeCases()
        {
            yield return new TestCaseData("schema_id", "NcYxiDXkpYi6ov5FcYDi1e:2:gvt:1.0")
                .SetName("GetCredentialDefinitionAttribute() returns correct schema_id.");
            yield return new TestCaseData("id", "NcYxiDXkpYi6ov5FcYDi1e:3:CL:NcYxiDXkpYi6ov5FcYDi1e:2:gvt:1.0:tag")
                .SetName("GetCredentialDefinitionAttribute() returns correct id.");
        }

        [Test, TestCaseSource(nameof(GetCredentialDefinitionAttributeCases))]
        public async Task GetCredentialDefinitionAttributeWorks(string tag, string expected)
        {
            //Arrange
            List<string> attrNames = new() { "gender", "age", "sex" };
            string issuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
            string schemaName = "gvt";
            string schemaVersion = "1.0";
            Schema schemaObject = await SchemaApi.CreateSchemaAsync(issuerDid, schemaName, schemaVersion, attrNames, 0);

            (CredentialDefinition credDefObject, _, _) =
                await CredentialDefinitionApi.CreateCredentialDefinitionAsync(issuerDid, schemaObject, "tag", Consts.SIGNATURE_TYPE, 1);

            //Act
            string actual = await CredentialDefinitionApi.GetCredentialDefinitionAttributeAsync(credDefObject, tag);

            //Assert
            actual.Should().BeEquivalentTo(expected);
        }
        #endregion
    }
}
