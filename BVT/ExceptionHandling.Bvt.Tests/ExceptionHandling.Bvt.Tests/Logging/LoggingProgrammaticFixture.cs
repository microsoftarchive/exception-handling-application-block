// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.TestUtilities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Bvt.Tests.Logging
{
    [TestClass]
    public class LoggingProgrammaticFixture
    {
        [TestMethod]
        public void CreatesLoggingExceptionHandlerWithProgrammaticConfiguration()
        {
            var loggingConfiguration = new LoggingConfiguration();
            var eventLog = new EventLog("Application", ".", "ExceptionHandling.Bvt.Tests - Programmatic");
            var eventLogTraceListener = new FormattedEventLogTraceListener(eventLog, new TextFormatter("{message}"));
            loggingConfiguration.AddLogSource("Sample Category", SourceLevels.All, true).AddTraceListener(eventLogTraceListener);
            var logWriter = new LogWriter(loggingConfiguration);

            var excepionPolicies = new List<ExceptionPolicyDefinition>();
            var excepionPolicyEntries = new List<ExceptionPolicyEntry>();
            var exceptionHandler = new LoggingExceptionHandler("Sample Category", 100, TraceEventType.Transfer, "Sample Title", 4, typeof(TextExceptionFormatter), logWriter);
            var exceptionPolicy = new ExceptionPolicyEntry(typeof(DivideByZeroException), PostHandlingAction.None, new IExceptionHandler[] { exceptionHandler });
            excepionPolicyEntries.Add(exceptionPolicy);
            excepionPolicies.Add(new ExceptionPolicyDefinition("Logging Policy", excepionPolicyEntries));

            var exceptionManager = new ExceptionManager(excepionPolicies);
            var exceptionTothrow = new DivideByZeroException("error message");
            exceptionManager.HandleException(exceptionTothrow, "Logging Policy");
            var entry = EventLogEntries.Last;
            Assert.IsTrue(entry.Message.Contains("Type : " + exceptionTothrow.GetType().FullName));
            Assert.IsTrue(entry.Message.Contains("Message : error message"));
        }
    }
}
