using FluentAssertions;
using indy_shared_rs_dotnet.indy_credx;
using indy_shared_rs_dotnet.Models;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet_test.indy_credx
{
    public class MasterSecretTests
    {
        [Test]
        [TestCase(TestName = "CreateMasterSecret does not throw an exception.")]
        public async Task CreateMasterSecretNoThrow()
        {
            //Arrange

            //Act
            Func<Task> act = async () => {await MasterSecretApi.CreateMasterSecretAsync(); };

            //Assert
            await act.Should().NotThrowAsync();
        }

        [Test]
        [TestCase(TestName = "CreateMasterSecret works.")]
        public async Task CreateMasterSecretWorks()
        {
            //Arrange

            //Act
            MasterSecret testObject = await MasterSecretApi.CreateMasterSecretAsync();

            //Assert
            testObject.Should().BeOfType(typeof(MasterSecret));
            testObject.Value.Ms.Should().NotBeNull();
            Console.WriteLine("MasterSecret: " + testObject.Value.Ms);
        }
    }
}
