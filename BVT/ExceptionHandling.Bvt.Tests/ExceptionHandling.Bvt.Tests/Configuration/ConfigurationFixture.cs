// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.Configuration
{
    [TestClass]
    public class ConfigurationFixture
    {
        private IConfigurationSource configurationSource;

        [TestInitialize]
        public void Initialize()
        {
            this.configurationSource = new FileConfigurationSource(@"configurations\\Configurations.config");
        }

        [TestCleanup]
        public void Cleanup()
        {
            ExceptionPolicy.Reset();

            if (this.configurationSource != null)
            {
                this.configurationSource.Dispose();
            }
        }

        [TestMethod]
        public void ThrowsConfigurationErrorsExceptionWhenWrapHandlerNameIsEmpty()
        {
            try
            {
                ExceptionPolicy.SetExceptionManager(((ExceptionHandlingSettings)this.configurationSource.GetSection("wrapHandlerWithoutName")).BuildExceptionManager());
                Assert.Fail("Test should throw");
            }
            catch (ConfigurationErrorsException ex)
            {
                Assert.IsTrue(ex.Message.Contains("The value for the property 'name' is not valid. The error is: The string must be at least 1 characters long."));
            }
        }

        [TestMethod]
        public void ThrowsArgumentNullExceptionWhenWrapHandlerHasEmptyWrapExceptionType()
        {
            try
            {
                ExceptionPolicy.SetExceptionManager(((ExceptionHandlingSettings)this.configurationSource.GetSection("wrapHandlerWithEmptyWrapExceptionType")).BuildExceptionManager());
                Assert.Fail("Test should throw");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.Message.Contains("Value cannot be null."));
            }
        }

        [TestMethod]
        public void ThrowsConfigurationErrorsExceptionWhenReplaceHandlerNameIsEmpty()
        {
            try
            {
                ExceptionPolicy.SetExceptionManager(((ExceptionHandlingSettings)this.configurationSource.GetSection("replaceHandlerWithoutName")).BuildExceptionManager());
                Assert.Fail("Test should throw");
            }
            catch (ConfigurationErrorsException ex)
            {
                Assert.IsTrue(ex.Message.Contains("The value for the property 'name' is not valid. The error is: The string must be at least 1 characters long."));
            }
        }

        [TestMethod]
        public void ThrowsArgumentNullExceptionWhenReplaceHandlerHasEmptyReplaceExceptionType()
        {
            try
            {
                ExceptionPolicy.SetExceptionManager(((ExceptionHandlingSettings)this.configurationSource.GetSection("replaceHandlerWithEmptyReplaceExceptionType")).BuildExceptionManager());
                Assert.Fail("Test should throw");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.Message.Contains("Value cannot be null."));
            }
        }

        [TestMethod]
        public void DoesNotThrowWhenNoExceptionHandlers()
        {
            try
            {
                ExceptionPolicy.SetExceptionManager(((ExceptionHandlingSettings)this.configurationSource.GetSection("noExceptionHandlers")).BuildExceptionManager());               
            }
            catch (Exception ex)
            {
                Assert.Fail("Test should not throw", ex.Message);
            }
        }
    }
}
