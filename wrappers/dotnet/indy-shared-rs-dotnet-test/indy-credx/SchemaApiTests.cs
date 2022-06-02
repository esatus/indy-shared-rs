﻿using FluentAssertions;
using indy_shared_rs_dotnet.indy_credx;
using indy_shared_rs_dotnet.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static indy_shared_rs_dotnet.Models.Structures;

namespace indy_shared_rs_dotnet_test.indy_credx
{
    class SchemaApiTests
    {/**
        [Test]
        [TestCase(TestName = "CreateSchema does not throw an exception.")]
        public async Task CreateSchemaNoThrow()
        {
            //Arrange
            string[] attrNames = { "gender", "age", "sex" };
            FfiStrList FfiAttrNames = FfiStrList.Create(attrNames);
            string did = "NcYxiDXkpYi6ov5FcYDi1e";
            FfiStr FfiDid = FfiStr.Create(did);
            string schemaName = "gvt";
            FfiStr FfiSchemaName = FfiStr.Create(schemaName);
            string schemaVersion = "1.0";
            FfiStr FfiSchemaVersion = FfiStr.Create(schemaVersion);

            //Act
            Func<Task> act = async () => { await SchemaApi.CreateSchema(FfiDid, FfiSchemaName, FfiSchemaVersion, FfiAttrNames, 0); };

            //Assert
            await act.Should().NotThrowAsync();
        }

        [Test]
        [TestCase(TestName = "CreateSchema works.")]
        public async Task CreateSchemaWorks()
        {
            //Arrange
            string[] attrNames = {"gender", "age", "sex" };
            FfiStrList FfiAttrNames = FfiStrList.Create(attrNames);

            string did = "NcYxiDXkpYi6ov5FcYDi1e";
            FfiStr FfiDid = FfiStr.Create(did);

            string schemaName = "gvt";
            FfiStr FfiSchemaName = FfiStr.Create(schemaName);

            string schemaVersion = "1.0";
            FfiStr FfiSchemaVersion = FfiStr.Create(schemaVersion);

            //Act
            //Schema testObject = await SchemaApi.CreateSchema(FfiDid, FfiSchemaName, FfiSchemaVersion, FfiAttrNames, 0);
            uint handle = await SchemaApi.CreateSchema(FfiDid, FfiSchemaName, FfiSchemaVersion, FfiAttrNames, 0);

            //Assert
            //testObject.Should().BeOfType(typeof(Schema));
        }

        [Test]
        [TestCase(TestName = "GetSchemaAttribute returns id string for key id.")]
        public async Task GetSchemaAttributeWorks()
        {
        }
    **/
        }
}
