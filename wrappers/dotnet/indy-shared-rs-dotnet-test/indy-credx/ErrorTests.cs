using FluentAssertions;
using indy_shared_rs_dotnet.indy_credx;
using NUnit.Framework;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet_test.indy_credx
{
    public class ErrorTests
    {
        [Test]
        [TestCase(TestName = "GetCurrentErrorAsync returns the json of an empty error.")]
        public async Task GetCurrentError()
        {
            //Arrange

            //Act
            string expected = "{\"code\":0,\"message\":null}";
            string actual = await Error.GetCurrentErrorAsync();

            //Assert
            actual.Should().Be(expected);
        }
    }
}
