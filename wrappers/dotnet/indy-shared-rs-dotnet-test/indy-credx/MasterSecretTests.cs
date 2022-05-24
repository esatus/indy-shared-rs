﻿using FluentAssertions;
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
            Console.WriteLine("tetst");
        }
    }
}