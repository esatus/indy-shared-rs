using FluentAssertions;
using indy_shared_rs_dotnet.IndyCredx;
using indy_shared_rs_dotnet.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet_test.IndyCredx
{
    public class CredentialApiTests
    {
        #region Tests for CreateCredentialAsync
        [Test, TestCase(TestName = "CreateCredentialAsync creates a credential object.")]
        public async Task CreateCredentialAsync()
        {
            //Arrange
            List<string> attrNames = new() { "name", "age", "sex" };
            List<string> attrNamesRaw = new() { "Alex", "20", "male" };
            List<string> attrNamesEnc = await CredentialApi.EncodeCredentialAttributesAsync(attrNamesRaw);
            string issuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
            string proverDid = "VsKV7grR1BUE29mG2Fm2kX";
            string schemaName = "gvt";
            string schemaVersion = "1.0";
            string testTailsPathForRevocation = null;

            MasterSecret masterSecretObject = await MasterSecretApi.CreateMasterSecretAsync();

            Schema schemaObject = await SchemaApi.CreateSchemaAsync(issuerDid, schemaName, schemaVersion, attrNames, 0);
            (CredentialDefinition credDefObject, CredentialDefinitionPrivate credDefPvtObject, CredentialKeyCorrectnessProof keyProofObject) =
                await CredentialDefinitionApi.CreateCredentialDefinitionAsync(issuerDid, schemaObject, "tag", SignatureType.CL, 1);

            string schemaId = await CredentialDefinitionApi.GetCredentialDefinitionAttributeAsync(credDefObject, "schema_id");
            CredentialOffer credOfferObject = await CredentialOfferApi.CreateCredentialOfferAsync(schemaId, credDefObject, keyProofObject);

            (CredentialRequest credRequestObject, CredentialRequestMetadata metaDataObject) =
                await CredentialRequestApi.CreateCredentialRequestAsync(proverDid, credDefObject, masterSecretObject, "testMasterSecretName", credOfferObject);

            (RevocationRegistryDefinition revRegDefObject, RevocationRegistryDefinitionPrivate revRegDefPvtObject, RevocationRegistry revRegObject, RevocationRegistryDelta revRegDeltaObject) =
                await RevocationApi.CreateRevocationRegistryAsync(issuerDid, credDefObject, "test_tag", RegistryType.CL_ACCUM, IssuerType.ISSUANCE_BY_DEFAULT, 99, testTailsPathForRevocation);

            CredentialRevocationConfig credRevInfo = new CredentialRevocationConfig
            {
                RevRegDefObjectHandle = revRegDefObject.Handle,
                RevRegDefPvtObjectHandle = revRegDefPvtObject.Handle,
                RevRegObjectHandle = revRegObject.Handle,
                TailsPath = revRegDefObject.Value.TailsLocation,
                RegIdx = 1,
                RegUsed = new List<long> { 1 }
            };

            //Act
            (Credential credObject, RevocationRegistry revRegObjectNew, RevocationRegistryDelta revDeltaObject) =
                await CredentialApi.CreateCredentialAsync(credDefObject, credDefPvtObject, credOfferObject, credRequestObject,
                attrNames, attrNamesRaw, attrNamesEnc, credRevInfo);

            //Assert
            credObject.Should().BeOfType(typeof(Credential));
            revRegObjectNew.Should().BeOfType(typeof(RevocationRegistry));
            //revDeltaObject.Should().BeOfType(typeof(RevocationRegistryDelta));
        }
        #endregion

        #region Tests for EncodeCredentialAttributesAsync
        [Test, TestCase(TestName = "EncodeCredentialAttributesAsync() with all arguments set returns encoded attribute.")]
        public async Task EncodeCredentialAttributesAsyncWorks()
        {
            //Arrange
            List<string> rawAttributes = new List<string>() { "test", "test2", "test3" };

            //Act
            List<string> result = await CredentialApi.EncodeCredentialAttributesAsync(rawAttributes);

            //Assert
            result.Should().NotBeNull();
            Console.WriteLine(result);
        }

        [Test, TestCase(TestName = "EncodeCredentialAttributesAsync() throws SharedRsException when given an empty list.")]
        public async Task EncodeCredentialAttributesAsyncThrowsException()
        {
            //Arrange
            List<string> rawAttributes = new List<string>();

            //Act
            Func<Task> act = async () => await CredentialApi.EncodeCredentialAttributesAsync(rawAttributes);

            //Assert
            await act.Should().ThrowAsync<SharedRsException>();
        }
        #endregion

        #region Tests for ProcessCredentialAsync
        [Test, TestCase(TestName = "ProcessCredentialAsync() creates a credential object.")]
        public async Task ProcessCredentialAsync()
        {
            //Arrange
            List<string> attrNames = new() { "name", "age", "sex" };
            List<string> attrNamesRaw = new() { "Alex", "20", "male" };
            List<string> attrNamesEnc = await CredentialApi.EncodeCredentialAttributesAsync(attrNamesRaw);
            string issuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
            string proverDid = "VsKV7grR1BUE29mG2Fm2kX";
            string schemaName = "gvt";
            string schemaVersion = "1.0";
            string testTailsPathForRevocation = null;

            MasterSecret masterSecretObject = await MasterSecretApi.CreateMasterSecretAsync();

            Schema schemaObject = await SchemaApi.CreateSchemaAsync(issuerDid, schemaName, schemaVersion, attrNames, 0);
            (CredentialDefinition credDefObject, CredentialDefinitionPrivate credDefPvtObject, CredentialKeyCorrectnessProof keyProofObject) =
                await CredentialDefinitionApi.CreateCredentialDefinitionAsync(issuerDid, schemaObject, "tag", SignatureType.CL, 1);

            string schemaId = await CredentialDefinitionApi.GetCredentialDefinitionAttributeAsync(credDefObject, "schema_id");
            CredentialOffer credOfferObject = await CredentialOfferApi.CreateCredentialOfferAsync(schemaId, credDefObject, keyProofObject);

            (CredentialRequest credRequestObject, CredentialRequestMetadata metaDataObject) =
                await CredentialRequestApi.CreateCredentialRequestAsync(proverDid, credDefObject, masterSecretObject, "testMasterSecretName", credOfferObject);

            (RevocationRegistryDefinition revRegDefObject, RevocationRegistryDefinitionPrivate revRegDefPvtObject, RevocationRegistry revRegObject, RevocationRegistryDelta revRegDeltaObject) =
                await RevocationApi.CreateRevocationRegistryAsync(issuerDid, credDefObject, "test_tag", RegistryType.CL_ACCUM, IssuerType.ISSUANCE_BY_DEFAULT, 99, testTailsPathForRevocation);

            CredentialRevocationConfig credRevInfo = new CredentialRevocationConfig
            {
                RevRegDefObjectHandle = revRegDefObject.Handle,
                RevRegDefPvtObjectHandle = revRegDefPvtObject.Handle,
                RevRegObjectHandle = revRegObject.Handle,
                TailsPath = revRegDefObject.Value.TailsLocation,
                RegIdx = 1,
                RegUsed = new List<long> { 1 }
            };

            (Credential credObject, RevocationRegistry revRegObjectNew, RevocationRegistryDelta revDeltaObject) =
                await CredentialApi.CreateCredentialAsync(credDefObject, credDefPvtObject, credOfferObject, credRequestObject,
                attrNames, attrNamesRaw, attrNamesEnc, credRevInfo);

            //Act
            Credential credObjectProcessed =
                await CredentialApi.ProcessCredentialAsync(credObject, metaDataObject, masterSecretObject, credDefObject, revRegDefObject);

            //Assert
            credObjectProcessed.Should().BeOfType(typeof(Credential));
        }
        #endregion

        #region Tests for GetCredentialAttributeAsync
        [Test, TestCase(TestName = "GetCredentialAttributeAsync() works for attribute names: schema_id, cred_def_id, rev_reg_id.")]
        public async Task GetCredentialAttributeAsync()
        {
            //Arrange
            List<string> attrNames = new() { "name", "age", "sex" };
            List<string> attrNamesRaw = new() { "Alex", "20", "male" };
            List<string> attrNamesEnc = await CredentialApi.EncodeCredentialAttributesAsync(attrNamesRaw);
            string issuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
            string proverDid = "VsKV7grR1BUE29mG2Fm2kX";
            string schemaName = "gvt";
            string schemaVersion = "1.0";
            string testTailsPathForRevocation = null;
            MasterSecret masterSecretObject = await MasterSecretApi.CreateMasterSecretAsync();

            Schema schemaObject = await SchemaApi.CreateSchemaAsync(issuerDid, schemaName, schemaVersion, attrNames, 0);
            (CredentialDefinition credDefObject, CredentialDefinitionPrivate credDefPvtObject, CredentialKeyCorrectnessProof keyProofObject) =
                await CredentialDefinitionApi.CreateCredentialDefinitionAsync(issuerDid, schemaObject, "tag", SignatureType.CL, 1);

            string schemaId = await CredentialDefinitionApi.GetCredentialDefinitionAttributeAsync(credDefObject, "schema_id");
            CredentialOffer credOfferObject = await CredentialOfferApi.CreateCredentialOfferAsync(schemaId, credDefObject, keyProofObject);

            (CredentialRequest credRequestObject, CredentialRequestMetadata metaDataObject) =
                await CredentialRequestApi.CreateCredentialRequestAsync(proverDid, credDefObject, masterSecretObject, "testMasterSecretName", credOfferObject);

            (RevocationRegistryDefinition revRegDefObject, RevocationRegistryDefinitionPrivate revRegDefPvtObject, RevocationRegistry revRegObject, RevocationRegistryDelta revRegDeltaObject) =
                await RevocationApi.CreateRevocationRegistryAsync(issuerDid, credDefObject, "test_tag", RegistryType.CL_ACCUM, IssuerType.ISSUANCE_BY_DEFAULT, 99, testTailsPathForRevocation);

            CredentialRevocationConfig credRevInfo = new CredentialRevocationConfig
            {
                RevRegDefObjectHandle = revRegDefObject.Handle,
                RevRegDefPvtObjectHandle = revRegDefPvtObject.Handle,
                RevRegObjectHandle = revRegObject.Handle,
                TailsPath = revRegDefObject.Value.TailsLocation,
                RegIdx = 1,
                RegUsed = new List<long> { 1 }
            };

            (Credential credObject, RevocationRegistry revRegObjectNew, RevocationRegistryDelta revDeltaObject) =
                await CredentialApi.CreateCredentialAsync(credDefObject, credDefPvtObject, credOfferObject, credRequestObject,
                attrNames, attrNamesRaw, attrNamesEnc, credRevInfo);

            //Act
            //note: only attribute "schema_id", "cred_def_id", "rev_reg_id", "rev_reg_index" supported so far.
            string attrSchemaId = await CredentialApi.GetCredentialAttributeAsync(credObject, "schema_id");
            string attrCredDefId = await CredentialApi.GetCredentialAttributeAsync(credObject, "cred_def_id");
            string attrRevRegId = await CredentialApi.GetCredentialAttributeAsync(credObject, "rev_reg_id");
            string attrRevRegIndex = await CredentialApi.GetCredentialAttributeAsync(credObject, "rev_reg_index");
            //string attrDefault = await CredentialApi.GetCredentialAttributeAsync(credObject, "default");

            //Assert
            attrSchemaId.Should().Be(credObject.SchemaId);
            attrCredDefId.Should().Be(credObject.CredentialDefinitionId);
            attrRevRegId.Should().Be(credObject.RevocationRegistryId);
            attrRevRegIndex.Should().Be(credObject.Signature.RCredential.I.ToString());
            //attrDefault.Should().Be("");
        }

        [Test, TestCase(TestName = "GetCredentialAttributeAsync() throws SharedRsException when given empty attribute name.")]
        public async Task GetCredentialAttributeAsyncThrowsException()
        {
            //Arrange
            string attributeName = "";
            List<string> attrNames = new() { "name", "age", "sex" };
            List<string> attrNamesRaw = new() { "Alex", "20", "male" };
            List<string> attrNamesEnc = await CredentialApi.EncodeCredentialAttributesAsync(attrNamesRaw);
            string issuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
            string proverDid = "VsKV7grR1BUE29mG2Fm2kX";
            string schemaName = "gvt";
            string schemaVersion = "1.0";
            string testTailsPathForRevocation = null;
            MasterSecret masterSecretObject = await MasterSecretApi.CreateMasterSecretAsync();

            Schema schemaObject = await SchemaApi.CreateSchemaAsync(issuerDid, schemaName, schemaVersion, attrNames, 0);
            (CredentialDefinition credDefObject, CredentialDefinitionPrivate credDefPvtObject, CredentialKeyCorrectnessProof keyProofObject) =
                await CredentialDefinitionApi.CreateCredentialDefinitionAsync(issuerDid, schemaObject, "tag", SignatureType.CL, 1);

            string schemaId = await CredentialDefinitionApi.GetCredentialDefinitionAttributeAsync(credDefObject, "schema_id");
            CredentialOffer credOfferObject = await CredentialOfferApi.CreateCredentialOfferAsync(schemaId, credDefObject, keyProofObject);

            (CredentialRequest credRequestObject, CredentialRequestMetadata metaDataObject) =
                await CredentialRequestApi.CreateCredentialRequestAsync(proverDid, credDefObject, masterSecretObject, "testMasterSecretName", credOfferObject);

            (RevocationRegistryDefinition revRegDefObject, RevocationRegistryDefinitionPrivate revRegDefPvtObject, RevocationRegistry revRegObject, RevocationRegistryDelta revRegDeltaObject) =
                await RevocationApi.CreateRevocationRegistryAsync(issuerDid, credDefObject, "test_tag", RegistryType.CL_ACCUM, IssuerType.ISSUANCE_BY_DEFAULT, 99, testTailsPathForRevocation);

            CredentialRevocationConfig credRevInfo = new CredentialRevocationConfig
            {
                RevRegDefObjectHandle = revRegDefObject.Handle,
                RevRegDefPvtObjectHandle = revRegDefPvtObject.Handle,
                RevRegObjectHandle = revRegObject.Handle,
                TailsPath = revRegDefObject.Value.TailsLocation,
                RegIdx = 1,
                RegUsed = new List<long> { 1 }
            };
            (Credential credObject, RevocationRegistry revRegObjectNew, RevocationRegistryDelta revDeltaObject) =
                await CredentialApi.CreateCredentialAsync(credDefObject, credDefPvtObject, credOfferObject, credRequestObject,
                attrNames, attrNamesRaw, attrNamesEnc, credRevInfo);

            //Act
            Func<Task> act = async () => await CredentialApi.GetCredentialAttributeAsync(credObject, attributeName);

            //Assert
            await act.Should().ThrowAsync<SharedRsException>();
        }
        #endregion
    }
}