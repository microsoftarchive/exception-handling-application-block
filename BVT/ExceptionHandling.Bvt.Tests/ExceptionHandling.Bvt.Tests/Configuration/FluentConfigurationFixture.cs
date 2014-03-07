// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.TestObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.TestUtilities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.Configuration
{
    [TestClass]
    public class FluentConfigurationFixture
    {
        [TestMethod]
        public void BuildsEmptyExceptionHandlingSettings()
        {
            DictionaryConfigurationSource emptyConfigSource = new DictionaryConfigurationSource();

            ConfigurationSourceBuilder builder = new ConfigurationSourceBuilder();
            builder.ConfigureExceptionHandling();
            builder.UpdateConfigurationWithReplace(emptyConfigSource);

            var settings = emptyConfigSource.GetSection(ExceptionHandlingSettings.SectionName) as ExceptionHandlingSettings;
            Assert.IsNotNull(settings);
            Assert.AreEqual(0, settings.ExceptionPolicies.Count);
        }

        [TestMethod]
        public void BuildsEmptyExceptionPolicyWithName()
        {
            DictionaryConfigurationSource emptyConfigSource = new DictionaryConfigurationSource();

            ConfigurationSourceBuilder builder = new ConfigurationSourceBuilder();
            builder.ConfigureExceptionHandling()
                .GivenPolicyWithName("Sample Policy");

            builder.UpdateConfigurationWithReplace(emptyConfigSource);

            var settings = emptyConfigSource.GetSection(ExceptionHandlingSettings.SectionName) as ExceptionHandlingSettings;
            Assert.IsNotNull(settings);
            Assert.AreEqual(1, settings.ExceptionPolicies.Count);
            var policy = settings.ExceptionPolicies.Get(0);
            Assert.AreEqual(0, policy.ExceptionTypes.Count);
        }

        [TestMethod]
        public void ThrowsArgumentExceptionWhenPolicyNameIsNull()
        {
            ConfigurationSourceBuilder builder = new ConfigurationSourceBuilder();

            ExceptionAssertHelper.Throws<ArgumentException>(() =>
                builder.ConfigureExceptionHandling()
                    .GivenPolicyWithName(null)
                        .ForExceptionType<Exception>().ThenDoNothing());
        }

        [TestMethod]
        public void ThrowsArgumentExceptionWhenPolicyNameIsEmpty()
        {
            ConfigurationSourceBuilder builder = new ConfigurationSourceBuilder();

            ExceptionAssertHelper.Throws<ArgumentException>(() =>
                builder.ConfigureExceptionHandling()
                    .GivenPolicyWithName(string.Empty)
                        .ForExceptionType<Exception>().ThenDoNothing());
        }

        [TestMethod]
        public void BuildsExceptionPolicyForNonePostHandlingAction()
        {
            ConfigurationSourceBuilder builder = new ConfigurationSourceBuilder();
            builder.ConfigureExceptionHandling()
                .GivenPolicyWithName("EH")
                    .ForExceptionType<Exception>().ThenDoNothing();

            DictionaryConfigurationSource source = new DictionaryConfigurationSource();
            builder.UpdateConfigurationWithReplace(source);

            var exceptionManager = ((ExceptionHandlingSettings)source.GetSection("exceptionHandling")).BuildExceptionManager();
            bool rethrowRecommended = exceptionManager.HandleException(new Exception(), "EH");
            Assert.IsFalse(rethrowRecommended);
        }

        [TestMethod]
        public void BuildsExceptionPolicyForNotifyRethrowPostHandlingAction()
        {
            ConfigurationSourceBuilder builder = new ConfigurationSourceBuilder();
            builder.ConfigureExceptionHandling()
                .GivenPolicyWithName("EH")
                    .ForExceptionType<Exception>().ThenNotifyRethrow();

            DictionaryConfigurationSource source = new DictionaryConfigurationSource();
            builder.UpdateConfigurationWithReplace(source);

            var exceptionManager = ((ExceptionHandlingSettings)source.GetSection("exceptionHandling")).BuildExceptionManager();
            bool rethrow = exceptionManager.HandleException(new Exception(), "EH");
            Assert.IsTrue(rethrow);
        }

        [TestMethod]
        public void BuildsExceptionPolicyForThrowNewExceptionPostHandlingAction()
        {
            ConfigurationSourceBuilder builder = new ConfigurationSourceBuilder();
            builder.ConfigureExceptionHandling()
                .GivenPolicyWithName("EH")
                    .ForExceptionType<Exception>().ThenThrowNewException();

            DictionaryConfigurationSource source = new DictionaryConfigurationSource();
            builder.UpdateConfigurationWithReplace(source);

            var exceptionManager = ((ExceptionHandlingSettings)source.GetSection("exceptionHandling")).BuildExceptionManager();
            var originalException = new Exception();
            ExceptionAssertHelper.Throws<Exception>(() => exceptionManager.HandleException(originalException, "EH"));

            Exception exception = null;
            bool rethrow = exceptionManager.HandleException(new Exception(), "EH", out exception);
            Assert.IsInstanceOfType(exception, typeof(Exception));
            Assert.IsTrue(rethrow);
        }

        [TestMethod]
        public void BuildsReplaceExceptionHandlerForThrowNewExceptionPostHandlingAction()
        {
            ConfigurationSourceBuilder builder = new ConfigurationSourceBuilder();
            builder.ConfigureExceptionHandling()
                .GivenPolicyWithName("EH")
                    .ForExceptionType<Exception>().ReplaceWith<InvalidCastException>().ThenThrowNewException();

            DictionaryConfigurationSource source = new DictionaryConfigurationSource();
            builder.UpdateConfigurationWithReplace(source);

            var exceptionManager = ((ExceptionHandlingSettings)source.GetSection("exceptionHandling")).BuildExceptionManager();
            ExceptionAssertHelper.Throws<InvalidCastException>(() => exceptionManager.HandleException(new Exception(), "EH"));
        
            Exception exception = null;
            bool rethrow = exceptionManager.HandleException(new Exception(), "EH", out exception);
            Assert.IsInstanceOfType(exception, typeof(InvalidCastException));
            Assert.IsNull(exception.InnerException);
            Assert.IsTrue(rethrow);
        }

        [TestMethod]
        public void BuildsReplaceExceptionHandlerWithMessageForThrowNewExceptionPostHandlingAction()
        {
            ConfigurationSourceBuilder builder = new ConfigurationSourceBuilder();
            builder.ConfigureExceptionHandling()
                .GivenPolicyWithName("EH")
                    .ForExceptionType<Exception>().ReplaceWith<InvalidCastException>().UsingMessage("string").ThenThrowNewException();

            DictionaryConfigurationSource source = new DictionaryConfigurationSource();
            builder.UpdateConfigurationWithReplace(source);

            var exceptionManager = ((ExceptionHandlingSettings)source.GetSection("exceptionHandling")).BuildExceptionManager();
            Exception exception = null;
            bool rethrow = exceptionManager.HandleException(new Exception(), "EH", out exception);
            Assert.IsInstanceOfType(exception, typeof(InvalidCastException));
            Assert.AreEqual("string", exception.Message);
            Assert.IsNull(exception.InnerException);
            Assert.IsTrue(rethrow);
        }

        [TestMethod]
        public void BuildsWrapExceptionHandlerForThrowNewExceptionPostHandlingAction()
        {
            ConfigurationSourceBuilder builder = new ConfigurationSourceBuilder();
            builder.ConfigureExceptionHandling()
                .GivenPolicyWithName("EH")
                    .ForExceptionType<Exception>().WrapWith<InvalidCastException>().ThenThrowNewException();

            DictionaryConfigurationSource source = new DictionaryConfigurationSource();
            builder.UpdateConfigurationWithReplace(source);

            var exceptionManager = ((ExceptionHandlingSettings)source.GetSection("exceptionHandling")).BuildExceptionManager();
            Exception originalException = new Exception();
            ExceptionAssertHelper.Throws<InvalidCastException>(() => exceptionManager.HandleException(originalException, "EH"));
        
            Exception exception = null;
            bool rethrow = exceptionManager.HandleException(originalException, "EH", out exception);
            Assert.IsInstanceOfType(exception, typeof(InvalidCastException));
            Assert.AreSame(originalException, exception.InnerException);
            Assert.IsTrue(rethrow);
        }

        [TestMethod]
        public void BuildsWrapExceptionHandlerWithMessageForThrowNewExceptionPostHandlingAction()
        {
            ConfigurationSourceBuilder builder = new ConfigurationSourceBuilder();
            builder.ConfigureExceptionHandling()
                .GivenPolicyWithName("EH")
                    .ForExceptionType<Exception>().WrapWith<InvalidCastException>().UsingMessage("string").ThenThrowNewException();

            DictionaryConfigurationSource source = new DictionaryConfigurationSource();
            builder.UpdateConfigurationWithReplace(source);

            var exceptionManager = ((ExceptionHandlingSettings)source.GetSection("exceptionHandling")).BuildExceptionManager();
            Exception originalException = new Exception();
            Exception exception = null;
            bool rethrow = exceptionManager.HandleException(originalException, "EH", out exception);
            Assert.IsInstanceOfType(exception, typeof(InvalidCastException));
            Assert.AreSame(originalException, exception.InnerException);
            Assert.AreEqual("string", exception.Message);
            Assert.IsTrue(rethrow);
        }

        [TestMethod]
        public void BuildsCustomExceptionHandlerForNotifyRethrowPostHandlingAction()
        {
            ConfigurationSourceBuilder builder = new ConfigurationSourceBuilder();
            builder.ConfigureExceptionHandling()
                .GivenPolicyWithName("EH")
                    .ForExceptionType<Exception>().HandleCustom<CustomExceptionHandler>().ThenNotifyRethrow();

            DictionaryConfigurationSource source = new DictionaryConfigurationSource();
            builder.UpdateConfigurationWithReplace(source);

            var exceptionManager = ((ExceptionHandlingSettings)source.GetSection("exceptionHandling")).BuildExceptionManager();
            bool rethrow = exceptionManager.HandleException(new Exception(), "EH");
            Assert.IsTrue(rethrow);

            Exception exception = null;
            bool rethrowWithOutput = exceptionManager.HandleException(new Exception(), "EH", out exception);
            Assert.IsNull(exception);
            Assert.IsTrue(rethrowWithOutput);
        }

        [TestMethod]
        public void BuildsCustomExceptionHandlerForThrowNewExceptionPostHandlingAction()
        {
            ConfigurationSourceBuilder builder = new ConfigurationSourceBuilder();
            builder.ConfigureExceptionHandling()
                .GivenPolicyWithName("EH")
                    .ForExceptionType<Exception>().HandleCustom<CustomExceptionHandler>().ThenThrowNewException();

            DictionaryConfigurationSource source = new DictionaryConfigurationSource();
            builder.UpdateConfigurationWithReplace(source);
            var exceptionManager = ((ExceptionHandlingSettings)source.GetSection("exceptionHandling")).BuildExceptionManager();

            ExceptionAssertHelper.Throws<NotImplementedException>(() => exceptionManager.HandleException(new Exception(), "EH"));

            Exception exception = null;
            bool rethrow = exceptionManager.HandleException(new Exception(), "EH", out exception);
            Assert.IsInstanceOfType(exception, typeof(NotImplementedException));
            Assert.IsTrue(rethrow);
        }

        [TestMethod]
        public void BuildsWrapAndReplaceAndCustomExceptionHandlersForAllPostHandlingActions()
        {
            DictionaryConfigurationSource emptyConfigSource = new DictionaryConfigurationSource();

            ConfigurationSourceBuilder builder = new ConfigurationSourceBuilder();
            builder.ConfigureExceptionHandling()
                .GivenPolicyWithName("Sample Policy")
                    .ForExceptionType(typeof(DivideByZeroException))
                        .ReplaceWith(typeof(CustomException))
                            .ThenDoNothing()
                .GivenPolicyWithName("Sample Policy 2")
                    .ForExceptionType(typeof(InvalidCastException))
                        .HandleCustom(typeof(CustomExceptionHandler))
                            .ThenNotifyRethrow()
                    .ForExceptionType(typeof(OutOfMemoryException))
                        .WrapWith(typeof(OverflowException))
                            .ThenThrowNewException();

            builder.UpdateConfigurationWithReplace(emptyConfigSource);

            ExceptionHandlingSettings settings = emptyConfigSource.GetSection(ExceptionHandlingSettings.SectionName) as ExceptionHandlingSettings;
            Assert.IsNotNull(settings);
            Assert.AreEqual(2, settings.ExceptionPolicies.Count);

            // Policy one
            var policy = settings.ExceptionPolicies.Get("Sample Policy");
            Assert.IsNotNull(policy);
            Assert.AreEqual(1, policy.ExceptionTypes.Count);
            var configuredException = policy.ExceptionTypes.Get(0);
            Assert.AreEqual(typeof(DivideByZeroException), configuredException.Type);
            Assert.AreEqual(PostHandlingAction.None, configuredException.PostHandlingAction);
            Assert.AreEqual(1, configuredException.ExceptionHandlers.Count);
            var handler = configuredException.ExceptionHandlers.Get(0) as ReplaceHandlerData;
            Assert.IsNotNull(handler);
            Assert.AreEqual(typeof(CustomException), handler.ReplaceExceptionType);

            // Policy 2
            policy = settings.ExceptionPolicies.Get("Sample Policy 2");
            Assert.IsNotNull(policy);
            Assert.AreEqual(2, policy.ExceptionTypes.Count);
            configuredException = policy.ExceptionTypes.Get(0);
            Assert.AreEqual(typeof(InvalidCastException), configuredException.Type);
            Assert.AreEqual(PostHandlingAction.NotifyRethrow, configuredException.PostHandlingAction);
            Assert.AreEqual(1, configuredException.ExceptionHandlers.Count);
            var customHandler = configuredException.ExceptionHandlers.Get(0) as CustomHandlerData;
            Assert.IsNotNull(customHandler);
            Assert.AreEqual(typeof(CustomExceptionHandler), customHandler.Type);
            configuredException = policy.ExceptionTypes.Get(1);
            Assert.AreEqual(typeof(OutOfMemoryException), configuredException.Type);
            Assert.AreEqual(PostHandlingAction.ThrowNewException, configuredException.PostHandlingAction);
            Assert.AreEqual(1, configuredException.ExceptionHandlers.Count);
            var wrapHandler = configuredException.ExceptionHandlers.Get(0) as WrapHandlerData;
            Assert.IsNotNull(wrapHandler);
            Assert.AreEqual(typeof(OverflowException), wrapHandler.WrapExceptionType);
        }
    }
}
