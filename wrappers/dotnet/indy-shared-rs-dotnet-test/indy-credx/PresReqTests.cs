using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using indy_shared_rs_dotnet.indy_credx;

namespace indy_shared_rs_dotnet_test.indy_credx
{
    class PresReqTests
    {
        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public async Task Test1()
        {
            var x = await PresReq.GenerateNonceAsync();
            Assert.IsTrue(x is string);
            Console.WriteLine(x);
        }

        [Test]
        public async Task Test2()
        {
            /*string schemaJson = "{id:{}}";
            var x = await PresReq.SchemaGetAttributeAsync();
            Assert.IsTrue(x is string);
            Console.WriteLine(x);*/
        }
    }
}
