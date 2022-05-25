using FluentAssertions;
using indy_shared_rs_dotnet.indy_credx;
using indy_shared_rs_dotnet.models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.models.Structures;

namespace indy_shared_rs_dotnet_test.indy_credx
{
    public class MasterSecretTests
    {
        [Test]
        [TestCase(TestName = "CreateMasterSecret does not throw an exception.")]
        public async Task CreateMasterSecret()
        {
            //Arrange

            //Act
            //Action act = () => { MasterSecret.CreateMasterSecret(); };
            MasterSecret ms = await MasterSecretApi.CreateMasterSecretAsync();
            IndyObject indyObj = new(ms.Handle);
            string typeName = indyObj.TypeName().GetAwaiter().GetResult();
            string content = await indyObj.toJson();
            //Assert
            //act.Should().NotThrow();
        }
    }
}
