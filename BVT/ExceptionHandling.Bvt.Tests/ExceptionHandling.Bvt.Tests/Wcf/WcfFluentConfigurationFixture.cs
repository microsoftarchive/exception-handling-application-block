// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.TestObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.Wcf
{
    [TestClass]
    public class WcfFluentConfigurationFixture
    {
        [TestMethod]
        public void BuildsWcfExceptionShildingWithFluentConfiguration()
        {
            DictionaryConfigurationSource emptyConfigSource = new DictionaryConfigurationSource();

            ConfigurationSourceBuilder builder = new ConfigurationSourceBuilder();
            builder.ConfigureExceptionHandling()
                .GivenPolicyWithName("policy")
                .ForExceptionType(typeof(NotFiniteNumberException))
                .ShieldExceptionForWcf(typeof(MockFaultContract), "fault message")
                    .MapProperty("Message", "{Message}")
                    .MapProperty("Data", "{Data}")
                    .MapProperty("SomeNumber", "{OffendingNumber}")
                .ThenThrowNewException();

            builder.UpdateConfigurationWithReplace(emptyConfigSource);

            ExceptionHandlingSettings settings = emptyConfigSource.GetSection(ExceptionHandlingSettings.SectionName) as ExceptionHandlingSettings;
            Assert.IsNotNull(settings);
            Assert.AreEqual(1, settings.ExceptionPolicies.Count);
            var policy = settings.ExceptionPolicies.Get("policy");
            Assert.IsNotNull(policy);
            Assert.AreEqual(1, policy.ExceptionTypes.Count);
            var configuredException = policy.ExceptionTypes.Get(0);
            Assert.AreEqual(typeof(NotFiniteNumberException), configuredException.Type);
            Assert.AreEqual(PostHandlingAction.ThrowNewException, configuredException.PostHandlingAction);
            Assert.AreEqual(1, configuredException.ExceptionHandlers.Count);

            var handler = configuredException.ExceptionHandlers.Get(0) as FaultContractExceptionHandlerData;
            Assert.IsNotNull(handler);
            Assert.AreEqual(typeof(MockFaultContract).AssemblyQualifiedName, handler.FaultContractType);
            Assert.AreEqual("fault message", handler.ExceptionMessage);
            Assert.AreEqual(3, handler.PropertyMappings.Count);
            Assert.IsNotNull(handler.PropertyMappings.SingleOrDefault(p => p.Name == "Message" && p.Source == "{Message}"));
            Assert.IsNotNull(handler.PropertyMappings.SingleOrDefault(p => p.Name == "Data" && p.Source == "{Data}"));
            Assert.IsNotNull(handler.PropertyMappings.SingleOrDefault(p => p.Name == "SomeNumber" && p.Source == "{OffendingNumber}"));

            var exceptionManager = settings.BuildExceptionManager();
            NotFiniteNumberException originalException = new NotFiniteNumberException("MyException", 555);
            originalException.Data.Add("someKey", "someValue");
            try
            {
                exceptionManager.HandleException(originalException, "policy");
                Assert.Fail("Should have thrown");
            }
            catch (FaultContractWrapperException ex)
            {
                MockFaultContract fault = (MockFaultContract)ex.FaultContract;
                Assert.AreEqual(originalException.Message, fault.Message);
                Assert.AreEqual(originalException.Data.Count, fault.Data.Count);
                Assert.AreEqual(originalException.Data["someKey"], fault.Data["someKey"]);
                Assert.AreEqual(originalException.OffendingNumber, fault.SomeNumber);
            }
        }
    }
}
