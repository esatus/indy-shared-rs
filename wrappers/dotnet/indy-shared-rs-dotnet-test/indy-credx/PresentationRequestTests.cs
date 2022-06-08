using NUnit.Framework;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet_test.indy_credx
{
    internal class PresentationRequestTests
    {
        [Test, TestCase(TestName = "")]
        public async Task PresentationRequestTest()
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
