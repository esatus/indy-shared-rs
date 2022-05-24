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
            Action act = () => { MasterSecret.CreateMasterSecret(); };
            uint objHandle = await MasterSecret.CreateMasterSecret();
            ObjectHandle test = new(objHandle);
            string testStr = await test.TypeName();

            IndyObject indyObj = new(objHandle);
            string ms_objectAsJson = await indyObj.toJson();
            string ms = indyObj.objectAsJson;

            uint objHandle2 = await MasterSecret.CreateMasterSecret();
            ObjectHandle test2 = new(objHandle2);
            string testStr2 = await test2.TypeName();

            //Assert
            //act.Should().NotThrow();
        }
    }
}
