using FluentAssertions;
using indy_shared_rs_dotnet.models;
using indy_shared_rs_dotnet.indy_credx;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.models.Structures;

namespace indy_shared_rs_dotnet_test.models
{
    public class ObjectHandleTests
    {
        [Test]
        [TestCase(TestName = "ObjectHandle continues ascending indexing when a handle in between is deleted")]
        public async Task RemovingObjectHandles()
        {
            //Arrange
            ObjectHandle objHandle1 = new(await MasterSecret.CreateMasterSecret());
            ObjectHandle objHandle2 = new(await MasterSecret.CreateMasterSecret());
            string objHandleName1 = await objHandle1.TypeName();
            string objHandleName2 = await objHandle2.TypeName();
            
            //Act
            objHandle2.ObjectFree();
            ObjectHandle objHandle3 = new(await MasterSecret.CreateMasterSecret());
            objHandleName1 = await objHandle1.TypeName();
            string objHandleName3 = await objHandle3.TypeName();
            objHandleName2 = await objHandle2.TypeName();

            //Assert
            objHandleName1.Should().Be("MasterSecret");
            objHandleName2.Should().Be("");
            objHandleName3.Should().Be("MasterSecret");
        }
    }
}
