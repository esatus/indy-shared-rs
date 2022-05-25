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
            uint objHandle = await MasterSecret.CreateMasterSecret();
            IndyObject indyObj = new(objHandle);
            string testStr = await indyObj.TypeName();
            string ms_content1 = await indyObj.toJson();
            string ms_content2 = indyObj.objectAsJson;

            //Assert
            //act.Should().NotThrow();
        }

        [Test]
        [TestCase(TestName = "Create CredDef.")]
        public async Task CreateACredDef()
        {
            //Arrange
            Byte bytey = (Byte)5;
            string[] attr = { "a", "b", "c" };
            //Act
            //Action act = () => { MasterSecret.CreateMasterSecret(); };
            Schema testSchema = new("did", "name", "version", attr, 10);

            CredDef test = new("did", 1, "tag", "signature", bytey);

            //Assert
            //act.Should().NotThrow();
        }
    }
}
