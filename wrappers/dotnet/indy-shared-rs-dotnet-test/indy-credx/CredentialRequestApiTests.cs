using FluentAssertions;
using indy_shared_rs_dotnet.indy_credx;
using indy_shared_rs_dotnet.Models;
using NUnit.Framework;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet_test.indy_credx
{
    internal class CredentialRequestApiTests
    {
        [Test, TestCase(TestName = "CreateCredentialRequestAsync with all Arguments set returns a request and metadata.")]
        public async Task CreateCredentialRequestAsyncWorks()
        {
            MasterSecret masterSecret = new();
            CredentialDefinition definition = new();
            CredentialOffer offer = new();
            (CredentialRequest request, CredentialRequestMetadata metaData) = await CredentialRequestApi.CreateCredentialRequestAsync("", definition, masterSecret, "", offer);

            request.Should().NotBeNull();
            metaData.Should().NotBeNull();
        }
    }
}
