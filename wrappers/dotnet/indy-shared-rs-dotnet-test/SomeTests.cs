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

            //Todo discuss error with team -> mix of [Marshal.Unmanaged...] string and FfiStrList
            Schema schemaObject = await SchemaApi.CreateSchemaAsync(FfiDid, FfiSchemaName, FfiSchemaVersion, FfiAttrNames, 0);
            
            IndyObject neu = new(schemaObject.Handle);
            string SchemaType = await neu.TypeName();
            string SchemaJson = await neu.toJson();

            string SchemaAttributeId = await SchemaApi.GetSchemaAttribute(neu._handle, FfiStr.Create("id")); ////should return id string (only one supported in rust)
            string SchemaAttributeVer = await SchemaApi.GetSchemaAttribute(neu._handle, FfiStr.Create("version")); //should return "" -> not supported in rust

            Console.WriteLine("CreateSchema test");
        }
    }  
}
