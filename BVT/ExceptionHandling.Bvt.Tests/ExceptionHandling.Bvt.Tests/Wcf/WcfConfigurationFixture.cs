// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.Wcf
{
    [TestClass]
    public class WcfConfigurationFixture
    {
        private IConfigurationSource configurationSource;

        [TestInitialize]
        public void Initialize()
        {
            this.configurationSource = new FileConfigurationSource(@"configurations\\WcfConfigurations.config");
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
        public void ThrowsConfigurationErrorsExceptionWhenFaultContractHandlerNameIsEmpty()
        {
            try
            {
                ExceptionPolicy.SetExceptionManager(((ExceptionHandlingSettings)this.configurationSource.GetSection("faultContractHandlerWithoutName")).BuildExceptionManager());
                Assert.Fail("Test should throw");
            }
            catch (ConfigurationErrorsException ex)
            {
                Assert.IsTrue(ex.Message.Contains("The value for the property 'name' is not valid. The error is: The string must be at least 1 characters long."));
            }
        }

        [TestMethod]
        public void ThrowsArgumentNullExceptionWhenFaultContractHandlerHasEmptyFaultContractType()
        {
            try
            {
                ExceptionPolicy.SetExceptionManager(((ExceptionHandlingSettings)this.configurationSource.GetSection("faultContractHandlerWithEmptyFaultContractType")).BuildExceptionManager());
                Assert.Fail("Test should throw");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsTrue(ex.Message.Contains("Value cannot be null."));
            }
        }
    }
}
