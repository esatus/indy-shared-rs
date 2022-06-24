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
                await CredentialDefinitionApi.CreateCredentialDefinitionAsync(issuerDid, schemaObject, "tag", SignatureType.CL, 1);

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

            (CredentialDefinition credDef, _, _) =
                await CredentialDefinitionApi.CreateCredentialDefinitionAsync(issuerDid, schemaObject, "tag", SignatureType.CL, 1);

            (RevocationRegistryDefinition revRegDefObject, _, RevocationRegistry tmpRevRegObject, _) =
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
                    revRegDefObject.Value.TailsLocation
                    );

            //Assert
            revRegObject.Value.Should().NotBeSameAs(tmpRevRegObject.Value);
            revRegDeltaObject.Should().NotBeNull();
        }
        #endregion

        #region Tests for RevokeCredentialAsync
        [Test, TestCase(TestName = "RevokeCredentialAsync() works.")]
        public async Task RevokeCredentialWorks()
        {
            //Arrange
            List<string> attrNames = new() { "gender", "age", "sex" };
            string issuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
            string schemaName = "gvt";
            string schemaVersion = "1.0";
            string testTailsPath = null;

            Schema schemaObject = await SchemaApi.CreateSchemaAsync(issuerDid, schemaName, schemaVersion, attrNames, 0);
            (CredentialDefinition credDef, 
                _, 
                _) = await CredentialDefinitionApi.CreateCredentialDefinitionAsync(issuerDid, schemaObject, "tag", SignatureType.CL, 1);
            
            (RevocationRegistryDefinition revRegDefObject, 
                _, 
                RevocationRegistry tmpRevRegObject, 
                _) = await RevocationApi.CreateRevocationRegistryAsync(issuerDid, credDef, "test_tag", RegistryType.CL_ACCUM, IssuerType.ISSUANCE_BY_DEFAULT, 99, testTailsPath);

            //Act
            (_, RevocationRegistryDelta actual) = 
                await RevocationApi.RevokeCredentialAsync(
                    revRegDefObject,
                    tmpRevRegObject,
                    0,
                    revRegDefObject.Value.TailsLocation
                    );

            //Assert
            actual.Value.PrevAccum.Should().NotBeNull();
            actual.Value.Revoked.Should().HaveCount(1);
        }
        #endregion

        #region Tests for MergeRevocationRegistryDeltas
        [Test, TestCase(TestName = "MergeRevocationRegistryAsync() works.")]
        public async Task MergeRevocationRegistryDeltasWorks()
        {
            //Arrange
            List<string> attrNames = new() { "gender", "age", "sex" };
            string issuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
            string schemaName = "gvt";
            string schemaVersion = "1.0";
            string testTailsPath = null;

            Schema schemaObject = await SchemaApi.CreateSchemaAsync(issuerDid, schemaName, schemaVersion, attrNames, 0);

            (CredentialDefinition credDef, _, _) =
                await CredentialDefinitionApi.CreateCredentialDefinitionAsync(issuerDid, schemaObject, "tag", SignatureType.CL, 1);

            (RevocationRegistryDefinition revRegDefObject, _, RevocationRegistry revRegObject, _) =
                await RevocationApi.CreateRevocationRegistryAsync(issuerDid, credDef, "test_tag", RegistryType.CL_ACCUM, IssuerType.ISSUANCE_BY_DEFAULT, 99, testTailsPath);

            List<long> issuedList1 = new List<long> { 0,2 };
            List<long> issuedList2 = new List<long> { 0,3 };
            List<long> revokedList1 = new List<long> { 0,2 };
            List<long> revokedList2 = new List<long> { 0,3 };

            (RevocationRegistry tmpRevReg, RevocationRegistryDelta delta1) = await RevocationApi.UpdateRevocationRegistryAsync(
                revRegDefObject,
                revRegObject,
                issuedList1,
                revokedList1,
                revRegDefObject.Value.TailsLocation
                );
            (RevocationRegistry tmpRevReg2, RevocationRegistryDelta delta2) = await RevocationApi.UpdateRevocationRegistryAsync(
                revRegDefObject,
                revRegObject,
                issuedList2,
                revokedList2,
                revRegDefObject.Value.TailsLocation
                );

            //Act
            RevocationRegistryDelta actual = await RevocationApi.MergeRevocationRegistryDeltasAsync(delta1, delta2);

            //Assert
            actual.Value.Revoked.Should().HaveCount(1);
            actual.Value.Revoked.Contains(2).Should().BeTrue();
        }
        #endregion

        #region Tests for CreateOrUpdateRevocationState
        [Test, TestCase(TestName = "CreateOrUpdateRevocationStateAsync() works.")]
        public async Task CreateOrUpdateRevocationStateWorks()
        {
            //Arrange
            List<string> attrNames = new() { "gender", "age", "sex" };
            string issuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
            string schemaName = "gvt";
            string schemaVersion = "1.0";
            string testTailsPath = null;

            Schema schemaObject = await SchemaApi.CreateSchemaAsync(issuerDid, schemaName, schemaVersion, attrNames, 0);

            (CredentialDefinition credDef,
                _,
                _) = await CredentialDefinitionApi.CreateCredentialDefinitionAsync(issuerDid, schemaObject, "tag", SignatureType.CL, 1);


            (RevocationRegistryDefinition revRegDefObject, _, RevocationRegistry tmpRevRegObject, RevocationRegistryDelta revRegDeltaObject) =
                await RevocationApi.CreateRevocationRegistryAsync(issuerDid, credDef, "test_tag", RegistryType.CL_ACCUM, IssuerType.ISSUANCE_BY_DEFAULT, 99, testTailsPath);

            CredentialRevocationState revState = new();

            //Act
            CredentialRevocationState actual = await RevocationApi.CreateOrUpdateRevocationStateAsync(
                revRegDefObject,
                revRegDeltaObject,
                0,
                0,
                revRegDefObject.Value.TailsLocation,
                revState
                );

            //Assert
            actual.Should().NotBeNull();
        }
        #endregion

        #region Tests for GetRevocationRegistryDefinitionAttribute
        [Test, TestCase(TestName = "GetRevocationRegistryDefinitionAttribute() works.")]
        public async Task GetRevocationRegistryDefinitionAttributeWorks()
        {
            //Arrange
            List<string> attrNames = new() { "gender", "age", "sex" };
            string issuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
            string schemaName = "gvt";
            string schemaVersion = "1.0";
            string testTailsPath = null;

            Schema schemaObject = await SchemaApi.CreateSchemaAsync(issuerDid, schemaName, schemaVersion, attrNames, 0);

            (CredentialDefinition credDef,
                _,
                _) = await CredentialDefinitionApi.CreateCredentialDefinitionAsync(issuerDid, schemaObject, "tag", SignatureType.CL, 1);

            (RevocationRegistryDefinition revRegDefObject, 
                _, 
                _, 
                _) = await RevocationApi.CreateRevocationRegistryAsync(issuerDid, credDef, "test_tag", RegistryType.CL_ACCUM, IssuerType.ISSUANCE_BY_DEFAULT, 99, testTailsPath);

            //Act
            string actual = await RevocationApi.GetRevocationRegistryDefinitionAttributeAsync(
                revRegDefObject,
                "id"
                );

            //Assert
            actual.Should().NotBeNull();
        }
        #endregion
    }
}
