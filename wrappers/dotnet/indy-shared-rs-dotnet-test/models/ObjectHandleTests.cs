using FluentAssertions;
using indy_shared_rs_dotnet.Models;
using indy_shared_rs_dotnet.indy_credx;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.Models.Structures;

namespace indy_shared_rs_dotnet_test.models
{
    public class ObjectHandleTests
    {
        [Test]
        [TestCase(TestName = "ObjectHandle continues ascending indexing when a handle in between is deleted")]
        public async Task RemovingObjectHandles()
        {
            //Arrange
            MasterSecret ms1 = await MasterSecretApi.CreateMasterSecretAsync();
            MasterSecret ms2 = await MasterSecretApi.CreateMasterSecretAsync();
            IndyObject indyObj1 = new(ms1.Handle);
            IndyObject indyObj2 = new(ms2.Handle);
            string objHandleName1 = await indyObj1.TypeName();
            string objHandleName2 = await indyObj2.TypeName();
            
            //Act
            indyObj2.ObjectFree();
            MasterSecret ms3 = await MasterSecretApi.CreateMasterSecretAsync();
            IndyObject indyObj3 = new(ms3.Handle);
            string objHandleName3 = await indyObj3.TypeName();
            objHandleName2 = await indyObj2.TypeName();

            //Assert
            objHandleName1.Should().Be("MasterSecret");
            objHandleName2.Should().Be("");
            objHandleName3.Should().Be("MasterSecret");
        }
    }
}
