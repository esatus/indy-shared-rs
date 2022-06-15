using indy_shared_rs_dotnet;
using indy_shared_rs_dotnet.indy_credx;
using indy_shared_rs_dotnet.Models;
using System;
using System.Collections.Generic;
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
                await CredentialDefinitionApi.CreateCredentialDefinitionAsync(issuerDid, testSchema, "tag", Consts.SIGNATURE_TYPE, 1);
        }

        public static async Task<CredentialDefinition> PrepareObjectsForCredOfferApiTests()
        {
            string issuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
            Schema testSchema = await CreateTestSchema();

            (CredentialDefinition credDef, CredentialDefinitionPrivate credDefPvt, CredentialKeyCorrectnessProof keyProof) =
                await CredentialDefinitionApi.CreateCredentialDefinitionAsync(issuerDid, testSchema, "tag", Consts.SIGNATURE_TYPE, 1);
        }

        public static async Task<CredentialDefinition> PrepareObjectsForCredReqApiTests()
        {
            string issuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
            Schema testSchema = await CreateTestSchema();

            (CredentialDefinition credDef, CredentialDefinitionPrivate credDefPvt, CredentialKeyCorrectnessProof keyProof) =
                await CredentialDefinitionApi.CreateCredentialDefinitionAsync(issuerDid, testSchema, "tag", Consts.SIGNATURE_TYPE, 1);
        }**/

        public static async Task<(PresentationRequest, Credential, RevocationRegistry, Schema, CredentialDefinition)> PrepareObjectsForPresentationTests()
        {
            // PresentationRequest
            string nonce = await PresentationRequestApi.GenerateNonceAsync();
            long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            string presReqJson =
                "{\"name\": \"proof\"," +
                "\"version\": \"1.0\", " +
                $"\"nonce\": \"{nonce}\"," +
                "\"requested_attributes\": " +
                "{" +
                "\"reft\": " +
                "{" +
                "\"name\":\"attr\"," +
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
                "}" +
                "}" +
                "}," +
                "\"non_revoked\": " +
                "{ " +
                $"\"from\": {timestamp}," +
                $"\"to\": {timestamp}" +
                "}," +
                "\"ver\": \"1.0\"" +
                "}";
            PresentationRequest presentationRequest = await PresentationRequestApi.CreatePresReqFromJsonAsync(presReqJson);

            // CredentialDefinition
            List<string> attrNames = new() { "name", "age", "sex" };
            string issuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
            string schemaName = "gvt";
            string schemaVersion = "1.0";

            Schema schemaObject = await SchemaApi.CreateSchemaAsync(issuerDid, schemaName, schemaVersion, attrNames, 0);

            (CredentialDefinition credDef, CredentialDefinitionPrivate credDefPvt, CredentialKeyCorrectnessProof keyProof) =
                await CredentialDefinitionApi.CreateCredentialDefinitionAsync(issuerDid, schemaObject, "tag", Consts.SIGNATURE_TYPE, 1);

            // Schema
            List<string> schemattrNames = new() { "name", "age", "sex" };
            string schemaIssuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
            string schemaSchemaName = "gvt";
            string schemaSchemaVersion = "1.0";

            Schema schema = schemaObject;//await SchemaApi.CreateSchemaAsync(schemaIssuerDid, schemaSchemaName, schemaSchemaVersion, schemattrNames, 0);

            // CredentialObject, RevObject
            List<string> credentialObjectAttrNames = new() { "name", "age", "sex" };
            List<string> attrNamesRaw = new() { "Alex", "20", "male" };
            List<string> attrNamesEnc = await CredentialApi.EncodeCredentialAttributesAsync(attrNamesRaw);
            string credentialObjectIssuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
            string credentialObjectProverDid = "VsKV7grR1BUE29mG2Fm2kX";
            string credentialObjectSchemaName = "gvt";
            string credentialObjectSchemaVersion = "1.0";
            string testTailsPath = null;

            MasterSecret masterSecretObject = await MasterSecretApi.CreateMasterSecretAsync();

            Schema credentialObjectSchemaNameSchemaObject = await SchemaApi.CreateSchemaAsync(credentialObjectIssuerDid, credentialObjectSchemaName, credentialObjectSchemaVersion, credentialObjectAttrNames, 0);
            (CredentialDefinition credDefObject, CredentialDefinitionPrivate credDefPvtObject, CredentialKeyCorrectnessProof keyProofObject) =
                await CredentialDefinitionApi.CreateCredentialDefinitionAsync(issuerDid, schemaObject, "tag", Consts.SIGNATURE_TYPE, 1);

            string schemaId = await CredentialDefinitionApi.GetCredentialDefinitionAttributeAsync(credDefObject, "schema_id");
            CredentialOffer credOfferObject = await CredentialOfferApi.CreateCredentialOfferAsync(schemaId, credDefObject, keyProofObject);

            (CredentialRequest credRequestObject, CredentialRequestMetadata metaDataObject) =
                await CredentialRequestApi.CreateCredentialRequestAsync(credentialObjectProverDid, credDefObject, masterSecretObject, "testMasterSecretName", credOfferObject);

            (RevocationRegistryDefinition revRegDefObject, RevocationRegistryDefinitionPrivate revRegDefPvtObject, RevocationRegistry revRegObject, RevocationRegistryDelta revRegDeltaObject) =
                await RevocationApi.CreateRevocationRegistryAsync(issuerDid, credDef, "test_tag", RegistryType.CL_ACCUM, IssuerType.ISSUANCE_BY_DEFAULT, 99, testTailsPath);

            CredentialRevocationConfig credRevInfo = new CredentialRevocationConfig
            {
                revRegDefObjectHandle = revRegDefObject.Handle,
                revRegDefPvtObjectHandle = revRegDefPvtObject.Handle,
                revRegObjectHandle = revRegObject.Handle,
                tailsPath = revRegDefObject.Value.TailsLocation,
                regIdx = 1,
                regUsed = new List<long> { 1 }
            };

            (Credential credObject, RevocationRegistry revRegObjectNew, RevocationRegistryDelta revDeltaObject) =
                await CredentialApi.CreateCredentialAsync(credDefObject, credDefPvtObject, credOfferObject, credRequestObject,
                attrNames, attrNamesRaw, attrNamesEnc, credRevInfo);

            return (presentationRequest, credObject, revRegObject, schema, credDef);
        }
    }
}
