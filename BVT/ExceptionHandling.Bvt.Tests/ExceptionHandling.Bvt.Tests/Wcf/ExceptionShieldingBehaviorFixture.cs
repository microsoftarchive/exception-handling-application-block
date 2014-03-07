// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ServiceModel;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.Wcf
{
    [TestClass]
    public class ExceptionShieldingBehaviorFixture
    {
        [TestInitialize]
        public void Initialize()
        {
            ExceptionPolicy.SetExceptionManager(new ExceptionPolicyFactory().CreateManager(), false);
        }

        [TestCleanup]
        public void Cleanup()
        {
            ExceptionPolicy.Reset();
        }

        [TestMethod]
        public void ReturnsGenericShildingExceptionMessageWhenPolicyNameIsInvalid()
        {
            Uri serviceUri = new Uri("http://localhost:30003/Test");
            ServiceHost host = new ServiceHost(typeof(MockService2), serviceUri);
            host.AddServiceEndpoint(typeof(IMockService), new BasicHttpBinding(), serviceUri);
            host.Open();

            try
            {
                MockServiceProxy proxy = new MockServiceProxy();
                proxy.Test();
            }
            catch (FaultException fex)
            {
                Assert.IsTrue(fex.Message.Contains("An error has occurred while consuming this service. Please contact your administrator for more information."));
            }
            finally
            {
                if (host.State == CommunicationState.Opened)
                {
                    host.Close();
                }
            }
        }

        [TestMethod]
        public void ReplacesFaultExceptionMessageWhenPostHandlingIsThrowNewException()
        {
            Uri serviceUri = new Uri("http://localhost:30003/Test");
            ServiceHost host = new ServiceHost(typeof(MockService1), serviceUri);
            host.AddServiceEndpoint(typeof(IMockService), new BasicHttpBinding(), serviceUri);
            host.Open();

            try
            {
                MockServiceProxy proxy = new MockServiceProxy();
                proxy.Test();
            }
            catch (FaultException fex) 
            {
                Assert.AreEqual("Test EHAB - WCF", fex.Message);
            }
            finally
            {
                if (host.State == CommunicationState.Opened)
                {
                    host.Close();
                }
            }
        }

        [TestMethod]
        public void ReturnsOriginalExceptionMessageWhenPostHandlingIsThrowNewExceptionAndNoExceptionMessageIsConfiguredInThePolicy()
        {
            Uri serviceUri = new Uri("http://localhost:30003/Test");
            ServiceHost host = new ServiceHost(typeof(MockService3), serviceUri);
            host.AddServiceEndpoint(typeof(IMockService), new BasicHttpBinding(), serviceUri);
            host.Open();

            try
            {
                MockServiceProxy proxy = new MockServiceProxy();
                proxy.Test();
            }
            catch (FaultException fex)
            {
                Assert.AreEqual("MockService3 exception", fex.Message);
            }
            finally
            {
                if (host.State == CommunicationState.Opened)
                {
                    host.Close();
                }
            }
        }

        [TestMethod]
        public void ReturnsGenericShildingExceptionMessageWhenPostHandlingIsNone()
        {
            Uri serviceUri = new Uri("http://localhost:30003/Test");
            ServiceHost host = new ServiceHost(typeof(MockService4), serviceUri);
            host.AddServiceEndpoint(typeof(IMockService), new BasicHttpBinding(), serviceUri);
            host.Open();

            try
            {
                MockServiceProxy proxy = new MockServiceProxy();
                proxy.Test();
            }
            catch (FaultException fex)
            {
                Assert.IsTrue(fex.Message.Contains("An error has occurred while consuming this service. Please contact your administrator for more information."));
            }
            finally
            {
                if (host.State == CommunicationState.Opened)
                {
                    host.Close();
                }
            }
        }

        [TestMethod]
        public void ReturnsGenericShildingExceptionMessageWhenPostHandlingIsNotifyRethrow()
        {
            Uri serviceUri = new Uri("http://localhost:30003/Test");
            ServiceHost host = new ServiceHost(typeof(MockService5), serviceUri);
            host.AddServiceEndpoint(typeof(IMockService), new BasicHttpBinding(), serviceUri);
            host.Open();

            try
            {
                MockServiceProxy proxy = new MockServiceProxy();
                proxy.Test();
            }
            catch (FaultException fex)
            {
                Assert.IsTrue(fex.Message.Contains("An error has occurred while consuming this service. Please contact your administrator for more information."));
            }
            finally
            {
                if (host.State == CommunicationState.Opened)
                {
                    host.Close();
                }
            }
        }
    }
}
