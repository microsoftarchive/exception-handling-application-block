// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.Logging
{
    [TestClass]
    public class LoggingConfigurationFixture
    {
        private IConfigurationSource configurationSource;

        [TestInitialize]
        public void Initialize()
        {
            this.configurationSource = new FileConfigurationSource(@"configurations\\LoggingConfigurations.config");

            LogWriterFactory factory = new LogWriterFactory((e) => { return this.configurationSource.GetSection(e); });
            LogWriter writer = factory.Create();
            Logger.SetLogWriter(writer);
        }

        [TestCleanup]
        public void Cleanup()
        {
            Logger.Reset();
            ExceptionPolicy.Reset();

            if (this.configurationSource != null)
            {
                this.configurationSource.Dispose();
            }
        }

        [TestMethod]
        public void ThrowsConfigurationErrorsExceptionWhenLoggingHandlerNameIsEmpty()
        {
            try
            {
                ExceptionPolicy.SetExceptionManager(((ExceptionHandlingSettings)this.configurationSource.GetSection("loggingHandlerWithoutName")).BuildExceptionManager());
                Assert.Fail("Test should throw");
            }
            catch (ConfigurationErrorsException ex)
            {
                Assert.IsTrue(ex.Message.Contains("The value for the property 'name' is not valid. The error is: The string must be at least 1 characters long."));
            }
        }

        [TestMethod]
        public void ThrowsConfigurationErrorsExceptionWhenLoggingHandlerHasEmptyFormatterType()
        {
            try
            {
                ExceptionPolicy.SetExceptionManager(((ExceptionHandlingSettings)this.configurationSource.GetSection("loggingHandlerWithEmptyFormatterType")).BuildExceptionManager());
                Assert.Fail("Test should throw");
            }
            catch (ConfigurationErrorsException ex)
            {
                Assert.IsTrue(ex.Message.Contains("The formatter type is not set"));
            }
        }

        [TestMethod]
        public void ThrowsExceptionHandlingExceptionWhenLoggingHandlerHasEmptyLogCategory()
        {
            try
            {
                ExceptionPolicy.SetExceptionManager(((ExceptionHandlingSettings)this.configurationSource.GetSection("loggingHandlerWithEmptyLogCategory")).BuildExceptionManager());
                ExceptionPolicy.HandleException(new Exception(), "Policy");
                Assert.Fail("Test should throw");
            }
            catch (ExceptionHandlingException ex)
            {
                Assert.IsTrue(ex.InnerException.Message.Contains("Value cannot be null."));
            }
        }

        [TestMethod]
        public void ThrowsConfigurationErrorsExceptionWhenLoggingHandlerHasEmptyEventId()
        {
            try
            {
                ExceptionPolicy.SetExceptionManager(((ExceptionHandlingSettings)this.configurationSource.GetSection("loggingHandlerWithEmptyEventId")).BuildExceptionManager());
                Assert.Fail("Test should throw");
            }
            catch (ConfigurationErrorsException ex)
            {
                Assert.IsTrue(ex.Message.Contains("The value of the property 'eventId' cannot be parsed."));
            }
        }

        [TestMethod]
        public void ThrowsConfigurationErrorsExceptionWhenLoggingHandlerHasEmptySeverity()
        {
            try
            {
                ExceptionPolicy.SetExceptionManager(((ExceptionHandlingSettings)this.configurationSource.GetSection("loggingHandlerWithEmptySeverity")).BuildExceptionManager());
                Assert.Fail("Test should throw");
            }
            catch (ConfigurationErrorsException ex)
            {
                Assert.IsTrue(ex.Message.Contains("The value of the property 'severity' cannot be parsed. The error is: The enumeration value must be one of the following: Critical, Error, Warning, Information, Verbose, Start, Stop, Suspend, Resume, Transfer."));
            }
        }

        [TestMethod]
        public void ThrowsConfigurationErrorsExceptionWhenLoggingHandlerHasEmptyPriority()
        {
            try
            {
                ExceptionPolicy.SetExceptionManager(((ExceptionHandlingSettings)this.configurationSource.GetSection("loggingHandlerWithEmptyPriority")).BuildExceptionManager());
                Assert.Fail("Test should throw");
            }
            catch (ConfigurationErrorsException ex)
            {
                Assert.IsTrue(ex.Message.Contains("The value of the property 'priority' cannot be parsed. The error is:  is not a valid value for Int32."));
            }
        }
    }
}
