using FluentAssertions;
using indy_shared_rs_dotnet.IndyCredx;
using indy_shared_rs_dotnet.Models;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet_test.IndyCredx
{
    public class PresentationRequestApiTests
    {
        #region Tests for GenerateNonce
        [Test, TestCase(TestName = "GenerateNonceAsync() returns a string that is not empty.")]
        public async Task GenerateNonce()
        {
            //Arrange

            //Act
            string actual = await PresentationRequestApi.GenerateNonceAsync();

            //Assert
            actual.Should().NotBeEmpty();
        }
        #endregion

        #region Tests for CreatePresReq
        [Test, TestCase(TestName = "CreatePresReqFromJsonAsync() creates PresentationRequest from Json.")]
        public async Task CreatePresReq()
        {
            string nonce = await PresentationRequestApi.GenerateNonceAsync();
            long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
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
            PresentationRequest actual = await PresentationRequestApi.CreatePresReqFromJsonAsync(presReqJson);

            actual.Name.Should().Be("proof");
            actual.RequestedAttributes.Count.Should().Be(1);
            actual.RequestedPredicates.Count.Should().Be(1);
        }
        #endregion
    }
}
