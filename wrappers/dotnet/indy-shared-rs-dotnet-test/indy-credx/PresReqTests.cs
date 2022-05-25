using FluentAssertions;
using indy_shared_rs_dotnet.indy_credx;
using NUnit.Framework;
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
    }
}
