// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.TestObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.Wcf
{
    [TestClass]
    public class WcfProgramaticFixture
    {
        [TestMethod]
        public void CreatesWcfExceptionShildingProgrammatically()
        {
            var excepionPolicies = new List<ExceptionPolicyDefinition>();
            var excepionPolicyEntries = new List<ExceptionPolicyEntry>();
            var attributes = new NameValueCollection { { "Message", "{Message}" }, { "Data", "{Data}" }, { "SomeNumber", "{OffendingNumber}" } };
            var exceptionHandler = new FaultContractExceptionHandler(typeof(MockFaultContract), "fault message", attributes);
            var exceptionPolicy = new ExceptionPolicyEntry(typeof(NotFiniteNumberException), PostHandlingAction.ThrowNewException, new IExceptionHandler[] { exceptionHandler });
            excepionPolicyEntries.Add(exceptionPolicy);
            excepionPolicies.Add(new ExceptionPolicyDefinition("Wcf Shielding Policy", excepionPolicyEntries));

            var exceptionManager = new ExceptionManager(excepionPolicies);
            NotFiniteNumberException originalException = new NotFiniteNumberException("MyException", 555);
            originalException.Data.Add("someKey", "someValue");
            try
            {
                exceptionManager.HandleException(originalException, "Wcf Shielding Policy");
                Assert.Fail("a new exception should have been thrown");
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
