// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.TestUtilities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.Logging
{
    [TestClass]
    public class LoggingFluentConfigurationFixture
    {
        [TestCleanup]
        public void Cleanup()
        {
            Logger.Reset();
        }

        [TestMethod]
        public void BuildsLoggingExceptionHandlerWithFluentConfiguration()
        {
            DictionaryConfigurationSource configSource = new DictionaryConfigurationSource();

            ConfigurationSourceBuilder builder = new ConfigurationSourceBuilder();
            builder.ConfigureExceptionHandling()
                .GivenPolicyWithName("Logging Policy")
                    .ForExceptionType(typeof(DivideByZeroException))
                        .LogToCategory("Sample Category")
                            .UsingEventId(100)
                            .UsingExceptionFormatter(typeof(TextExceptionFormatter))
                            .UsingTitle("Sample Title")
                            .WithPriority(4)
                            .WithSeverity(TraceEventType.Transfer)
                    .ThenDoNothing();

            builder.ConfigureLogging()
                .LogToCategoryNamed("Sample Category")
                .WithOptions
                .SetAsDefaultCategory()
                .ToSourceLevels(SourceLevels.All)
                .SendTo
                    .EventLog("Default Listener")
                    .UsingEventLogSource("ExceptionHandling.Bvt.Tests - Fluent")
                    .FormatWith(
                        new FormatterBuilder()
                            .TextFormatterNamed("Default Text Formatter")
                            .UsingTemplate("{message}"));

            builder.UpdateConfigurationWithReplace(configSource);

            ExceptionHandlingSettings settings = configSource.GetSection(ExceptionHandlingSettings.SectionName) as ExceptionHandlingSettings;
            Assert.IsNotNull(settings);
            Assert.AreEqual(1, settings.ExceptionPolicies.Count);
            var policy = settings.ExceptionPolicies.Get("Logging Policy");
            Assert.IsNotNull(policy);
            Assert.AreEqual(1, policy.ExceptionTypes.Count);
            var configuredException = policy.ExceptionTypes.Get(0);
            Assert.AreEqual(typeof(DivideByZeroException), configuredException.Type);
            Assert.AreEqual(PostHandlingAction.None, configuredException.PostHandlingAction);
            Assert.AreEqual(1, configuredException.ExceptionHandlers.Count);

            var handler = configuredException.ExceptionHandlers.Get(0) as LoggingExceptionHandlerData;
            Assert.IsNotNull(handler);
            Assert.AreEqual("Sample Category", handler.LogCategory);
            Assert.AreEqual(4, handler.Priority);
            Assert.AreEqual(TraceEventType.Transfer, handler.Severity);
            Assert.AreEqual("Sample Title", handler.Title);
            Assert.AreEqual(100, handler.EventId);
            Assert.AreEqual(typeof(TextExceptionFormatter), handler.FormatterType);

            var factory = new LogWriterFactory((e) => configSource.GetSection(e));
            Logger.SetLogWriter(factory.Create());
            var exceptionManager = settings.BuildExceptionManager();
            var exceptionTothrow = new DivideByZeroException("error message");
            exceptionManager.HandleException(exceptionTothrow, "Logging Policy");
            var entry = EventLogEntries.Last;
            Assert.IsTrue(entry.Message.Contains("Type : " + exceptionTothrow.GetType().FullName));
            Assert.IsTrue(entry.Message.Contains("Message : error message"));
        }
    }
}
