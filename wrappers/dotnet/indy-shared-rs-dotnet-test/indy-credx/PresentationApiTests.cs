using FluentAssertions;
using indy_shared_rs_dotnet.indy_credx;
using indy_shared_rs_dotnet.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace indy_shared_rs_dotnet_test.indy_credx
{
    public class PresentationApiTests
    {
        #region Tests for CreatePresentationAsync
        [Test, TestCase(TestName = "CreatePresentationAsync() works.")]
        public async Task CreatePresentation()
        {
            //Arrange
            (PresentationRequest presentationRequest,
                Credential credential,
                RevocationRegistry revocation,
                Schema schema,
                CredentialDefinition credentialDefinition) = await TestSetup.PrepareObjectsForPresentationTests();


            List<CredentialEntry> credentialEntries = new()
            {
                new CredentialEntry(credential, revocation)
                //{
                //    CredentialObjectHandle = credential.Handle,
                //    Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds(),
                //    RevStateObjectHandle = revocation.Handle
                //}
            };

            List<CredentialProve> credentialProves = new()
            {
                new CredentialProve
                {
                    EntryIndex = 1,
                    IsPredicate = Convert.ToByte(false),
                    Referent = "testReferent",
                    Reveal = Convert.ToByte(false)
                }
            };

            List<string> selfAttestNames = new()
            {
                "testSelfAttestName1"
            };

            List<string> selfAttestValues = new()
            {
                "testSelfAttestName1"
            };

            MasterSecret masterSecret = await MasterSecretApi.CreateMasterSecretAsync();

            List<Schema> schemas = new()
            {
                schema
            };

            List<CredentialDefinition> credentialDefinitions = new()
            {
                credentialDefinition
            };

            //Act
            Presentation actual = await PresentationApi.CreatePresentationAsync(
                presentationRequest,
                credentialEntries,
                credentialProves,
                selfAttestNames,
                selfAttestValues,
                masterSecret,
                schemas,
                credentialDefinitions
                );

            //Assert

            actual.Should().BeOfType(typeof(Presentation));
        }
        #endregion

        #region Tests for VerifyPresentationAsync
        [Test, TestCase(TestName = "VerifyPresentationAsync() works.")]
        public async Task VerifyPresentationWorks()
        {
            //Arrange
            byte expected = 0;
            (PresentationRequest presentationRequest,
                Credential credential,
                RevocationRegistry revocation,
                Schema schema,
                CredentialDefinition credentialDefinition) = await TestSetup.PrepareObjectsForPresentationTests();

            List<CredentialEntry> credentialEntries = new()
            {
                new CredentialEntry(credential, revocation)
            };

            List<CredentialProve> credentialProves = new()
            {
                new CredentialProve
                {
                    EntryIndex = 1,
                    IsPredicate = Convert.ToByte(false),
                    Referent = "testReferent",
                    Reveal = Convert.ToByte(false)
                }
            };

            List<string> selfAttestNames = new()
            {
                "testSelfAttestName1"
            };

            List<string> selfAttestValues = new()
            {
                "testSelfAttestName1"
            };

            MasterSecret masterSecret = await MasterSecretApi.CreateMasterSecretAsync();

            List<Schema> schemas = new()
            {
                schema
            };

            List<CredentialDefinition> credentialDefinitions = new()
            {
                credentialDefinition
            };

            List<RevocationRegistryDefinition> revocationRegistryDefinitions = new();
            List<RevocationRegistryEntry> revocationRegistryEntries = new();

            Presentation presentationObject = await PresentationApi.CreatePresentationAsync(
                presentationRequest,
                credentialEntries,
                credentialProves,
                selfAttestNames,
                selfAttestValues,
                masterSecret,
                schemas,
                credentialDefinitions
                );

            //Act
            byte actual = await PresentationApi.VerifyPresentationAsync(
                presentationObject,
                presentationRequest,
                schemas,
                credentialDefinitions,
                revocationRegistryDefinitions,
                revocationRegistryEntries
                );

            //Assert
            actual.Should().Be(expected);
        }
        #endregion



    }
}
