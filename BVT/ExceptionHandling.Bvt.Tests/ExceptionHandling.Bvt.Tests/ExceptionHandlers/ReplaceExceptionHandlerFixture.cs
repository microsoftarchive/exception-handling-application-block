// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.TestObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.TestUtilities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Globalization;
using System.Threading;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.ExceptionHandlers
{
    [TestClass]
    public class ReplaceExceptionHandlerFixture
    {
        private IConfigurationSource configurationSource;
        private ExceptionManager exceptionManager;

        [TestInitialize]
        public void Initialize()
        {
            this.configurationSource = new FileConfigurationSource(@"configurations\\ExceptionHandlers.config");

            this.exceptionManager = ((ExceptionHandlingSettings)this.configurationSource.GetSection("exceptionHandling")).BuildExceptionManager();
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
        public void ThrowsReplacedExceptionWhenPostHandlingIsThrowNewExceptionAndExceptionShouldBeHandled()
        {
            var originalException = new DBConcurrencyException();

            BusinessException thrownException = ExceptionAssertHelper.Throws<BusinessException>(() =>
                this.exceptionManager.HandleException(originalException, "Replace DBConcurrencyException with BusinessException, ThrowNewException postHandling"));

            Assert.IsNotNull(thrownException);
            Assert.IsTrue(typeof(BusinessException).IsAssignableFrom(thrownException.GetType()));
        }

        [TestMethod]
        public void ReplacesExceptionInOutParameterAndRecommendsRethrowingWhenPostHandlingIsThrowNewExceptionAndExceptionShouldBeHandled()
        {
            var originalException = new DBConcurrencyException();

            Exception exceptionToThrow;
            bool rethrowRecommended = this.exceptionManager.HandleException(
                originalException, 
                "Replace DBConcurrencyException with BusinessException, ThrowNewException postHandling", 
                out exceptionToThrow);

            Assert.IsTrue(rethrowRecommended);
            Assert.IsNotNull(exceptionToThrow);
            Assert.IsTrue(typeof(BusinessException).IsAssignableFrom(exceptionToThrow.GetType()));
        }

        [TestMethod]
        public void DoesNotThrowAndRecommendsRethrowingWhenPostHandlingIsThrowNewExceptionAndExceptionShouldNotBeHandled()
        {
            bool rethrowRecommended = false;

            ExceptionAssertHelper.DoesNotThrow(() =>
            {
                rethrowRecommended = this.exceptionManager.HandleException(
                    new ApplicationException(),
                    "Replace DBConcurrencyException with BusinessException, ThrowNewException postHandling");
            });

            Assert.IsTrue(rethrowRecommended);
        }

        [TestMethod]
        public void ProcessThrowsAndReplacesExceptionWhenPostHandlingIsThrowNewExceptionAndExceptionShouldBeHandled()
        {
            var originalException = new DBConcurrencyException();

            BusinessException thrownException = ExceptionAssertHelper.Throws<BusinessException>(() =>
                this.exceptionManager.Process(
                    () => { throw originalException; },
                    "Replace DBConcurrencyException with BusinessException, ThrowNewException postHandling"));

            Assert.IsNotNull(thrownException);
            Assert.IsTrue(typeof(BusinessException).IsAssignableFrom(thrownException.GetType()));
        }

        [TestMethod]
        public void ProcessThrowsOriginalExceptionWhenPostHandlingIsThrowNewExceptionAndExceptionShouldNotBeHandled()
        {
            var originalException = new ApplicationException();

            ApplicationException thrownException = ExceptionAssertHelper.Throws<ApplicationException>(() =>
                this.exceptionManager.Process(
                    () => { throw originalException; },
                    "Replace DBConcurrencyException with BusinessException, ThrowNewException postHandling"));

            Assert.AreSame(originalException, thrownException);
        }

        [TestMethod]
        public void ReturnsNullInOutParameterAndRecommendsRethrowingWhenPostHandlingIsNotifyRethrowAndExceptionShouldBeHandled()
        {
            var originalException = new DBConcurrencyException();

            Exception exceptionToThrow;
            bool rethrowRecommended = this.exceptionManager.HandleException(
                originalException,
                "Replace DBConcurrencyException with BusinessException, NotifyRethrow postHandling",
                out exceptionToThrow);

            Assert.IsTrue(rethrowRecommended);
            Assert.IsNull(exceptionToThrow);
        }

        [TestMethod]
        public void DoesNotThrowAndRecommendsRethrowingWhenPostHandlingIsNotifyRethrowAndExceptionShouldNotBeHandled()
        {
            bool rethrowRecommended = false;

            ExceptionAssertHelper.DoesNotThrow(() =>
            {
                rethrowRecommended = this.exceptionManager.HandleException(
                    new ApplicationException(),
                    "Replace DBConcurrencyException with BusinessException, NotifyRethrow postHandling");
            });

            Assert.IsTrue(rethrowRecommended);
        }

        [TestMethod]
        public void ProcessThrowsOriginalExceptionWhenPostHandlingIsNotifyRethrowAndExceptionShouldBeHandled()
        {
            var originalException = new DBConcurrencyException();

            DBConcurrencyException thrownException = ExceptionAssertHelper.Throws<DBConcurrencyException>(() =>
                this.exceptionManager.Process(
                    () => { throw originalException; },
                    "Replace DBConcurrencyException with BusinessException, NotifyRethrow postHandling"));

            Assert.AreSame(originalException, thrownException);
        }

        [TestMethod]
        public void ProcessThrowsOriginalExceptionWhenPostHandlingIsNotifyRethrowAndExceptionShouldNotBeHandled()
        {
            var originalException = new ApplicationException();

            ApplicationException thrownException = ExceptionAssertHelper.Throws<ApplicationException>(() =>
                this.exceptionManager.Process(
                    () => { throw originalException; },
                    "Replace DBConcurrencyException with BusinessException, NotifyRethrow postHandling"));

            Assert.AreSame(originalException, thrownException);
        }

        [TestMethod]
        public void ReturnsNullInOutParameterAndDoesNotRecommendRethrowingWhenPostHandlingIsNoneAndExceptionShouldBeHandled()
        {
            var originalException = new DBConcurrencyException();

            Exception exceptionToThrow;
            bool rethrowRecommended = this.exceptionManager.HandleException(
                originalException,
                "Replace DBConcurrencyException with BusinessException, None postHandling",
                out exceptionToThrow);

            Assert.IsFalse(rethrowRecommended);
            Assert.IsNull(exceptionToThrow);
        }

        [TestMethod]
        public void DoesNotThrowAndRecommendsRethrowingWhenPostHandlingIsNoneAndExceptionShouldNotBeHandled()
        {
            bool rethrowRecommended = false;

            ExceptionAssertHelper.DoesNotThrow(() =>
            {
                rethrowRecommended = this.exceptionManager.HandleException(
                    new ApplicationException(),
                    "Replace DBConcurrencyException with BusinessException, None postHandling");
            });

            Assert.IsTrue(rethrowRecommended);
        }

        [TestMethod]
        public void ProcessDoesNotThrowWhenPostHandlingIsNoneAndExceptionShouldBeHandled()
        {
            ExceptionAssertHelper.DoesNotThrow(() =>
                this.exceptionManager.Process(
                    () => { throw new DBConcurrencyException(); },
                    "Replace DBConcurrencyException with BusinessException, None postHandling"));
        }

        [TestMethod]
        public void ProcessThrowsOriginalExceptionWhenPostHandlingIsNoneAndExceptionShouldNotBeHandled()
        {
            var originalException = new ApplicationException();

            ApplicationException thrownException = ExceptionAssertHelper.Throws<ApplicationException>(() =>
                this.exceptionManager.Process(
                    () => { throw originalException; },
                    "Replace DBConcurrencyException with BusinessException, None postHandling"));

            Assert.AreSame(originalException, thrownException);
        }

        [TestMethod]
        public void ThrowsExceptionWhenExceptionToHandleIsNull()
        {
            ExceptionAssertHelper.Throws<ArgumentNullException>(() =>
                this.exceptionManager.HandleException(null, "Replace DBConcurrencyException with BusinessException, ThrowNewException postHandling"));
        }

        [TestMethod]
        public void GetsExceptionMessageFromResourceFileWhenExceptionMessageResourceIsConfigured()
        {
            BusinessException thrownException = ExceptionAssertHelper.Throws<BusinessException>(() =>
                this.exceptionManager.HandleException(new DBConcurrencyException(), "Replace DBConcurrencyException with BusinessException, ThrowNewException postHandling"));

            Assert.AreEqual("Message to be translated in English", thrownException.Message);
        }

        [TestMethod]
        public void TranslatesExceptionMessageFromResourceFileWhenExceptionMessageResourceIsConfigured()
        {
            DBConcurrencyException exceptionThatIsHandled = new DBConcurrencyException();
            Exception exceptionToThrowEnglish;
            this.exceptionManager.HandleException(exceptionThatIsHandled, "Replace DBConcurrencyException with BusinessException, ThrowNewException postHandling", out exceptionToThrowEnglish);

            CultureInfo originalCulture = Thread.CurrentThread.CurrentUICulture;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-Ca");
            Exception exceptionToThrowFrench;
            try
            {
                this.exceptionManager.HandleException(exceptionThatIsHandled, "Replace DBConcurrencyException with BusinessException, ThrowNewException postHandling", out exceptionToThrowFrench);
            }
            finally
            {
                Thread.CurrentThread.CurrentUICulture = originalCulture;
            }

            Assert.IsTrue(exceptionToThrowEnglish.Message.Contains("English"), "English problem");
            Assert.IsTrue(exceptionToThrowFrench.Message.Contains("French"), "French problem");
            Assert.AreNotEqual(exceptionToThrowEnglish.Message, exceptionToThrowFrench.Message);
        }

        [TestMethod]
        public void ExceptionPolicyHandleExceptionThrowsReplacedExceptionWhenPostHandlingIsThrowNewExceptionAndExceptionShouldBeHandled()
        {
            var originalException = new DBConcurrencyException();

            ExceptionPolicy.SetExceptionManager(this.exceptionManager, false);
            BusinessException thrownException = ExceptionAssertHelper.Throws<BusinessException>(() =>
                ExceptionPolicy.HandleException(originalException, "Replace DBConcurrencyException with BusinessException, ThrowNewException postHandling"));

            Assert.IsNotNull(thrownException);
            Assert.IsTrue(typeof(BusinessException).IsAssignableFrom(thrownException.GetType()));
        }

        [TestMethod]
        public void ExceptionPolicyHandleExceptionDoesNotThrowAndRecommendsRethrowingWhenPostHandlingIsThrowNewExceptionAndExceptionShouldNotBeHandled()
        {
            bool rethrowRecommended = false;

            ExceptionPolicy.SetExceptionManager(this.exceptionManager, false);
            ExceptionAssertHelper.DoesNotThrow(() =>
            {
                rethrowRecommended = ExceptionPolicy.HandleException(
                    new ApplicationException(),
                    "Replace DBConcurrencyException with BusinessException, ThrowNewException postHandling");
            });

            Assert.IsTrue(rethrowRecommended);
        }
    }
}
