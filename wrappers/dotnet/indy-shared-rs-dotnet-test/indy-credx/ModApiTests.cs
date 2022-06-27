using FluentAssertions;
using indy_shared_rs_dotnet.indy_credx;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet_test.indy_credx
{
    public class ModApiTests
    {
        #region Tests for SetDefaultLoggerAsync
        [Test, TestCase(TestName = "SetDefaultLoggerAsync() does not throw an exception.")]
        public async Task SetDefaultLoggerAsyncWorks()
        {
            //Arrange

            //Act
            Action act = () => { ModApi.SetDefaultLoggerAsync(); };

            //Assert
            act.Should().NotThrow();
        }
        #endregion

        #region Tests for GetVersionAsync
        [Test, TestCase(TestName = "GetVersionAsync() returns a string that is not empty.")]
        public async Task GetVersion()
        {
            //Arrange

            //Act
            string actual = await ModApi.GetVersionAsync();

            //Assert
            actual.Should().NotBeEmpty();
        }
        #endregion
    }
}
