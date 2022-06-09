﻿using FluentAssertions;
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
            //Arrange
            List<string> attrNames = new() { "name", "age", "sex" };
            string issuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
            string proverDid = "VsKV7grR1BUE29mG2Fm2kX";
            string schemaName = "gvt";
            string schemaVersion = "1.0";

            //Todo discuss error with team -> mix of [Marshal.Unmanaged...] string and FfiStrList
            Schema schemaObject = await SchemaApi.CreateSchemaAsync(issuerDid, schemaName, schemaVersion, attrNames, 0);

            string SchemaAttributeId = await SchemaApi.GetSchemaAttribute(schemaObject, "id"); ////should return id string (only one supported in rust)
            string SchemaAttributeVer = await SchemaApi.GetSchemaAttribute(schemaObject, "version"); //should return "" -> not supported in rust

            Console.WriteLine("CreateSchema test");
        }
        
        [Test]
        [TestCase(TestName = "SchemaFromJson test in progress.")]
        public async Task SchemaFromJsonInProgress()
        {
            //Arrange
            List<string> attrNames = new() { "gender", "age", "sex" };
            List<string> attrNames2 = new() { "a", "b" };
            string issuerDid = "NcYxiDXkpYi6ov5FcYDi1e";
            string proverDid = "VsKV7grR1BUE29mG2Fm2kX";
            string schemaName = "gvt";
            string schemaVersion = "1.0";

            Schema schemaObjectBase = await SchemaApi.CreateSchemaAsync(issuerDid, schemaName, schemaVersion, attrNames, 0);
            //await Task.Delay(5000);
            Schema schemaObjectBase1 = await SchemaApi.CreateSchemaAsync(proverDid, schemaName, schemaVersion, attrNames2, 0);
            //await Task.Delay(5000);
            //Act
            //string schemaJsonTest = await ObjectApi.ToJson(schemaObjectBase.Handle);
            //Does not work because not all versions are allows must be 1.0 not 2.0
            //string newSchemaJson = "{\"ver\":\"2.0\",\"id\":\"testSchema: 2:gvt: 2.0\",\"name\":\"gvt\",\"version\":\"1.0\",\"attrNames\":[\"gender\",\"age\",\"testAttribut\"],\"seqNo\":5}";
            //await Task.Delay(5000);
            string newSchemaJson = "{\"ver\":\"1.0\",\"id\":\"testSchema:2:gvt:1.0\",\"name\":\"gvt\",\"version\":\"1.0\",\"attrNames\":[\"gender\",\"age\",\"testAttribut\"],\"seqNo\":null}";
            string newSchemaJson2 = "{\"ver\":\"1.0\",\"id\":\"NcYxiDXkpYi6ov5FcYDi1e:2:gvt:1.0\",\"name\":\"gvt\",\"version\":\"1.0\",\"attrNames\":[\"gender\",\"age\",\"sex\"],\"seqNo\":0}";
            
            //Schema schemaObjectTest = await SchemaApi.CreateSchemaFromJsonAsync(schemaJsonTest);
            Schema schemaObjectTest = await SchemaApi.CreateSchemaFromJsonAsync(newSchemaJson2);
           // await Task.Delay(5000);
            Schema newSchemaObjectTest = await SchemaApi.CreateSchemaFromJsonAsync(newSchemaJson2);

            //Assert
            Console.WriteLine("CreateSchema test");
        }
    }
}
