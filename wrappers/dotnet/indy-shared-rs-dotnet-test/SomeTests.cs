using FluentAssertions;
using indy_shared_rs_dotnet.indy_credx;
using indy_shared_rs_dotnet.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.Models.Structures;

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
        [TestCase(TestName = "FFiStringList test  in progress.")]
        public async Task FFiStringListInProgess()
        {
            //string[] attrNames = { "gender", "age", "sex" };
            List<string> attrNames = new() { "gender", "age"};
            string did = "NcYxiDXkpYi6ov5FcYDi1e";
            string schemaName = "gvt";
            string schemaVersion = "1.0";
            FfiStrList FfiAttrNames = FfiStrList.Create(attrNames);
            FfiStr FfiDid = FfiStr.Create(did);
            FfiStr FfiSchemaName = FfiStr.Create(schemaName);
            FfiStr FfiSchemaVersion = FfiStr.Create(schemaVersion);

            uint handle = await SchemaApi.CreateSchema(FfiDid, schemaName, FfiSchemaVersion, FfiAttrNames, 0);
            
            IndyObject neu = new(handle);
            string SchemaType = await neu.TypeName();
            string SchemaJson = await neu.toJson();

            string SchemaAttributeId = await SchemaApi.GetSchemaAttribute(neu._handle, FfiStr.Create("id")); ////should return id string (only one supported in rust)
            string SchemaAttributeVer = await SchemaApi.GetSchemaAttribute(neu._handle, FfiStr.Create("version")); //should return "" -> not supported in rust

            Console.WriteLine("CreateSchema test");

        }
        
        [Test]
        [TestCase(TestName = "CreateCredentialDefinition test  in progress.")]
        public async Task CreateCredentialDefinitionInProgess()
        {
            string[] attrNames = { "gender", "age", "sex" };
            string did = "NcYxiDXkpYi6ov5FcYDi1e";
            string schemaName = "gvt";
            string schemaVersion = "1.0";
            FfiStrList FfiAttrNames = FfiStrList.Create(attrNames);
            FfiStr FfiDid = FfiStr.Create(did);
            FfiStr FfiSchemaName = FfiStr.Create(schemaName);
            FfiStr FfiSchemaVersion = FfiStr.Create(schemaVersion);

            uint handle = await SchemaApi.CreateSchema(FfiDid, schemaName, FfiSchemaVersion, FfiAttrNames, 0);
            (CredentialDefinition credDef, CredentialDefinitionPrivate credDefPvt, CredentialKeyCorrectnessProof keyProof) = 
                await CredentialDefinitionApi.CreateCredentialDefinition(did, handle, "tag", "CL", 1);

            Console.WriteLine("CreateSchema test");
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
            char[] attr1 = new char[] { 'n', 'a', 'm', 'e'};
            char[] attr2 = new char[] { 'a', 'g', 'e'};
            char[] attr3 = new char[] { 's', 'e', 'x'};
            char[] attr4 = new char[] { 'h', 'e', 'i', 'g', 'h', 't' };
            //attrNames = { attr1, attr2, attr3, attr4 };
            string[] attrNames = {"name","age", "sex", "height"};
            //uint handle = await SchemaApi.CreateSchema("NcYxiDXkpYi6ov5FcYDi1e", "gvt", "1.0", attrNames, 0);
            /**
            char[] did = new char[] { 'N', 'c', 'Y', 'x', 'i', 'D', 'X', 'k', 'p', 'Y', 'i', '6', 'o', 'v', '5', 'F', 'c', 'Y', 'D', 'i', '1', 'e' };
            char[] schemaName = new char[] { 'g', 'v', 't' };
            char[] schemaVersion = new char[] { '1', '.', '0' };
            fixed (char* str = &did[0]) 
            {
                fixed (char* str2 = &schemaName[0])
                {
                    fixed (char* str3 = &schemaVersion[0])
                    {
                        uint handle = SchemaApi.CreateSchema(str, str2, str3, null, 0).GetAwaiter().GetResult();
                    }
                }
                    
            }
            **/

            //char[] test = new char[] { 'g', 'v', 't'};
            //FfiStr str = new();
            //char v = test[0];
            //str.data = &v;
            //uint handle = SchemaApi.CreateSchema("NcYxiDXkpYi6ov5FcYDi1e", str, "1.0", attrNames, 0).GetAwaiter().GetResult();

            /**
            char[] did = new char[] { 'N', 'c', 'Y', 'x', 'i', 'D', 'X', 'k', 'p', 'Y', 'i', '6', 'o', 'v', '5', 'F', 'c', 'Y', 'D', 'i', '1', 'e' };
            char[] schemaName = new char[] { 'g', 'v', 't' };
            char[] schemaVersion = new char[] { '1', '.', '0' };
            char[] attrNames = new char[] { 'a', 'g', 'e' };
            //uint handle = SchemaApi.CreateSchema(did, schemaName, schemaVersion, attrNames, 0).GetAwaiter().GetResult();
            **/

            //IndyObject indyObj = new(handle);
            //string testStr = await indyObj.TypeName();
            //string ms_content1 = await indyObj.toJson();

            //Assert
            //act.Should().NotThrow();
        }
    }


    
}
