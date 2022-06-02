using FluentAssertions;
using indy_shared_rs_dotnet.indy_credx;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet_test.indy_credx
{
    internal class CredentialApiTests
    {
        #region Tests for CreateCredentialAsync
        [Test, TestCase(TestName = "CreateCredentialAsync creates a credential object.")]
        public async Task CreateCredentialAsync()
        {
            //Arrange

            //Act
            //var result = await CredentialApi.CreateCredentialAsync();

            //Assert
            //result.Should().NotBeNull();
        }
        #endregion

        #region Tests for EncodeCredentialAttributesAsync
        [Test, TestCase(TestName = "EncodeCredentialAttributesAsync with all arguments set returns encoded attribute.")]
        public async Task EncodeCredentialAttributesAsyncWorks()
        {
            //Arrange
            List<string> rawAttributes = new List<string>(){ "test", "test2", "test3" };

            //Act
            string result = await CredentialApi.EncodeCredentialAttributesAsync(rawAttributes);

            //Assert
            result.Should().NotBeNull();
        }
        #endregion

        #region Tests for ProcessCredentialAsync
        #endregion

        #region Tests for GetCredentialAttributeAsync
        #endregion
        
    }
}
