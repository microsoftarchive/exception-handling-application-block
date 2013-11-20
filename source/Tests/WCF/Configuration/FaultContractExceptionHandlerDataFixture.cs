// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Tests.Configuration
{
    [TestClass]
    public class GivenContractExceptionHandlerData : ArrangeActAssert
    {
        private FaultContractExceptionHandlerData configuration;

        protected override void Arrange()
        {
            this.configuration = new FaultContractExceptionHandlerData("wcf", typeof(MockFaultContract).AssemblyQualifiedName);
        }

        [TestMethod]
        public void WhenCreatingHandler_ThenHasNoSettings()
        {
            var handler = (FaultContractExceptionHandler)this.configuration.BuildExceptionHandler();

            var exception = new Exception();
            var handledException = handler.HandleException(exception, Guid.NewGuid());
            Assert.IsInstanceOfType(handledException, typeof(FaultContractWrapperException));
            Assert.AreEqual(exception.Message, handledException.Message);
        }
    }

    [TestClass]
    public class GivenFaultContractExceptionHandlerResourceName : ArrangeActAssert
    {
        private FaultContractExceptionHandlerData configuration;

        protected override void Arrange()
        {
            configuration = new FaultContractExceptionHandlerData("wcf", typeof(MockFaultContract).AssemblyQualifiedName)
            {
                ExceptionMessageResourceName = "WcfMessageResource",
                ExceptionMessageResourceType = typeof(Resources).AssemblyQualifiedName
            };
        }

        [TestMethod]
        public void WhenCreatingHandler_ThenUsesResolverMessage()
        {
            var handler = (FaultContractExceptionHandler)this.configuration.BuildExceptionHandler();

            var exception = new Exception();
            var handledException = handler.HandleException(exception, Guid.NewGuid());
            Assert.IsInstanceOfType(handledException, typeof(FaultContractWrapperException));
            Assert.AreEqual(Resources.WcfMessageResource, handledException.Message);
        }
    }

    [TestClass]
    public class GivenFaultContractExceptionHandlerWithExceptionMessage : ArrangeActAssert
    {
        private FaultContractExceptionHandlerData configuration;
        private const string ErrorMessage = "Exception message";
        protected override void Arrange()
        {
            configuration = new FaultContractExceptionHandlerData("wcf", typeof(MockFaultContract).AssemblyQualifiedName)
            {
                ExceptionMessage = ErrorMessage
            };
        }

        [TestMethod]
        public void WhenCreatingHandler_ThenUsesMessage()
        {
            var handler = (FaultContractExceptionHandler)this.configuration.BuildExceptionHandler();

            var exception = new Exception();
            var handledException = handler.HandleException(exception, Guid.NewGuid());
            Assert.IsInstanceOfType(handledException, typeof(FaultContractWrapperException));
            Assert.AreEqual(ErrorMessage, handledException.Message);
        }
    }

    [TestClass]
    public class GivenFaultContractExceptionHandlerWithOverridenMessage : ArrangeActAssert
    {
        private FaultContractExceptionHandlerData configuration;
        private const string ErrorMessage = "Exception message";
        protected override void Arrange()
        {
            configuration = new FaultContractExceptionHandlerData("wcf", typeof(MockFaultContract).AssemblyQualifiedName)
            {
                ExceptionMessage = ErrorMessage,
                ExceptionMessageResourceName = "WcfMessageResource",
                ExceptionMessageResourceType = typeof(Resources).AssemblyQualifiedName
            };
        }

        [TestMethod]
        public void WhenCreatingHandler_ThenUsesMessage()
        {
            var handler = (FaultContractExceptionHandler)this.configuration.BuildExceptionHandler();

            var exception = new Exception();
            var handledException = handler.HandleException(exception, Guid.NewGuid());
            Assert.IsInstanceOfType(handledException, typeof(FaultContractWrapperException));
            Assert.AreEqual(Resources.WcfMessageResource, handledException.Message);
        }
    }
}
