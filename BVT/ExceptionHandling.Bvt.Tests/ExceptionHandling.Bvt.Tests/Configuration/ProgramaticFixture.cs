// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.TestObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.Configuration
{
    [TestClass]
    public class ProgramaticFixture
    {
        [TestMethod]
        public void ThrowsArgumentExceptionWhenPolicyNameIsNull()
        {
            var excepionPolicies = new List<ExceptionPolicyDefinition>();
            
            ExceptionAssertHelper.Throws<ArgumentException>(() =>
                excepionPolicies.Add(new ExceptionPolicyDefinition(null, new List<ExceptionPolicyEntry>())));
        }

        [TestMethod]
        public void ThrowsArgumentExceptionWhenPolicyNameIsEmpty()
        {
            var excepionPolicies = new List<ExceptionPolicyDefinition>();

            ExceptionAssertHelper.Throws<ArgumentException>(() =>
                excepionPolicies.Add(new ExceptionPolicyDefinition(string.Empty, new List<ExceptionPolicyEntry>())));
        }

        [TestMethod]
        public void CreatesExceptionPolicyForNonePostHandlingAction()
        {
            var excepionPolicies = new List<ExceptionPolicyDefinition>();
            var excepionPolicyEntries = new List<ExceptionPolicyEntry>();

            var exceptionPolicy = new ExceptionPolicyEntry(typeof(Exception), PostHandlingAction.None, new IExceptionHandler[] { });
            excepionPolicyEntries.Add(exceptionPolicy);
            excepionPolicies.Add(new ExceptionPolicyDefinition("EH", excepionPolicyEntries));

            var exceptionManager = new ExceptionManager(excepionPolicies);
            bool rethrowRecommended = exceptionManager.HandleException(new Exception(), "EH");
            Assert.IsFalse(rethrowRecommended);
        }

        [TestMethod]
        public void CreatesExceptionPolicyForNotifyRethrowPostHandlingAction()
        {
            var excepionPolicies = new List<ExceptionPolicyDefinition>();
            var excepionPolicyEntries = new List<ExceptionPolicyEntry>();

            var exceptionPolicy = new ExceptionPolicyEntry(typeof(Exception), PostHandlingAction.NotifyRethrow, new IExceptionHandler[] { });
            excepionPolicyEntries.Add(exceptionPolicy);
            excepionPolicies.Add(new ExceptionPolicyDefinition("EH", excepionPolicyEntries));

            var exceptionManager = new ExceptionManager(excepionPolicies);
            bool rethrow = exceptionManager.HandleException(new Exception(), "EH");
            Assert.IsTrue(rethrow);
        }

        [TestMethod]
        public void CreatesExceptionPolicyForThrowNewExceptionPostHandlingAction()
        {
            var excepionPolicies = new List<ExceptionPolicyDefinition>();
            var excepionPolicyEntries = new List<ExceptionPolicyEntry>();

            var exceptionPolicy = new ExceptionPolicyEntry(typeof(Exception), PostHandlingAction.ThrowNewException, new IExceptionHandler[] { });
            excepionPolicyEntries.Add(exceptionPolicy);
            excepionPolicies.Add(new ExceptionPolicyDefinition("EH", excepionPolicyEntries));

            var exceptionManager = new ExceptionManager(excepionPolicies);
            var originalException = new Exception();
            ExceptionAssertHelper.Throws<Exception>(() => exceptionManager.HandleException(originalException, "EH"));

            Exception exception = null;
            bool rethrow = exceptionManager.HandleException(new Exception(), "EH", out exception);
            Assert.IsInstanceOfType(exception, typeof(Exception));
            Assert.IsTrue(rethrow);
        }

        [TestMethod]
        public void CreatesReplaceExceptionHandlerWithMessageForThrowNewExceptionPostHandlingAction()
        {
            var excepionPolicies = new List<ExceptionPolicyDefinition>();
            var excepionPolicyEntries = new List<ExceptionPolicyEntry>();

            var exceptionHandler = new ReplaceHandler("string", typeof(InvalidCastException));
            var exceptionPolicy = new ExceptionPolicyEntry(typeof(Exception), PostHandlingAction.ThrowNewException, new IExceptionHandler[] { exceptionHandler });
            excepionPolicyEntries.Add(exceptionPolicy);
            excepionPolicies.Add(new ExceptionPolicyDefinition("EH", excepionPolicyEntries));

            var exceptionManager = new ExceptionManager(excepionPolicies);
            ExceptionAssertHelper.Throws<InvalidCastException>(() => exceptionManager.HandleException(new Exception(), "EH"));

            Exception exception = null;
            bool rethrow = exceptionManager.HandleException(new Exception(), "EH", out exception);
            Assert.IsInstanceOfType(exception, typeof(InvalidCastException));
            Assert.AreEqual("string", exception.Message);
            Assert.IsNull(exception.InnerException);
            Assert.IsTrue(rethrow);
        }

        [TestMethod]
        public void CreatesWrapExceptionHandlerWithMessageForThrowNewExceptionPostHandlingAction()
        {
            var excepionPolicies = new List<ExceptionPolicyDefinition>();
            var excepionPolicyEntries = new List<ExceptionPolicyEntry>();

            var exceptionHandler = new WrapHandler("string", typeof(InvalidCastException));
            var exceptionPolicy = new ExceptionPolicyEntry(typeof(Exception), PostHandlingAction.ThrowNewException, new IExceptionHandler[] { exceptionHandler });
            excepionPolicyEntries.Add(exceptionPolicy);
            excepionPolicies.Add(new ExceptionPolicyDefinition("EH", excepionPolicyEntries));

            var exceptionManager = new ExceptionManager(excepionPolicies);
            Exception originalException = new Exception();
            ExceptionAssertHelper.Throws<InvalidCastException>(() => exceptionManager.HandleException(originalException, "EH"));

            Exception exception = null;
            bool rethrow = exceptionManager.HandleException(originalException, "EH", out exception);
            Assert.IsInstanceOfType(exception, typeof(InvalidCastException));
            Assert.AreSame(originalException, exception.InnerException);
            Assert.AreEqual("string", exception.Message);
            Assert.IsTrue(rethrow);
        }

        [TestMethod]
        public void CreatesCustomExceptionHandlerForNotifyRethrowPostHandlingAction()
        {
            var excepionPolicies = new List<ExceptionPolicyDefinition>();
            var excepionPolicyEntries = new List<ExceptionPolicyEntry>();

            var exceptionHandler = new CustomExceptionHandler(null);
            var exceptionPolicy = new ExceptionPolicyEntry(typeof(Exception), PostHandlingAction.NotifyRethrow, new IExceptionHandler[] { exceptionHandler });
            excepionPolicyEntries.Add(exceptionPolicy);
            excepionPolicies.Add(new ExceptionPolicyDefinition("EH", excepionPolicyEntries));

            var exceptionManager = new ExceptionManager(excepionPolicies);
            bool rethrow = exceptionManager.HandleException(new Exception(), "EH");
            Assert.IsTrue(rethrow);

            Exception exception = null;
            bool rethrowWithOutput = exceptionManager.HandleException(new Exception(), "EH", out exception);
            Assert.IsNull(exception);
            Assert.IsTrue(rethrowWithOutput);
        }

        [TestMethod]
        public void CreatesCustomExceptionHandlerForThrowNewExceptionPostHandlingAction()
        {
            var excepionPolicies = new List<ExceptionPolicyDefinition>();
            var excepionPolicyEntries = new List<ExceptionPolicyEntry>();

            var exceptionHandler = new CustomExceptionHandler(null);
            var exceptionPolicy = new ExceptionPolicyEntry(typeof(Exception), PostHandlingAction.ThrowNewException, new IExceptionHandler[] { exceptionHandler });
            excepionPolicyEntries.Add(exceptionPolicy);
            excepionPolicies.Add(new ExceptionPolicyDefinition("EH", excepionPolicyEntries));

            var exceptionManager = new ExceptionManager(excepionPolicies);
            ExceptionAssertHelper.Throws<NotImplementedException>(() => exceptionManager.HandleException(new Exception(), "EH"));

            Exception exception = null;
            bool rethrow = exceptionManager.HandleException(new Exception(), "EH", out exception);
            Assert.IsInstanceOfType(exception, typeof(NotImplementedException));
            Assert.IsTrue(rethrow);
        }
    }
}
