using FluentAssertions;
using indy_shared_rs_dotnet;
using indy_shared_rs_dotnet.indy_credx;
using indy_shared_rs_dotnet.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet_test.indy_credx
{
    public class RevocationApiTests
    {
        #region Tests for CreateRevocationRegistry
        [Test, TestCase(TestName = "CreateRevocationRegistryAsync() returns a CredentialDefintion, CredentialDefinitionPrivate and CredentialKeyCorrectnessProof object.")]
        public async Task CreateRevocationRegistryAsyncWorks()
        {
            //Arrange
            List<string> attrNames = new() { "gender", "age", "sex" };
            string issuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
            string schemaName = "gvt";
            string schemaVersion = "1.0";
            string testTailsPath = null;

            Schema schemaObject = await SchemaApi.CreateSchemaAsync(issuerDid, schemaName, schemaVersion, attrNames, 0);
            (CredentialDefinition credDef, CredentialDefinitionPrivate credDefPvt, CredentialKeyCorrectnessProof keyProof) =
                await CredentialDefinitionApi.CreateCredentialDefinitionAsync(issuerDid, schemaObject, "tag", Consts.SIGNATURE_TYPE, 1);

            //Act
            (RevocationRegistryDefinition revRegDefObject, RevocationRegistryDefinitionPrivate revRegDefPvtObject, RevocationRegistry revRegObject, RevocationRegistryDelta revRegDeltaObject) =
                await RevocationApi.CreateRevocationRegistryAsync(issuerDid, credDef, "test_tag", RegistryType.CL_ACCUM, IssuerType.ISSUANCE_BY_DEFAULT, 99, testTailsPath);

            //Assert
            revRegDefObject.Should().BeOfType(typeof(RevocationRegistryDefinition));
            revRegDefPvtObject.Should().BeOfType(typeof(RevocationRegistryDefinitionPrivate));
            revRegObject.Should().BeOfType(typeof(RevocationRegistry));
            revRegDeltaObject.Should().BeOfType(typeof(RevocationRegistryDelta));
        }
        #endregion

        #region Tests for UpdateRevocationRegistry
        [Test, TestCase(TestName = "UpdateRevocationRegistryAsync() works.")]
        public async Task UpdateRevocationRegistryWorks()
        {
            //Arrange
            List<string> attrNames = new() { "gender", "age", "sex" };
            string issuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
            string schemaName = "gvt";
            string schemaVersion = "1.0";
            string testTailsPath = null;

            Schema schemaObject = await SchemaApi.CreateSchemaAsync(issuerDid, schemaName, schemaVersion, attrNames, 0);

            (CredentialDefinition credDef, CredentialDefinitionPrivate credDefPvt, CredentialKeyCorrectnessProof keyProof) =
                await CredentialDefinitionApi.CreateCredentialDefinitionAsync(issuerDid, schemaObject, "tag", Consts.SIGNATURE_TYPE, 1);

            (RevocationRegistryDefinition revRegDefObject, RevocationRegistryDefinitionPrivate revRegDefPvtObject, RevocationRegistry tmpRevRegObject, RevocationRegistryDelta tmpRevRegDeltaObject) =
                await RevocationApi.CreateRevocationRegistryAsync(issuerDid, credDef, "test_tag", RegistryType.CL_ACCUM, IssuerType.ISSUANCE_BY_DEFAULT, 99, testTailsPath);

            List<long> issuedList = new List<long> { 0 };
            List<long> revokedList = new List<long> { 0 };

            //Act
            (RevocationRegistry revRegObject, RevocationRegistryDelta revRegDeltaObject) = 
                await RevocationApi.UpdateRevocationRegistryAsync(
                    revRegDefObject,
                    tmpRevRegObject,
                    issuedList,
                    revokedList,
                    testTailsPath
                    );

            //Assert
            revRegObject.Should().NotBeNull();
            revRegDeltaObject.Should().NotBeNull();
        }
        #endregion

        #region Tests for RevokeCredentialAsync
        [Test, TestCase(TestName = "RevokeCredentialAsync() works.")]
        public async Task RevokeCredentialWorks()
        {
            //Arrange

            //Act

            //Assert
        }
        #endregion

        #region Tests for MergeRevocationRegistryDeltas
        [Test, TestCase(TestName = "MergeRevocationRegistryAsync() works.")]
        public async Task MergeRevocationRegistryDeltasWorks()
        {
            //Arrange

            //Act

            //Assert
        }
        #endregion

        #region Tests for CreateOrUpdateRevocationState
        [Test, TestCase(TestName = "CreateOrUpdateRevocationStateAsync() works.")]
        public async Task CreateOrUpdateRevocationStateWorks()
        {
            //Arrange

            //Act

            //Assert
        }
        #endregion

        #region Tests for GetRevocationRegistryDefinitionAttribute
        [Test, TestCase(TestName = "GetRevocationRegistryDefinitionAttribute() works.")]
        public async Task GetRevocationRegistryDefinitionAttributeWorks()
        {
            //Arrange

            //Act

            //Assert
        }
        #endregion
    }
}
