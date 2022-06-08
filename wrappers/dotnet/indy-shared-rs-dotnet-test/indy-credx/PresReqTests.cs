using FluentAssertions;
using indy_shared_rs_dotnet.indy_credx;
using indy_shared_rs_dotnet.Models;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet_test.indy_credx
{
    public class PresReqTests
    {
        [Test]
        [TestCase(TestName = "GenerateNonceAsync returns a string that is not empty.")]
        public async Task GenerateNonce()
        {
            //Arrange

            //Act
            string actual = await PresentationRequestApi.GenerateNonceAsync();

            //Assert
            actual.Should().NotBeEmpty();
        }

        [Test]
        [TestCase(TestName = "Create PresentationRequest from Json.")]
        public async Task CreatePresReq()
        {
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
                            "{\"schema_name\": \"blubb2\"," +
                            "\"schema_version\": \"2.0\"}" +
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
            PresentationRequest actual = await PresentationRequestApi.CreatePresReqFromJsonAsync(presReqJson);

            actual.Name.Should().Be("proof");
            actual.RequestedAttributes.Count.Should().Be(1);
            actual.RequestedPredicates.Count.Should().Be(1);
        }

        [Test, TestCase(TestName = "")]
        public async Task PredicateTest()
        {
            string presReqJson = "{" +
                "\"nonce\": nonce," +
                "\"name\":\"pres_req_1\"," +
                "\"version\":\"0.1\"," +
                "\"requested_attributes\":{" +
                    "\"attr1_referent\":{" +
                        "\"name\":\"name\"" +
                    "}," +
                    "\"attr2_referent\":{" +
                        "\"name\":\"sex\"" +
                    "}," +
                    "\"attr3_referent\":{ " +
                        "\"name\":\"phone\"" +
                    "}," +
                    "\"attr4_referent\":{" +
                        "\"names\": [\"name\", \"height\"]" +
                    "}" +
                "}," +
                "\"requested_predicates\":{" +
                    "\"predicate1_referent\":{ " +
                        "\"name\":\"age\"," +
                        "\"p_type\":\">=\"," +
                        "\"p_value\":18" +
                    "}" +
                "}" +
            "}";

            string singlePredicateJson =
                "\"predicate1_referent\":{ " +
                        "\"name\":\"age\"," +
                        "\"p_type\":\">=\"," +
                        "\"p_value\":18" +
                    "}";


        }

    }
}
