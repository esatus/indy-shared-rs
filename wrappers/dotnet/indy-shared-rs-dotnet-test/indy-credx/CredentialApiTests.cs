using FluentAssertions;
using indy_shared_rs_dotnet.indy_credx;
using indy_shared_rs_dotnet.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.Models.Structures;

namespace indy_shared_rs_dotnet_test.indy_credx
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
            List<string> attrNamesEnc = new() { "Alex", "20", "male" };
            string issuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
            string proverDid = "VsKV7grR1BUE29mG2Fm2kX";
            string schemaName = "gvt";
            string schemaVersion = "1.0";

            MasterSecret masterSecretObject = await MasterSecretApi.CreateMasterSecretAsync();

            Schema schemaObject = await SchemaApi.CreateSchemaAsync(issuerDid, schemaName, schemaVersion, attrNames, 0);
            (CredentialDefinition credDefObject, CredentialDefinitionPrivate credDefPvtObject, CredentialKeyCorrectnessProof keyProofObject) =
                await CredentialDefinitionApi.CreateCredentialDefinitionAsync(issuerDid, schemaObject, "tag", "CL", 1);

            string schemaId = await CredentialDefinitionApi.GetCredentialDefinitionAttribute(credDefObject, "schema_id");
            CredentialOffer credOfferObject = await CredentialOfferApi.CreateCredentialOfferAsync(schemaId, credDefObject, keyProofObject);

            (CredentialRequest credRequestObject, CredentialRequestMetadata metaDataObject) = 
                await CredentialRequestApi.CreateCredentialRequestAsync(proverDid, credDefObject, masterSecretObject, "testMasterSecretName", credOfferObject);

            //Todo fix class CredentialRevocationInfo
            CredentialRevocationInfo credRevInfo = null;
            
            //Act
            (Credential credObject, RevocationRegistry revRegObject, RevocationDelta revDeltaObject) =
                await CredentialApi.CreateCredentialAsync(credDefObject, credDefPvtObject, credOfferObject, credRequestObject,
                attrNames, attrNamesRaw, attrNamesEnc, credRevInfo);

            //Assert
            credObject.Should().BeOfType(typeof(Credential));
            //revRegObject.Should().BeOfType(typeof(RevocationRegistry));
            //revDeltaObject.Should().BeOfType(typeof(RevocationDelta));
        }
        #endregion

        #region Tests for EncodeCredentialAttributesAsync
        [Test, TestCase(TestName = "EncodeCredentialAttributesAsync with all arguments set returns encoded attribute.")]
        public async Task EncodeCredentialAttributesAsyncWorks()
        {
            //Arrange
            List<string> rawAttributes = new List<string>(){ "test", "test2", "test3" };
            //Act
            string result = await CredentialApi.EncodeCredentialAttributesAsync(rawAttributes);

            //Assert
            result.Should().NotBeNull();
            Console.WriteLine(result);
        }
        #endregion

        #region Tests for ProcessCredentialAsync
        [Test, TestCase(TestName = "ProcessCredentialAsync creates a credential object.")]
        public async Task ProcessCredentialAsync()
        {
            //Arrange
            List<string> attrNames = new() { "gender", "age", "sex" };
            string issuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
            string proverDid = "VsKV7grR1BUE29mG2Fm2kX";
            string schemaName = "gvt";
            string schemaVersion = "1.0";

            MasterSecret masterSecretObject = await MasterSecretApi.CreateMasterSecretAsync();

            Schema schemaObject = await SchemaApi.CreateSchemaAsync(issuerDid, schemaName, schemaVersion, attrNames, 0);
            (CredentialDefinition credDefObject, CredentialDefinitionPrivate credDefPvtObject, CredentialKeyCorrectnessProof keyProofObject) =
                await CredentialDefinitionApi.CreateCredentialDefinitionAsync(issuerDid, schemaObject, "tag", "CL", 1);

            string schemaId = await CredentialDefinitionApi.GetCredentialDefinitionAttribute(credDefObject, "schema_id");
            CredentialOffer credOfferObject = await CredentialOfferApi.CreateCredentialOfferAsync(schemaId, credDefObject, keyProofObject);

            (CredentialRequest credRequestObject, CredentialRequestMetadata metaDataObject) =
                await CredentialRequestApi.CreateCredentialRequestAsync(proverDid, credDefObject, masterSecretObject, "testMasterSecretName", credOfferObject);

            //Todo fix class CredentialRevocationInfo
            CredentialRevocationInfo credRevInfo = new();

            (Credential credObject, RevocationRegistry revRegObject, RevocationDelta revDeltaObject) =
                await CredentialApi.CreateCredentialAsync(credDefObject, credDefPvtObject, credOfferObject, credRequestObject,
                attrNames, attrNames, attrNames, credRevInfo);

            //Todo get real revRegDefObject by calling corresponding Create function
            RevocationRegistryDefinition revRegDefObject = new();
            //Act
            Credential credObjectProcessed =
                await CredentialApi.ProcessCredentialAsync(credObject, credRequestObject, masterSecretObject, credDefObject, revRegDefObject);

            //Assert
            credObject.Should().BeOfType(typeof(Credential));
        }
        #endregion

        #region Tests for GetCredentialAttributeAsync
        [Test, TestCase(TestName = "GetCredentialAttributeAsync works for attributes: schema_id, cred_def_id, rev_reg_id, rev_reg_index.")]
        public async Task GetCredentialAttributeAsync()
        {
            //Arrange
            List<string> attrNames =new() { "gender", "age", "sex" };
            string issuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
            string proverDid = "VsKV7grR1BUE29mG2Fm2kX";
            string schemaName = "gvt";
            string schemaVersion = "1.0";

            MasterSecret masterSecretObject = await MasterSecretApi.CreateMasterSecretAsync();

            Schema schemaObject = await SchemaApi.CreateSchemaAsync(issuerDid, schemaName, schemaVersion, attrNames, 0);
            (CredentialDefinition credDefObject, CredentialDefinitionPrivate credDefPvtObject, CredentialKeyCorrectnessProof keyProofObject) =
                await CredentialDefinitionApi.CreateCredentialDefinitionAsync(issuerDid, schemaObject, "tag", "CL", 1);

            string schemaId = await CredentialDefinitionApi.GetCredentialDefinitionAttribute(credDefObject, "schema_id");
            CredentialOffer credOfferObject = await CredentialOfferApi.CreateCredentialOfferAsync(schemaId, credDefObject, keyProofObject);

            (CredentialRequest credRequestObject, CredentialRequestMetadata metaDataObject) =
                await CredentialRequestApi.CreateCredentialRequestAsync(proverDid, credDefObject, masterSecretObject, "testMasterSecretName", credOfferObject);

            //Todo fix class CredentialRevocationInfo
            CredentialRevocationInfo credRevInfo = new();

            (Credential credObject, RevocationRegistry revRegObject, RevocationDelta revDeltaObject) =
                await CredentialApi.CreateCredentialAsync(credDefObject, credDefPvtObject, credOfferObject, credRequestObject,
                attrNames, attrNames, attrNames, credRevInfo);

            //Act
            //note: only attribute "schema_id", "cred_def_id", "rev_reg_id", "rev_reg_index" supported so far.
            string attrSchemaId = await CredentialApi.GetCredentialAttributeAsync(credObject, "schema_id");
            string attrCredDefId = await CredentialApi.GetCredentialAttributeAsync(credObject, "cred_def_id");
            string attrRevRegId = await CredentialApi.GetCredentialAttributeAsync(credObject, "rev_reg_id");
            string attrRevRegIndex = await CredentialApi.GetCredentialAttributeAsync(credObject, "rev_reg_index");
            string attrDefault = await CredentialApi.GetCredentialAttributeAsync(credObject, "default");

            //Assert
            attrSchemaId.Should().Be(credObject.SchemaId);
            attrCredDefId.Should().Be(credObject.CredentialDefinitionId);
            attrRevRegId.Should().Be(credObject.RevocationRegistryId);
            //attrRevRegIndex.Should().Be(credObject.Signature);
            attrDefault.Should().Be("");
        }
        #endregion
    }
}
