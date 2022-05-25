using FluentAssertions;
using indy_shared_rs_dotnet.indy_credx;
using indy_shared_rs_dotnet.models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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

        [Test]
        [TestCase(TestName = "CreateSchema test  in progress.")]
        public async Task CreateSchemaInProgess()
        {
            //Arrange
            //did aus rustCode
            // pub const ISSUER_DID: &'static str = "NcYxiDXkpYi6ov5FcYDi1e";
            //pub const PROVER_DID: &'static str = "VsKV7grR1BUE29mG2Fm2kX";
            //Act
            //Action act = () => { MasterSecret.CreateMasterSecret(); };
            string[] attrNames = { 
                "name", 
                "age", 
                "sex", 
                "height" 
            };
            uint handle = await SchemaApi.CreateSchema("", "gvt", "1.0",attrNames,0);
            //IndyObject indyObj = new(handle);
            //string testStr = await indyObj.TypeName();
            //string ms_content1 = await indyObj.toJson();

            //Assert
            //act.Should().NotThrow();
        }
    }


    
}
