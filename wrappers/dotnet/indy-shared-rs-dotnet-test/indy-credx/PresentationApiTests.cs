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
    public class PresentationApiTests
    {
        /**
        [Test]
        [TestCase(TestName = "CreatePresentationAsync works.")]
        public async Task CreatePresentation()
        {
            //Arrange
            (PresentationRequest presentationRequest,
                Credential credential,
                RevocationRegistry revocation,
                Schema schema,
                CredentialDefinition credentialDefinition) = await TestSetup.PrepareObjectsForPresentationTests();


            List<CredentialEntry> credentialEntries = new()
            {
                new CredentialEntry(credential, revocation)
                //{
                //    CredentialObjectHandle = credential.Handle,
                //    Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds(),
                //    RevStateObjectHandle = revocation.Handle
                //}
            };

            List<CredentialProve> credentialProves = new()
            {
                new CredentialProve
                {
                    EntryIndex = 1,
                    IsPredicate = Convert.ToByte(false),
                    Referent = "testReferent",
                    Reveal = Convert.ToByte(false)
                }
            };

            List<string> selfAttestNames = new()
            {
                "testSelfAttestName1"
            };

            List<string> selfAttestValues = new()
            {
                "testSelfAttestName1"
            };

            MasterSecret masterSecret = await MasterSecretApi.CreateMasterSecretAsync();

            List<Schema> schemas = new()
            {
               schema
            };

            List<CredentialDefinition> credentialDefinitions = new()
            {
                credentialDefinition
            };

            //Act
            Presentation actual = await PresentationApi.CreatePresentationAsync(
                presentationRequest,
                credentialEntries,
                credentialProves,
                selfAttestNames,
                selfAttestValues,
                masterSecret,
                schemas,
                credentialDefinitions
                );

            //Assert

            actual.Should().BeOfType(typeof(Presentation));
        }**/

        [Test]
        [TestCase(TestName = "CreatePresentationAsync works neu.")]
        public async Task CreatePresentationWorks()
        {
            //Arrange
            /**
            (PresentationRequest presentationRequest,
                Credential credential,
                RevocationRegistry revocation,
                Schema schema,
                CredentialDefinition credentialDefinition) = await TestSetup.PrepareObjectsForPresentationTests();**/

            string nonce = await PresentationRequestApi.GenerateNonceAsync();
            var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            string presReqJson = "{" +
                "\"name\": \"proof\"," +
                "\"version\": \"1.0\", " +
                $"\"nonce\": \"{nonce}\"," +
                "\"requested_attributes\": " +
                "{" +
                    "\"reft\": " +
                    "{" +
                        "\"name\":\"attr\"," +
                        "\"value\":\"myValue\"," +
                        "\"names\": [], " +
                        "\"non_revoked\":" +
                        "{ " +
                            $"\"from\": {timestamp}, " +
                            $"\"to\": {timestamp}" +
                        "}" +
                    "}" +
                "}," +
                "\"requested_predicates\": " +
                "{" +
                    "\"light\": " +
                    "{" +
                        "\"name\":\"pred\"," +
                        "\"p_type\":\">=\"," +
                        "\"p_value\":18," +
                        "\"non_revoked\":" +
                        "{ " +
                            $"\"from\": {timestamp}, " +
                            $"\"to\": {timestamp}" +
                        "}," +
                        "\"restrictions\":" +
                        "[" +
                            "{\"schema_name\": \"blubb\"," +
                            "\"schema_version\": \"1.0\"}," +
                            "{\"cred_def_id\": \"blubb2\"," +
                            "\"schema_version\": \"2.0\"}," +
                            "{\"not_an_attribute\": \"should Fail\"}" +
                        "]" +
                    "}" +
                "}," +
                "\"non_revoked\": " +
                "{ " +
                    $"\"from\": {timestamp}," +
                    $"\"to\": {timestamp}" +
                "}," +
                "\"ver\": \"1.0\"" +
                "}";
            PresentationRequest presReqObject = await PresentationRequestApi.CreatePresReqFromJsonAsync(presReqJson);

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
                await CredentialDefinitionApi.CreateCredentialDefinitionAsync(issuerDid, schemaObject, "tag", Consts.SIGNATURE_TYPE, 1);

            string schemaId = await CredentialDefinitionApi.GetCredentialDefinitionAttributeAsync(credDefObject, "schema_id");
            CredentialOffer credOfferObject = await CredentialOfferApi.CreateCredentialOfferAsync(schemaId, credDefObject, keyProofObject);

            (CredentialRequest credRequestObject, CredentialRequestMetadata metaDataObject) =
                await CredentialRequestApi.CreateCredentialRequestAsync(proverDid, credDefObject, masterSecretObject, "testMasterSecretName", credOfferObject);

            (RevocationRegistryDefinition revRegDefObject, RevocationRegistryDefinitionPrivate revRegDefPvtObject, RevocationRegistry revRegObject, RevocationRegistryDelta revRegDeltaObject) =
                await RevocationApi.CreateRevocationRegistryAsync(issuerDid, credDefObject, "test_tag", RegistryType.CL_ACCUM, IssuerType.ISSUANCE_BY_DEFAULT, 99, testTailsPathForRevocation);

            CredentialRevocationConfig credRevInfo = new CredentialRevocationConfig
            {
                revRegDefObjectHandle = revRegDefObject.Handle,
                revRegDefPvtObjectHandle = revRegDefPvtObject.Handle,
                revRegObjectHandle = revRegObject.Handle,
                tailsPath = revRegDefObject.Value.TailsLocation,
                regIdx = 1,
                regUsed = new List<long> { 1 }
            };

            //Act
            (Credential credObject, RevocationRegistry revRegObjectNew, RevocationRegistryDelta revDeltaObject) =
                await CredentialApi.CreateCredentialAsync(credDefObject, credDefPvtObject, credOfferObject, credRequestObject,
                attrNames, attrNamesRaw, attrNamesEnc, credRevInfo);

            CredentialRevocationState emptyRevocationState = new() { Handle = 0 };
            CredentialRevocationState credRevRegState = await RevocationApi.CreateOrUpdateRevocationState(
                revRegDefObject, 
                revRegDeltaObject, 
                credObject.Signature.RCredential.I, 
                timestamp, 
                revRegDefObject.Value.TailsLocation, 
                emptyRevocationState);

            List<CredentialEntry> credentialEntries = new()
            {
                new CredentialEntry(credObject, timestamp, credRevRegState)
                //with empty timestamp and revState
                //new CredentialEntry(credObject,0, null)
            };

            List<CredentialProve> credentialProves = new()
            {
                new CredentialProve
                {
                    EntryIndex = 1,
                    IsPredicate = Convert.ToByte(false),
                    Referent = "testReferent",
                    Reveal = Convert.ToByte(true)
                }
            };

            List<string> selfAttestNames = new()
            {
                "testSelfAttestName1"
            };

            List<string> selfAttestValues = new()
            {
                "testSelfAttestName1"
            };

            MasterSecret masterSecret = await MasterSecretApi.CreateMasterSecretAsync();

            List<Schema> schemas = new()
            {
                schemaObject
            };

            List<CredentialDefinition> credentialDefinitions = new()
            {
                credDefObject
            };
            
            //Act
            Presentation actual = await PresentationApi.CreatePresentationAsync(
                presReqObject,
                credentialEntries,
                credentialProves,
                selfAttestNames,
                selfAttestValues,
                masterSecret,
                schemas,
                credentialDefinitions
                );

            //Assert

            actual.Should().BeOfType(typeof(Presentation));
        }

    }
}
