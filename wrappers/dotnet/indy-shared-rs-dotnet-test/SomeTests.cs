using FluentAssertions;
using indy_shared_rs_dotnet.indy_credx;
using indy_shared_rs_dotnet.models;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet_test
{
    class SomeTests
    {
        [Test]
        [TestCase(TestName = "SomeTests in progress.")]
        public async Task SomeTestsInProgess()
        {
            //Arrange

            //Act
            //Action act = () => { MasterSecret.CreateMasterSecret(); };
            MasterSecret ms = await MasterSecretApi.CreateMasterSecretAsync();
            IndyObject indyObj = new(ms.Handle);
            string testStr = await indyObj.TypeName();
            string ms_content1 = await indyObj.toJson();
            string ms_content2 = indyObj.objectAsJson;

            //Assert
            //act.Should().NotThrow();
        }
    }
}
