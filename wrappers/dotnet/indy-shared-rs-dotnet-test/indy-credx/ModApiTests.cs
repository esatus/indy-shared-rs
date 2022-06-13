using FluentAssertions;
using indy_shared_rs_dotnet.indy_credx;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet_test.indy_credx
{
    public class ModApiTests
    {
        [Test]
        [TestCase(TestName = "SetDefaultLogger does not throw an exception.")]
        public async Task SetDefaultLogger()
        {
            //Arrange

            //Act
            Action act = () => { ModApi.SetDefaultLogger(); };

            //Assert
            act.Should().NotThrow();
        }

        [Test]
        [TestCase(TestName = "GetVersionAsync returns a string that is not empty.")]
        public async Task GetVersion()
        {
            //Arrange

            //Act
            string actual = await ModApi.GetVersionAsync();

            //Assert
            actual.Should().NotBeEmpty();
        }
    }
}
